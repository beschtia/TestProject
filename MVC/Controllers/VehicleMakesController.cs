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
    public class VehicleMakesController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        public VehicleMakesController(IVehicleService vehicleService, IMapper mapper)
        {            
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(MakesViewModel model)
        {
            ViewData["PageSize"] = new SelectList(new List<int> { 2, 5, 10, 25, 50, 100 });
            var makes = await _vehicleService.GetPagedMakes(model.Filtering, model.Sorting, model.Paging);
            model.Makes = _mapper.Map<IPagedList<VehicleMake>, IPagedList<VehicleMakeViewModel>>(makes);
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {                
                return BadRequest();
            }


            var vehicleMake = await _vehicleService.MakeRepository.GetByIdAsync(id);

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
                    _vehicleService.MakeRepository.Insert(vehicleMake);
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

            var vehicleMake = await _vehicleService.MakeRepository.GetByIdAsync(id);
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
                    _vehicleService.MakeRepository.Update(vehicleMake);
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

            var vehicleMake = await _vehicleService.MakeRepository.GetByIdAsync(id);
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
                var vehicleMake = await _vehicleService.MakeRepository.GetByIdAsync(id);
                _vehicleService.MakeRepository.DeleteAsync(id);
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
