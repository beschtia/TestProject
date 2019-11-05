using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public class VehicleService : IVehicleService, IDisposable
    {
        private readonly ProjectContext dbContext;
        private IGenericRepository<VehicleMake> makeRepository;
        private IGenericRepository<VehicleModel> modelRepository;

        public VehicleService(ProjectContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IGenericRepository<VehicleMake> MakeRepository
        {
            get
            {
                if (this.makeRepository == null)
                {
                    this.makeRepository = new GenericRepository<VehicleMake>(dbContext);
                }
                return makeRepository;
            }
        }

        public IGenericRepository<VehicleModel> ModelRepository
        {
            get
            {
                if (this.modelRepository == null)
                {
                    this.modelRepository = new GenericRepository<VehicleModel>(dbContext);
                }
                return modelRepository;
            }
        }

        public async Task<IPagedList<VehicleMake>> GetPagedMakes(FilteringModel filterModel, SortingModel sortingModel, PagingModel pagingModel)
        {
            Expression<Func<VehicleMake, bool>> filter = null;
            if (filterModel != null && !string.IsNullOrEmpty(filterModel.Filter))
            {
                filter = e => e.Name.ToUpper().Contains(filterModel.Filter.ToUpper()) ||
                                e.Abrv.ToUpper().Contains(filterModel.Filter.ToUpper());
            }

            Func<IQueryable<VehicleMake>, IOrderedQueryable<VehicleMake>> orderBy = sortingModel.SortParameter switch
            {
                "Name" => q => q.OrderBy(s => s.Name),
                "name_desc" => q => q.OrderByDescending(s => s.Name),
                "Abrv" => q => q.OrderBy(s => s.Abrv),
                "abrv_desc" => q => q.OrderByDescending(s => s.Abrv),
                _ => null,
            };

            return await MakeRepository.GetPagedAsync(pagingModel.Page, pagingModel.PageSize, filter, orderBy);
        }

        public async Task<IPagedList<VehicleModel>> GetPagedModels(FilteringModel filterModel, SortingModel sortingModel, PagingModel pagingModel)
        {
            Expression<Func<VehicleModel, bool>> filter = null;
            if (filterModel.FilterById != null)
            {
                filter = e => e.MakeId == filterModel.FilterById;
            }

            Func<IQueryable<VehicleModel>, IOrderedQueryable<VehicleModel>> orderBy = sortingModel.SortParameter switch
            {
                "Name" => q => q.OrderBy(e => e.Name),
                "name_desc" => q => q.OrderByDescending(e => e.Name),
                "Abrv" => q => q.OrderBy(e => e.Abrv),
                "abrv_desc" => q => q.OrderByDescending(e => e.Abrv),
                "Make" => q => q.OrderBy(e => e.Make.Name),
                "make_desc" => q => q.OrderByDescending(e => e.Make.Name),
                _ => null,
            };

            return await ModelRepository.GetPagedAsync(pagingModel.Page, pagingModel.PageSize, filter, orderBy, "Make");
        }

        public async Task<IEnumerable<VehicleMake>> GetVehicleMakesDropDownList()
        {
            return await MakeRepository.GetAllAsync(q => q.OrderBy(e => e.Name));
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
