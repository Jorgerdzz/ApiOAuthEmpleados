using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Models;
using MvcOAuthApiEmpleados.Services;
using System.Threading.Tasks;

namespace MvcOAuthApiEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceEmpleados service;

        public ManagedController(ServiceEmpleados service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string token = await this.service.LoginAsync(model.Username, model.Password);
            if(token == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
            }
            else
            {
                ViewData["MENSAJE"] = "Ya tienes tu Token!!!";
                HttpContext.Session.SetString("TOKEN", token);
            }
            return View();
        }
    }
}
