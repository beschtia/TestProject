using AutoMapper;
using Service.AutoMapper;
using Service.EFModels;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Service.DAL
{
    public class VehicleService : IVehicleService
    {
        private readonly IMakeRepository _makeRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IMapper mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.AddProfile<ServiceMapProfile>();
            }));

        public VehicleService(IMakeRepository makeRepository, IModelRepository modelRepository)
        {
            _makeRepository = makeRepository;
            _modelRepository = modelRepository;
        }

        public Task<IPagedList<VehicleMake>> GetPagedMakesAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
        {
            return _makeRepository.GetPagedMakesAsync(filterModel, sortingModel, pagingModel);
        }

        public ValueTask<VehicleMake> GetMakeByIdAsync(object id)
        {
            return _makeRepository.GetByIdAsync(id);
        }

        public Task<List<VehicleMake>> GetMakesAsync()
        {
            return _makeRepository.GetAllAsync();
        }

        public Task InsertMakeAsync(IVehicleMake makeDTO)
        {
           return _makeRepository.InsertAsync(mapper.Map<VehicleMake>(makeDTO));
        }

        public Task UpdateMakeAsync(IVehicleMake makeDTO)
        {
            return _makeRepository.UpdateAsync(mapper.Map<VehicleMake>(makeDTO));
        }

        public Task DeleteMakeAsync(VehicleMake make)
        {
            return _makeRepository.DeleteAsync(make);
        }

        public Task<int> GetMakesCountAsync()
        {
            return _makeRepository.GetCountAsync();
        }

        public Task<IPagedList<VehicleModel>> GetPagedModelsAsync(IFilteringModel filterModel, ISortingModel sortingModel, IPagingModel pagingModel)
        {
            return _modelRepository.GetPagedModelsAsync(filterModel, sortingModel, pagingModel);
        }

        public ValueTask<VehicleModel> GetModelByIdAsync(object id)
        {
            return _modelRepository.GetByIdAsync(id);
        }

        public Task<VehicleModel> GetModelWithDetailsAsync(int id)
        {
            return _modelRepository.GetModelWithDetailsAsync(id);
        }

        public Task InsertModelAsync(IVehicleModel modelDTO)
        {
            return _modelRepository.InsertAsync(mapper.Map<VehicleModel>(modelDTO));
        }

        public Task UpdateModelAsync(IVehicleModel modelDTO)
        {
            return _modelRepository.UpdateAsync(mapper.Map<VehicleModel>(modelDTO));
        }

        public Task DeleteModelAsync(VehicleModel model)
        {
            return _modelRepository.DeleteAsync(model);
        }

        public Task<int> GetModelCountAsync()
        {
            return _modelRepository.GetCountAsync();
        }
    }
}
