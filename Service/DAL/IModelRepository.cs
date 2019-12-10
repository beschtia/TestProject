using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public interface IModelRepository : IGenericRepository<VehicleModel>
    {
        Task<IPagedList<VehicleModel>> GetPagedModelsAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        Task<VehicleModel> GetModelWithDetailsAsync(int id);
    }
}
