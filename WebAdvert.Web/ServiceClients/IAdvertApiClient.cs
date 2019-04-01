using System.Collections.Generic;
using System.Threading.Tasks;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<AdvertResponse> Create(CreateAdvertModel model);
        Task<bool> Confirm(ConfirmAdvertRequest model);
        Task<List<Advertisement>> GetAllAsync();
    }
}