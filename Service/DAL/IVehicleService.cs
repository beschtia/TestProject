using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public interface IVehicleService
    {
        IGenericRepository<VehicleMake> MakeRepository { get; }
        IGenericRepository<VehicleModel> ModelRepository { get; }
        Task<IPagedList<VehicleMake>> GetPagedMakes(FilteringModel filterModel, SortingModel sortingModel, PagingModel pagingModel);
        Task<IPagedList<VehicleModel>> GetPagedModels(FilteringModel filterModel, SortingModel sortingModel, PagingModel pagingModel);
        Task<IEnumerable<VehicleMake>> GetVehicleMakesDropDownList();
        Task<int> SaveAsync();
        void Dispose();

    }
}
