using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL
{
    public interface IVehicleService
    {
        IGenericService<VehicleMake> MakeService { get; }
        IGenericService<VehicleModel> ModelService { get; }
        Task<int> SaveAsync();
        void Dispose();

    }
}
