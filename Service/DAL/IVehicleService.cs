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
        IGenericRepository<VehicleMake> MakeRepository { get; }
        IGenericRepository<VehicleModel> ModelRepository { get; }
        Task<IPagedList<VehicleMake>> GetPagedMakesAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        Task InsertMakeAsync(IVehicleMake makeDTO);
        Task UpdateMakeAsync(IVehicleMake makeDTO);

        Task<IPagedList<VehicleModel>> GetPagedModelsAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        Task<VehicleModel> GetModelWithDetailsAsync(int id);
        Task InsertModelAsync(IVehicleModel modelDTO);
        Task UpdateModelAsync(IVehicleModel modelDTO);

        void Dispose();

    }
}
