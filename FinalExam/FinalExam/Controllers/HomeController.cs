using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinalExam.Models;
using FinalExam.DAL;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Chef> chefs = await _context.Chefs.ToListAsync();
        return View(chefs);
    }
}
