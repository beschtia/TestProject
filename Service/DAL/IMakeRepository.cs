using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public interface IMakeRepository : IGenericRepository<VehicleMake>
    {
        Task<IPagedList<VehicleMake>> GetPagedMakesAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
    }
}
