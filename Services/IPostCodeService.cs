using System.Threading.Tasks;

namespace PostCodeWebApplication.Services
{
    public interface IPostCodeService
    {
        Task<string> GetPostCode(string address);
    }
}
