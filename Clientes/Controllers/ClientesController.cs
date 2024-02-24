using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clientes.Models;

namespace Clientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesContext _context;

        public ClientesController(ClientesContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                lista = _context.Clientes.ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Response = lista });
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int idCliente)
        {
            Cliente oCliente = _context.Clientes.Find(idCliente);

            if (oCliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            try
            {
                oCliente = _context.Clientes.Where(c => c.ID == idCliente).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oCliente });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, Response = oCliente });
            }
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string nombre)
        {
            try
            {
                // Realizar la búsqueda por nombre que contenga el valor proporcionado
                var clientesEncontrados = _context.Clientes
                    .Where(c => c.Nombres.Contains(nombre))
                    .ToList();

                if (clientesEncontrados.Count == 0)
                {
                    return BadRequest("Clientes no encontrados");
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = clientesEncontrados });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert([FromBody] Cliente objeto)
        {
            try
            {
                // Validar el modelo antes de intentar guardarlo
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { mensaje = "La solicitud contiene datos no válidos", errores = ModelState });
                }

                _context.Clientes.Add(objeto);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Cliente guardado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] Cliente objeto)
        {
            Cliente oCliente = _context.Clientes.Find(objeto.ID);

            if (oCliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }
            try
            {
                oCliente.Nombres = objeto.Nombres is null ? oCliente.Nombres : objeto.Nombres;
                oCliente.Apellidos = objeto.Apellidos is null ? oCliente.Apellidos : objeto.Apellidos;
                oCliente.CUIT = objeto.CUIT is null ? oCliente.CUIT : objeto.CUIT;
                oCliente.TelefonoCelular = objeto.TelefonoCelular is null ? oCliente.TelefonoCelular : objeto.TelefonoCelular;
                oCliente.Email = objeto.Email is null ? oCliente.Email : objeto.Email;
                oCliente.Domicilio = objeto.Domicilio is null ? oCliente.Domicilio : objeto.Domicilio;
                oCliente.FechaNacimiento = objeto.FechaNacimiento is null ? oCliente.FechaNacimiento : objeto.FechaNacimiento;


                _context.Clientes.Update(oCliente);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Cliente modificado correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
