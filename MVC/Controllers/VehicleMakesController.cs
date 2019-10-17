using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Service;
using Service.DAL;
using X.PagedList;

namespace MVC.Controllers
{
    public class VehicleMakesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        public VehicleMakesController(IVehicleService vehicleService, IMapper mapper)
        {            
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int? page, string sortOrder, string searchString, string currentFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.AbrvSortParm = sortOrder == "Abrv" ? "abrv_desc" : "Abrv";

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            Expression<Func<VehicleMake, bool>> filter = null;
            if (!string.IsNullOrEmpty(searchString))
            {
                filter = e => e.Name.ToUpper().Contains(searchString.ToUpper()) ||
                                e.Abrv.ToUpper().Contains(searchString.ToUpper());
            }

            Func<IQueryable<VehicleMake>, IOrderedQueryable<VehicleMake>> orderBy = sortOrder switch
            {
                "Name" => q => q.OrderBy(s => s.Name),
                "name_desc" => q => q.OrderByDescending(s => s.Name),
                "Abrv" => q => q.OrderBy(s => s.Abrv),
                "abrv_desc" => q => q.OrderByDescending(s => s.Abrv),
                _ => null,
            };

            int totalItems = await _vehicleService.MakeService.GetCountAsync(filter);

            var vehicleMakes = await _vehicleService.MakeService.GetPageAsync(
                pageNumber, pageSize, filter, orderBy);

            var makes = new List<VehicleMakeViewModel>();
            foreach (var make in vehicleMakes)
            {
                var viewModel = _mapper.Map<VehicleMakeViewModel>(make);
                makes.Add(viewModel);
            }

            var pagedViewModel = new StaticPagedList<VehicleMakeViewModel>(makes, pageNumber, pageSize, totalItems);
            return View(pagedViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {                
                return BadRequest();
            }


            var vehicleMake = await _vehicleService.MakeService.GetByIdAsync(id);

            if (vehicleMake == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Abrv")] VehicleMakeViewModel viewModel)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicleMake = _mapper.Map<VehicleMake>(viewModel);
                    _vehicleService.MakeService.Insert(vehicleMake);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }

            var vehicleMake = await _vehicleService.MakeService.GetByIdAsync(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleMakeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var vehicleMake = _mapper.Map<VehicleMake>(viewModel);
                    _vehicleService.MakeService.Update(vehicleMake);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

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

            var vehicleMake = await _vehicleService.MakeService.GetByIdAsync(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleMakeViewModel>(vehicleMake);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicleMake = await _vehicleService.MakeService.GetByIdAsync(id);
                _vehicleService.MakeService.DeleteAsync(id);
                await _vehicleService.SaveAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _vehicleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
