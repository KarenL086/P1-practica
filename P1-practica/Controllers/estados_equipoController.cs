using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class estados_equipoController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_equipo> listadoEe = (from ee in _equiposContexto.estados_equipo
                                          select ee).ToList();

            if (listadoEe.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEe);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarca([FromBody] estados_equipo estadosE)
        {
            try
            {
                _equiposContexto.estados_equipo.Add(estadosE);
                _equiposContexto.SaveChanges();
                return Ok(estadosE);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] estados_equipo eeModificar)
        {
            estados_equipo? eeActual = (from ee in _equiposContexto.estados_equipo
                               where ee.id_estados_equipo == id
                               select ee).FirstOrDefault();
            if (eeActual == null)
            {
                return NotFound();
            }
            eeActual.id_estados_equipo = eeModificar.id_estados_equipo;
            eeActual.descripcion = eeModificar.descripcion;
            eeActual.estado = eeModificar.estado;

            _equiposContexto.Entry(eeActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(eeActual);
        }
        //Se cambiara el estado a I (inactivo)
        [HttpPut]
        [Route("Desacrivar_marca/{id}")]
        public IActionResult Desactivar_marca(int id)
        {
            estados_equipo? eeActual = (from ee in _equiposContexto.estados_equipo
                               where ee.id_estados_equipo == id
                               select ee).FirstOrDefault();
            if (eeActual == null)
            {
                return NotFound();
            }

            eeActual.estado = "I";

            _equiposContexto.Entry(eeActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(eeActual);
        }
    }
}
