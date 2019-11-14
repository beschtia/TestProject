using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Models
{
    public interface IFilteringModel
    {
        string Filter { get; set; }
        int? FilterById { get; set; }
    }
}
