using System.Net.Http;
using System.Threading.Tasks;

namespace ImageGallery.Core
{
    public interface IHttpGetRequestSender
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}