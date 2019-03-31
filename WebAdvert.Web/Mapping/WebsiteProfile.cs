using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebAdvert.Web.Models;
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
            CreateMap<AdvertType, SearchViewModel>().ReverseMap();
        }
    }
}
