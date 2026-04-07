using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Models;
using MvcOAuthApiEmpleados.Services;
using System.Threading.Tasks;

namespace MvcOAuthApiEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private ServiceEmpleados service;

        public EmpleadosController(ServiceEmpleados service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int id)
        {
            //TENDREMOS EL TOKEN EN SESSION
            string token = HttpContext.Session.GetString("TOKEN");
            if(token == null)
            {
                ViewData["MENSAJE"] = "Debe hacer login";
                return View();
            }
            else
            {
                Empleado empleado = await this.service.FindEmpleadoAsync(id, token);
                return View(empleado);
            }
        }
    }
}
