using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.DAL
{
    public class VehicleService : IVehicleService, IDisposable
    {
        private readonly ProjectContext dbContext;
        private IGenericService<VehicleMake> makeService;
        private IGenericService<VehicleModel> modelService;

        public VehicleService(ProjectContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IGenericService<VehicleMake> MakeService
        {
            get
            {
                if (this.makeService == null)
                {
                    this.makeService = new GenericService<VehicleMake>(dbContext);
                }
                return makeService;
            }
        }

        public IGenericService<VehicleModel> ModelService
        {
            get
            {
                if (this.modelService == null)
                {
                    this.modelService = new GenericService<VehicleModel>(dbContext);
                }
                return modelService;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
