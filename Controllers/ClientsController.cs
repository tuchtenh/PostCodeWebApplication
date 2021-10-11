using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PostCodeWebApplication.Models;
using PostCodeWebApplication.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PostCodeWebApplication.Controllers
{

    public class ClientsController : Controller
    {
        private readonly Models.DatabaseContext _context;
        private readonly IPostCodeService _pcservice;

        public ClientsController(Models.DatabaseContext dbContext, IPostCodeService postcodeservice)
        {
            _context = dbContext;
            _pcservice = postcodeservice;
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

            try
            {
                if (file == null)
                {
                    throw new Exception("No file attached");
                }

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = reader.ReadToEnd();
                }

                List<Clients> userObjects;
                userObjects = JsonConvert.DeserializeObject<List<Clients>>(content);

                foreach (var user in userObjects)
                {
                    var client = _context.Clients.FirstOrDefault(x => string.Equals(user.Name, x.Name));
                    if (client != null)
                    {
                        client.Address = user.Address;
                        client.PostCode = user.PostCode;
                    }
                    else
                    {
                        Clients us = new Clients
                        {
                            Name = user.Name,
                            Address = user.Address,
                            PostCode = user.PostCode
                        };

                        _context.Add(us);
                    }
                    _context.SaveChanges();
                }

                return View("~/Views/Clients/UploadSuccess.cshtml");
            }
            catch (Exception)
            {

                return View("~/Views/Clients/UploadFailed.cshtml");
                //return BadRequest(ex.GetType().FullName);
            }

        }

        //URL Client - Update button - Updates PostCode from postit.lt
        [HttpPost]
        public async Task<IActionResult> Update()
        {

            List<Clients> userObjects = _context.Clients.ToList();
            foreach (var user in userObjects)
            {
                Clients us = new Clients
                {
                    Address = user.Address
                };
                try
                {
                    user.PostCode = await _pcservice.GetPostCode(us.Address);
                    if (user.PostCode == null)
                    {
                        throw new Exception("Failed to retrieve a valid PostCode");
                    }
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    return View("~/Views/Clients/UpdateFailed.cshtml");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
