using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using Domain;
using Infrastructure;
using Application;
using Microsoft.AspNetCore.Http;

namespace Presentation.WebApp.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly AlumnosDbContext _alumnosDbContext;

        public AlumnosController(IConfiguration configuration)
        {
            _alumnosDbContext = new AlumnosDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        // Acción para listar los alumnos
        public IActionResult Index()
        {
            var data = _alumnosDbContext.List();
            return View(data);
        }

        // Acción para mostrar los detalles de un alumno
        public IActionResult Details(Guid id)
        {
            var data = _alumnosDbContext.Details(id);
            return PartialView(data);
        }

        // Acción para crear un nuevo alumno (vista de creación)
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un nuevo alumno (proceso POST)
        [HttpPost]
        public IActionResult Create(Alumno data, IFormFile file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _alumnosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        // Acción para editar un alumno (vista de edición)
        public IActionResult Edit(Guid id)
        {
            var data = _alumnosDbContext.Details(id);
            return View(data);
        }

        // Acción para editar un alumno (proceso POST)
        [HttpPost]
        public IActionResult Edit(Alumno data, IFormFile file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _alumnosDbContext.Edit(data);
            return RedirectToAction("Index");
        }

        // Acción para eliminar un alumno
        public IActionResult Delete(Guid id)
        {
            _alumnosDbContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
