using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Service.Models;
using X.PagedList;

namespace Service.DAL
{
    public class ModelRepository : GenericRepository<VehicleModel>, IModelRepository
    {
        public ModelRepository(ProjectContext dbContext) : base(dbContext)
        {
        }

        public Task<IPagedList<VehicleModel>> GetPagedModelsAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
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

            return GetPagedAsync(pagingModel.Page, pagingModel.PageSize, filter, orderBy, "Make");
        }

        public Task<VehicleModel> GetModelWithDetailsAsync(int id)
        {
            return GetOneAsync(e => e.Id == id, "Make");
        }       
    }
}
