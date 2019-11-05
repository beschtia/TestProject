using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public class PagingModel
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
