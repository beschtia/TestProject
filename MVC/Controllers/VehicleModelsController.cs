using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IActionResult> Index(int? page, int? selectedMakeId, string sortOrder, string searchString, int? currentFilter)
        { 
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.AbrvSortParm = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";
            ViewBag.MakeSortParm = sortOrder == "Make" ? "make_desc" : "Make";

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (selectedMakeId != null)
            {
                page = 1;
            }
            else
            {
                selectedMakeId = currentFilter;
            }
            ViewBag.CurrentFilter = selectedMakeId;

            PopulateVehicleMakesDropDownList(selectedMakeId);            

            Expression<Func<VehicleModel, bool>> filter = null;            
            if (selectedMakeId != null)
            {
                filter = e => e.MakeId == selectedMakeId;
            }

            Func<IQueryable<VehicleModel>, IOrderedQueryable<VehicleModel>> orderBy = sortOrder switch
            {
                "Name" => q => q.OrderBy(e => e.Name),
                "name_desc" => q => q.OrderByDescending(e => e.Name),
                "Abrv" => q => q.OrderBy(e => e.Abrv),
                "abrv_desc" => q => q.OrderByDescending(e => e.Abrv),
                "Make" => q => q.OrderBy(e => e.Make.Name),
                "make_desc" => q => q.OrderByDescending(e => e.Make.Name),
                _ => null,
            };

            int totalItems = await _vehicleService.ModelService.GetCountAsync(filter);

            var vehicleModels = await _vehicleService.ModelService.GetPageAsync(
                pageNumber, pageSize, filter, orderBy, "Make");

            var models = new List<VehicleModelViewModel>();
            foreach (var vehicle in vehicleModels)
            {
                var viewModel = _mapper.Map<VehicleModelViewModel>(vehicle);
                models.Add(viewModel);
            }
            
            var pagedViewModel = new StaticPagedList<VehicleModelViewModel>(models, pageNumber, pageSize, totalItems);
            return View(pagedViewModel);            
        }
                
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModel = await _vehicleService.ModelService.GetOneAsync(v => v.Id == id, "Make");
            
            if (vehicleModel == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
            return View(viewModel);
        }

        
        public IActionResult Create()
        {
            PopulateVehicleMakesDropDownList();
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
                    _vehicleService.ModelService.Insert(vehicleModel);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulateVehicleMakesDropDownList(viewModel.MakeId);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModel = await _vehicleService.ModelService.GetByIdAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModel);
            PopulateVehicleMakesDropDownList(viewModel.MakeId);
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
                    _vehicleService.ModelService.Update(vehicleModel);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateVehicleMakesDropDownList(viewModel.MakeId);
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
            var vehicleModel = await _vehicleService.ModelService.GetOneAsync(v => v.Id == id, "Make");
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
                var vehicleModel = await _vehicleService.ModelService.GetByIdAsync(id);
                _vehicleService.ModelService.DeleteAsync(id);
                await _vehicleService.SaveAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        
        private void PopulateVehicleMakesDropDownList(object selectedVehicleMake = null)
        {
            var makes = _vehicleService.MakeService.GetAllAsync(q => q.OrderBy(e => e.Name)).Result;
            var items = new List<VehicleMakeDropListModel>();
            foreach (var make in makes)
            {
                var model = _mapper.Map<VehicleMakeDropListModel>(make);
                items.Add(model);
            }
            ViewData["MakeId"] = new SelectList(items, "Id", "Name", selectedVehicleMake);
        }

        protected override void Dispose(bool disposing)
        {
            _vehicleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
