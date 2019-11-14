using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class FilteringModel : IFilteringModel
    {
        public FilteringModel()
        {
            Filter = "";
        }
        public string Filter { get; set; }
        public int? FilterById { get; set; }
    }
}
