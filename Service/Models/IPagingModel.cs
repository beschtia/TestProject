using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public interface IPagingModel
    {
        int PageSize { get; set; }
        int Page { get; set; }
    }
}
