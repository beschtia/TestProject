using System;
using System.Collections.Generic;
using System.Text;

namespace Service.EFModels
{
    public interface IVehicleModel
    {
        int Id { get; set; }
        int MakeId { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
        VehicleMake Make { get; set; }
    }
}
