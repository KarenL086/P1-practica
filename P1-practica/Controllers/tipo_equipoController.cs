using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class tipo_equipoController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoTe = (from te in _equiposContexto.tipo_equipo
                                              select te).ToList();

            if (listadoTe.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoTe);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarca([FromBody] tipo_equipo tipoE)
        {
            try
            {
                _equiposContexto.tipo_equipo.Add(tipoE);
                _equiposContexto.SaveChanges();
                return Ok(tipoE);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] tipo_equipo teModificar)
        {
            tipo_equipo? teActual = (from te in _equiposContexto.tipo_equipo
                                        where te.id_tipo_equipo == id
                                        select te).FirstOrDefault();
            if (teActual == null)
            {
                return NotFound();
            }
            teActual.id_tipo_equipo = teModificar.id_tipo_equipo;
            teActual.descripcion = teModificar.descripcion;
            teActual.estado = teModificar.estado;

            _equiposContexto.Entry(teActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(teActual);
        }
        //Se cambiara el estado a I (inactivo)
        [HttpPut]
        [Route("Desacrivar_marca/{id}")]
        public IActionResult Desactivar_marca(int id)
        {
            tipo_equipo? teActual = (from te in _equiposContexto.tipo_equipo
                                        where te.id_tipo_equipo == id
                                        select te).FirstOrDefault();
            if (teActual == null)
            {
                return NotFound();
            }

            teActual.estado = "I";

            _equiposContexto.Entry(teActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(teActual);
        }
    }
}

