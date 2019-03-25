using AdvertApi.Models;

namespace WebAdvert.Web.Services
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
        public string FilePath { get; set; }
    }
}