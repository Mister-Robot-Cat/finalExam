using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalExam.DAL;
using FinalExam.Models;
using FinalExam.Areas.Admin.ViewModels;
using FinalExam.Utilities.Extensions;
using FinalExam.Utilities.Enums;

namespace FinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ChefController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        public async Task<IActionResult> Index()

        {
            return View(await _context.Chefs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chef == null)
            {
                return NotFound();
            }

            return View(chef);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Photo.ValidateSize(10, FileSize.MB))
            {
                ModelState.AddModelError("Photo", "Image is too heavy");
                return View(vm);
            }
            if (!vm.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is not relative");
                return View(vm);
            }

            Chef chef = new Chef()
            {
                FullName = vm.FullName,
                Designation = vm.Designation,
                ImageUrl = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images")
            };

            await _context.AddAsync(chef);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }
            Chef chef = await _context.Chefs.FirstOrDefaultAsync(che => che.Id == id);
            if (chef == null)
            {
                return NotFound();
            }
            UpdateVM vm = new UpdateVM
            {
                FullName = chef.FullName,
                Designation = chef.Designation,
                ImageUrl = chef.ImageUrl
            };
            return View(chef);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVM vm)
        {
            if (id == null || id < 1)
            {
                return BadRequest();
            }
            Chef chef = await _context.Chefs.FirstOrDefaultAsync(che => che.Id == id);
            if (vm.Photo != null)
            {
                if (vm.Photo.ValidateSize(10, FileSize.MB))
                {
                    ModelState.AddModelError("Photo", "Image is too heavy");
                    return View(vm);
                }
                if (!vm.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "Image type is not relative");
                    return View(vm);
                }

                chef.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "images");
                chef.ImageUrl = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
            }
            chef.FullName = vm.FullName;
            chef.Designation = vm.Designation;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chef == null)
            {
                return NotFound();
            }

            return View(chef);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chef = await _context.Chefs.FindAsync(id);
            if (chef != null)
            {
                _context.Chefs.Remove(chef);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChefExists(int id)
        {
            return _context.Chefs.Any(e => e.Id == id);
        }
    }
}
