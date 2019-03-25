using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertApi.Models;
using AutoMapper;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.Mapping
{
    public class AdvertModelsProfile : Profile
    {
        public AdvertModelsProfile()
        {
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();

            CreateMap<AdvertResponse, CreateAdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdvertModel>().ReverseMap();
        }
    }
}
