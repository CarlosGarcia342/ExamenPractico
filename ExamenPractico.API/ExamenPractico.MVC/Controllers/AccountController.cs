using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ExamenPractico.MVC.Models;

namespace ExamenPractico.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Llamada al endpoint de login en la API
            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                // Aquí se podría extraer un token y almacenar datos en la sesión o cookie.
                // Por simplicidad, redirigimos al Home
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Credenciales inválidas.");
                return View(loginDto);
            }
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var json = JsonSerializer.Serialize(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Llamada al endpoint de registro en la API
            var response = await _httpClient.PostAsync("api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                // Registro exitoso; redirigimos al login
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "No se pudo registrar el usuario.");
                return View(registerDto);
            }
        }
    }
}
