using Service.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service
{
    public partial class VehicleModel : IVehicleModel
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Abrv { get; set; }

        [ForeignKey("MakeId")]
        [InverseProperty("VehicleModel")]
        public virtual VehicleMake Make { get; set; }
    }
}