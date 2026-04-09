using Microsoft.AspNetCore.Mvc;
using MvcOAuthApiEmpleados.Filters;
using MvcOAuthApiEmpleados.Models;
using MvcOAuthApiEmpleados.Services;
using System.Security.Claims;
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

        [AuthorizeEmpleados]
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.service.GetEmpleadosAsync();
            return View(empleados);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Details(int id)
        {
            Empleado empleado = await this.service.FindEmpleadoAsync(id);
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Perfil()
        {
            Empleado empleado = await this.service.GetPerfilAsync();
            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<ActionResult> Compis()
        {
            List<Empleado> compis = await this.service.GetCompisAsync();
            return View(compis);
        }

        public async Task<ActionResult> EmpleadosOficios()
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EmpleadosOficios(int? incremento, List<string>oficio, string accion)
        {
            List<string> oficios = await this.service.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;

            if(accion.ToLower() == "update")
            {
                await this.service.UpdateEmpleadoAsync(incremento.Value, oficio);
            }

            List<Empleado> empleados = await this.service.GetEmpleadosOficioAsync(oficio);
            return View(empleados);
        }

    }
}
