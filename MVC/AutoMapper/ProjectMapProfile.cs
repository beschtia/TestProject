using AutoMapper;
using MVC.Models;
using MVC.ViewModels;
using Service;
using Service.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MVC.AutoMapper
{
    public class ProjectMapProfile : Profile
    {
        public ProjectMapProfile()
        {
            //CreateMap<VehicleMake, VehicleMakeViewModel>();
            //CreateMap<VehicleMake, VehicleMakeViewModel>().ReverseMap();
            //CreateMap<VehicleModel, VehicleModelViewModel>()
            //    .ForMember(dest => dest.Make,
            //                opts => opts.MapFrom(
            //                    src => string.Format("{0} ({1})", src.Make.Name, src.Make.Abrv)));
            //CreateMap<VehicleModelViewModel, VehicleModel>();
            //CreateMap<VehicleMake, VehicleMakeDropListModel>()
            //    .ForMember(dest => dest.Name,
            //                opts => opts.MapFrom(
            //                    src => string.Format("{0} ({1})", src.Name, src.Abrv)));
            //CreateMap<IPagedList<VehicleModel>, IPagedList<VehicleModelViewModel>>().ConvertUsing(new PagedListTypeConverter<VehicleModel, VehicleModelViewModel>());
            //CreateMap<IPagedList<VehicleMake>, IPagedList<VehicleMakeViewModel>>().ConvertUsing(new PagedListTypeConverter<VehicleMake, VehicleMakeViewModel>());

            CreateMap<IVehicleMake, VehicleMakeViewModel>();
            CreateMap<VehicleMakeViewModel, IVehicleMake>();
            CreateMap<IPagedList<IVehicleMake>, IPagedList<VehicleMakeViewModel>>()
                .ConvertUsing(new PagedListTypeConverter<IVehicleMake, VehicleMakeViewModel>());

            CreateMap<IVehicleModel, VehicleModelViewModel>()
                .ForMember(dest => dest.Make,
                            opts => opts.MapFrom(
                                src => string.Format("{0} ({1})", src.Make.Name, src.Make.Abrv)));
            CreateMap<VehicleModelViewModel, IVehicleModel>();
            CreateMap<IPagedList<IVehicleModel>, IPagedList<VehicleModelViewModel>>()
                .ConvertUsing(new PagedListTypeConverter<IVehicleModel, VehicleModelViewModel>());
            
            CreateMap<IVehicleMake, VehicleMakeDropListModel>()
                .ForMember(dest => dest.Name,
                            opts => opts.MapFrom(
                                src => string.Format("{0} ({1})", src.Name, src.Abrv)));
        }
    }
}
