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
        Task<IPagedList<IVehicleMake>> GetPagedMakes(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);
        Task<IVehicleMake> GetMakeByIdAsync(object id);
        void InsertMake(IVehicleMake makeDTO);
        void UpdateMake(IVehicleMake makeDTO);
        Task<IEnumerable<IVehicleMake>> GetMakes();

        Task<IPagedList<IVehicleModel>> GetPagedModels(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel);        
        Task<IVehicleModel> GetModelByIdAsync(int id);
        void InsertModel(IVehicleModel modelDTO);
        void UpdateModel(IVehicleModel modelDTO);
        Task SaveAsync();
        void Dispose();

    }
}
