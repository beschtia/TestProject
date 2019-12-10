using Service.EFModels;
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
        Task<IPagedList<VehicleMake>> GetPagedMakesAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        ValueTask<VehicleMake> GetMakeByIdAsync(object id);
        Task<List<VehicleMake>> GetMakesAsync();
        Task InsertMakeAsync(IVehicleMake makeDTO);
        Task UpdateMakeAsync(IVehicleMake makeDTO);
        Task DeleteMakeAsync(VehicleMake make);
        Task<int> GetMakesCountAsync();

        Task<IPagedList<VehicleModel>> GetPagedModelsAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        ValueTask<VehicleModel> GetModelByIdAsync(object id);
        Task<VehicleModel> GetModelWithDetailsAsync(int id);
        Task InsertModelAsync(IVehicleModel modelDTO);
        Task UpdateModelAsync(IVehicleModel modelDTO);
        Task DeleteModelAsync(VehicleModel model);
        Task<int> GetModelCountAsync();
    }
}
