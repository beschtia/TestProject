using Service.Models;
using X.PagedList;

namespace MVC.Models
{
    public class ModelsViewModel
    {
        public ModelsViewModel()
        {
            Sorting = new SortingModel();
            Filtering = new FilteringModel();
            Paging = new PagingModel();
        }

        public FilteringModel Filtering { get; set; }
        public SortingModel Sorting { get; set; }
        public PagingModel Paging { get; set; }
        public IPagedList<VehicleModelViewModel> Models { get; set; }
    }
}
