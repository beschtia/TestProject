using AutoMapper;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class ProjectMapProfile : Profile
    {
        public ProjectMapProfile()
        {

            CreateMap<VehicleMake, VehicleMakeViewModel>();
            CreateMap<VehicleMake, VehicleMakeViewModel>().ReverseMap();
            CreateMap<VehicleModel, VehicleModelViewModel>()
                .ForMember(dest => dest.Make,
                            opts => opts.MapFrom(
                                src => string.Format("{0} ({1})", src.Make.Name, src.Make.Abrv)));
            CreateMap<VehicleModelViewModel, VehicleModel>();
            CreateMap<VehicleMake, VehicleMakeDropListModel>()
                .ForMember(dest => dest.Name,
                            opts => opts.MapFrom(
                                src => string.Format("{0} ({1})", src.Name, src.Abrv)));
        }
    }
}
