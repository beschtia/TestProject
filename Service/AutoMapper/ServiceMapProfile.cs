using AutoMapper;
using Service.EFModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.AutoMapper
{
    class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            CreateMap<IVehicleMake, VehicleMake>();
            CreateMap<IVehicleModel, VehicleModel>();
        }
    }
}
