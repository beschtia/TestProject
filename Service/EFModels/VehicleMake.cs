using Service.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service
{
    public partial class VehicleMake : IVehicleMake
    {
        public VehicleMake()
        {
            VehicleModel = new HashSet<VehicleModel>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Abrv { get; set; }

        [InverseProperty("Make")]
        public virtual ICollection<VehicleModel> VehicleModel { get; set; }
    }
}