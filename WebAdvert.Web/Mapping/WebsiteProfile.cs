using AdvertApi.Models;
using AutoMapper;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Models.Home;
using WebAdvert.Web.ServiceClients;

namespace WebAdvert.Web.Mapping
{
    public class WebsiteProfile : Profile
    {
        public WebsiteProfile()
        {
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();

            CreateMap<AdvertType, SearchViewModel>();
            CreateMap<AdvertModel, Advertisement>().ReverseMap();

            CreateMap<Advertisement, IndexViewModel>()
                .ForMember(dest => dest.Image, src => src.MapFrom(field => field.FilePath));
        }
    }
}
