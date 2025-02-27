using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExamenPractico.MVC.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ExamenPractico.MVC.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmpleadoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // Acción para listar empleados con búsqueda y paginación
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync("api/empleados");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Empleado>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var empleados = JsonSerializer.Deserialize<List<Empleado>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Filtrado por búsqueda (Nombre, Apellido o Email)
            if (!string.IsNullOrEmpty(search))
            {
                empleados = empleados.Where(e => e.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase)
                                                || e.Apellido.Contains(search, StringComparison.OrdinalIgnoreCase)
                                                || e.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int totalItems = empleados.Count;
            var pagedEmpleados = empleados.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.Search = search;

            return View(pagedEmpleados);
        }

        // GET: /Empleado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return View(empleado);

            var json = JsonSerializer.Serialize(empleado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/empleados", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Error al crear el empleado");
            return View(empleado);
        }

        // GET: /Empleado/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/empleados/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var json = await response.Content.ReadAsStringAsync();
            var empleado = JsonSerializer.Deserialize<Empleado>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (empleado == null)
                return NotFound();
            return View(empleado);
        }

        // POST: /Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
                return View(empleado);

            var json = JsonSerializer.Serialize(empleado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/empleados/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Error al actualizar el empleado");
            return View(empleado);
        }

        // GET: /Empleado/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"api/empleados/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var json = await response.Content.ReadAsStringAsync();
            var empleado = JsonSerializer.Deserialize<Empleado>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (empleado == null)
                return NotFound();
            return View(empleado);
        }

        // POST: /Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/empleados/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Error al eliminar el empleado");
            return RedirectToAction("Delete", new { id });
        }
    }
}
