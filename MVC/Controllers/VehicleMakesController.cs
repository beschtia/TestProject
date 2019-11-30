using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.ViewModels;
using Service.DAL;
using Service.EFModels;
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

        public async Task<IActionResult> Index(FilteringModel filtering, SortingModel sorting, PagingModel paging)
        {
            ViewBag.Filter = filtering;
            ViewBag.Sort = sorting;
            ViewBag.Paging = paging;
            ViewBag.PageSize = new SelectList(new List<int> { 2, 5, 10, 25, 50, 100 }, paging.PageSize);

            var makes = await _vehicleService.GetPagedMakesAsync(filtering, sorting, paging);
            var viewModel = _mapper.Map<IPagedList<IVehicleMake>, IPagedList<VehicleMakeViewModel>>(makes);
            return View(viewModel);            
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
                    var vehicleMakeDTO = _mapper.Map<IVehicleMake>(viewModel);
                    await _vehicleService.InsertMakeAsync(vehicleMakeDTO);
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("", "Unable to save. Try again, and if the problem persists see your system administrator.");
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
                    var vehicleMakeDTO = _mapper.Map<IVehicleMake>(viewModel);
                    await _vehicleService.UpdateMakeAsync(vehicleMakeDTO);
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
                if (vehicleMake == null)
                {
                    return NotFound();
                }
                await _vehicleService.MakeRepository.DeleteAsync(vehicleMake);
            }         
            catch (DbUpdateException)
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
