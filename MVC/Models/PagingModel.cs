using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class PagingModel : IPagingModel
    {
        public PagingModel()
        {
            PageSize = 5;
            Page = 1;
        }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
