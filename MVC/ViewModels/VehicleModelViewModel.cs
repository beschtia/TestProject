using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewModels
{
    public class VehicleModelViewModel
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Abrv { get; set; }
        public string Make { get; set; }
    }
}
