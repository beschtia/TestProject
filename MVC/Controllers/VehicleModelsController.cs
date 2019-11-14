using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleModelsController(IVehicleService vehicleService, IMapper mapper)
        {
            this._vehicleService = vehicleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(FilteringModel filtering, SortingModel sorting, PagingModel paging)
        {            
            ViewBag.Filter = filtering;
            ViewBag.Sort = sorting;
            ViewBag.Paging = paging;
            ViewBag.PageSize = new SelectList(new List<int> { 2, 5, 10, 25, 50, 100 }, paging.PageSize);

            await PopulateMakesSelectList(filtering.FilterById);
            var models = await _vehicleService.GetPagedModels(filtering, sorting, paging);
            if (models.TotalItemCount == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            var viewModel = _mapper.Map<IPagedList<IVehicleModel>, IPagedList<VehicleModelViewModel>>(models);
            return View(viewModel);            
        }
                
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModelDTO = await _vehicleService.GetModelByIdAsync(id.GetValueOrDefault());
            if (vehicleModelDTO == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModelDTO);
            return View(viewModel);
        }
        
        public async Task<IActionResult> Create()
        {
            await PopulateMakesSelectList();
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
                    var vehicleModelDTO = _mapper.Map<IVehicleModel>(viewModel);
                    _vehicleService.InsertModel(vehicleModelDTO);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("", "Unable to save. Try again, and if the problem persists see your system administrator.");
            }    
            await PopulateMakesSelectList(viewModel.MakeId);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var vehicleModelDTO = await _vehicleService.GetModelByIdAsync(id.GetValueOrDefault());
            if (vehicleModelDTO == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModelDTO);
            await PopulateMakesSelectList(viewModel.MakeId);
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
                    var vehicleModelDTO = _mapper.Map<IVehicleModel>(viewModel);
                    _vehicleService.UpdateModel(vehicleModelDTO);
                    await _vehicleService.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            await PopulateMakesSelectList(viewModel.MakeId);
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
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var vehicleModelDTO = await _vehicleService.GetModelByIdAsync(id.GetValueOrDefault());
            if (vehicleModelDTO == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<VehicleModelViewModel>(vehicleModelDTO);
            return View(viewModel);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var vehicleModelDTO = await _vehicleService.GetModelByIdAsync(id);
                if (vehicleModelDTO == null)
                {
                    return NotFound();
                }
                await _vehicleService.ModelRepository.DeleteAsync(id);
                await _vehicleService.SaveAsync();
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        private async Task PopulateMakesSelectList(object selectedVehicleMake = null)
        {
            var makesDTO = await _vehicleService.GetMakes();
            var items = _mapper.Map<IEnumerable<VehicleMakeDropListModel>>(makesDTO);
            ViewBag.MakesSelectList = new SelectList(items.OrderBy(i => i.Name), "Id", "Name", selectedVehicleMake);
        }

        protected override void Dispose(bool disposing)
        {
            _vehicleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
