using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using Service;
using Service.DAL;
using X.PagedList;

namespace MVC.Controllers
{
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleModelsController(IVehicleService vehicleService, IMapper mapper)
        {
            this._vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(ModelsViewModel model)
        {
            PopulateMakesSelectList();
            ViewData["PageSize"] = new SelectList(new List<int> { 2, 5, 10, 25, 50, 100 });
            var models = await _vehicleService.GetPagedModels(model.Filtering, model.Sorting, model.Paging);
            model.Models = _mapper.Map<IPagedList<VehicleModel>, IPagedList<VehicleModelViewModel>>(models);
            return View(model);
        }
                
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModel = await _vehicleService.ModelRepository.GetOneAsync(v => v.Id == id, "Make");
            
            if (vehicleModel == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
            return View(viewModel);
        }

        
        public IActionResult Create()
        {
            PopulateMakesSelectList();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakeId,Name,Abrv")] VehicleModelViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicleModel = _mapper.Map<VehicleModel>(viewModel);
                    _vehicleService.ModelRepository.Insert(vehicleModel);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulateMakesSelectList(viewModel.MakeId);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModel = await _vehicleService.ModelRepository.GetByIdAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
            PopulateMakesSelectList(viewModel.MakeId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleModelViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicleModel = _mapper.Map<VehicleModel>(viewModel);
                    _vehicleService.ModelRepository.Update(vehicleModel);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateMakesSelectList(viewModel.MakeId);
            return View(viewModel);            
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var vehicleModel = await _vehicleService.ModelRepository.GetOneAsync(v => v.Id == id, "Make");
            if (vehicleModel == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicleModel = await _vehicleService.ModelRepository.GetByIdAsync(id);
                _vehicleService.ModelRepository.DeleteAsync(id);
                await _vehicleService.SaveAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        private void PopulateMakesSelectList(object selectedVehicleMake = null)
        {
            var makes = _vehicleService.GetVehicleMakesDropDownList().Result;
            var items = _mapper.Map<IEnumerable<VehicleMakeDropListModel>>(makes);
            ViewData["MakesSelectList"] = new SelectList(items, "Id", "Name", selectedVehicleMake);
        }

        protected override void Dispose(bool disposing)
        {
            _vehicleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
