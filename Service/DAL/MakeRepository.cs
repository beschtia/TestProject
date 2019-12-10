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
    public class MakeRepository : GenericRepository<VehicleMake>, IMakeRepository
    {
        public MakeRepository(ProjectContext dbContext) : base(dbContext)
        {
        }

        public Task<IPagedList<VehicleMake>> GetPagedMakesAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
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

            return GetPagedAsync(pagingModel.Page, pagingModel.PageSize, filter, orderBy);
        }
    }
}
