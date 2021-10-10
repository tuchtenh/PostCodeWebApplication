using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostCodeWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostCodeWebApplication.Controllers
{

    public class ClientsController : Controller
    {
        private readonly Models.DatabaseContext _context;

        public ClientsController(Models.DatabaseContext dbContext)
        {
            _context = dbContext;

        }

        //URL Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        //URL Clients/Upload
        public IActionResult Upload()
        {
            return View();
        }

        //URL Clients/Upload
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            var content = string.Empty;

            if (file != null)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = reader.ReadToEnd();
                }
            }
            else
            {
                return View("~/Views/Clients/UploadFailed.cshtml");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                content = reader.ReadToEnd();
            }
            List<Clients> userObjects;
            try
            {
                userObjects = JsonConvert.DeserializeObject<List<Clients>>(content);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetType().FullName);
            }
            //var valid = true;
            foreach (var user in userObjects)
            {
                Clients us = new Clients
                {
                    Name = user.Name,
                    Address = user.Address,
                    PostCode = user.PostCode
                };
                try
                {
                    _context.Add(us);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    /*_context.Entry(us).State = EntityState.Modified;
                    _context.SaveChanges();
                    valid = false;*/
                    return View("~/Views/Clients/UploadFailed.cshtml");
                }
            }
            /*if (!valid)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }*/
            return View("~/Views/Clients/UploadSuccess.cshtml");
        }

        //URL Client - Update button - Updates PostCodes from postit.lt
        public async Task<IActionResult> Update()
        {
            /*var client = factory.CreateClient();
            client.BaseAddress = new Uri("https://api.postit.lt");*/

            List<Clients> userObjects = _context.Clients.ToList();
            foreach (var user in userObjects)
            {
                Clients us = new Clients
                {
                    Address = user.Address
                };
                // TODO: url from appssettings Uri(WebAPIBaseUrl + "?address=" + Address + "&key=" + WebAPIKey);
                var url = "https://api.postit.lt" + $"/?address=" + user.Address + "&key=EqycqTXbjZU34nUTRmbO";

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        var PostItList = JsonConvert.DeserializeObject<PostItModel>(apiResponse);
                        try
                        {
                            // TODO: fix api request, currently PostItList.post_code = null
                            user.PostCode = PostItList.post_code;
                            _context.Entry(user).State = EntityState.Modified;
                            //_context.Update(user);
                            _context.SaveChanges();
                        }
                        catch (DbUpdateException ex)
                        {
                            //valid = false;
                            return View("~/Views/Clients/UploadFailed.cshtml");
                        }


                    }
                }
            }

            //try
            //{
            //    var json = await client.GetStringAsync($"/?address=" + user.Address + "&key=EqycqTXbjZU34nUTRmbO");
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.GetType().FullName);
            //}

            //var json = client.GetStringAsync($"/?address=" + user.Address + "&key=EqycqTXbjZU34nUTRmbO");
            //var content = JsonToken.Parse(json.post_code);

            //var x = JObject.Parse (content);
            //var post_code = x["data"]["post_code"];
            //user.PostCode = json.SelectToken("data.post_code");

            //user.PostCode = JsonConvert.DeserializeObject<Data>(json);

            //_context.Entry("Client").State = EntityState.Modified;
            //_context.SaveChanges();
            //WebAPIBaseUrl + "?address=" + Address + "&key=" + WebAPIKey;
            return View(await _context.Clients.ToListAsync());
        }
    }

}
