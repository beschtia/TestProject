using AutoMapper;
using Service.AutoMapper;
using Service.EFModels;
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
        private readonly IMapper mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.AddProfile<ServiceMapProfile>();
            }));


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

        public async Task<IPagedList<IVehicleMake>> GetPagedMakes(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
        {
            Expression<Func<VehicleMake, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(filterModel.Filter))
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

        public async Task<IPagedList<IVehicleModel>> GetPagedModels(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
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

        public async Task<IVehicleMake> GetMakeByIdAsync(object id)
        {
            return await MakeRepository.GetByIdAsync(id);
        }

        public void InsertMake(IVehicleMake makeDTO)
        {
            MakeRepository.Insert(mapper.Map<VehicleMake>(makeDTO));
        }

        public void UpdateMake(IVehicleMake makeDTO)
        {
            MakeRepository.Update(mapper.Map<VehicleMake>(makeDTO));
        }

        public async Task<IEnumerable<IVehicleMake>> GetMakes()
        {
            return await MakeRepository.GetAllAsync();
        }

        public async Task<IVehicleModel> GetModelByIdAsync(int id)
        {
            return await ModelRepository.GetOneAsync(v => v.Id == id, "Make");
        }

        public void InsertModel(IVehicleModel modelDTO)
        {
            ModelRepository.Insert(mapper.Map<VehicleModel>(modelDTO));
        }

        public void UpdateModel(IVehicleModel modelDTO)
        {
            ModelRepository.Update(mapper.Map<VehicleModel>(modelDTO));
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
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
