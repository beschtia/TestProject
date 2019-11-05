using Service.Models;
using X.PagedList;

namespace MVC.Models
{
    public class MakesViewModel
    {
        public MakesViewModel()
        {
            Sorting = new SortingModel();
            Filtering = new FilteringModel();
            Paging = new PagingModel();
        }
        public FilteringModel Filtering { get; set; }
        public SortingModel Sorting { get; set; }
        public PagingModel Paging { get; set; }
        public IPagedList<VehicleMakeViewModel> Makes { get; set; }
    }
}
