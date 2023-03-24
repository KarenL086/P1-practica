using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoEquipos = (from e in _equiposContexto.equipos
                                  join m in _equiposContexto.marcas on e.marca_id equals m.id_marcas
                                  join ti in _equiposContexto.tipo_equipo on e.tipo_equipo_id equals ti.id_tipo_equipo
                                  join ee in _equiposContexto.estados_equipo on  e.estado_equipo_id equals ee.id_estados_equipo
                                  select new
                                  {
                                    e.id_equipos, e.nombre, e.descripcion, tipo_equipo = ti.descripcion, m.nombre_marca, e.modelo, e.anio_compra, e.costo, e.vida_util, ee.id_estados_equipo, e.estado
                                  }).ToList();

            if(listadoEquipos.Count == 0)   
            {
                return NotFound();
            }

            return Ok(listadoEquipos);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            object? equipo = (from e in _equiposContexto.equipos
                               join m in _equiposContexto.marcas on e.marca_id equals m.id_marcas
                               where e.id_equipos == id
                               select new
                               {
                                   e.id_equipos,
                                   e.nombre,
                                   e.descripcion,
                                   e.tipo_equipo_id,
                                   e.marca_id,
                                   m.nombre_marca
                               }).FirstOrDefault();
            if(equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpGet]
        [Route("Filtro/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoActual = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if(equipoActual == null)
            {
                return NotFound();
            }
            equipoActual.nombre= equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoActual);
        }
        //No se borrara solo se actializara el estado de A a I
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarEquipo(int id)
        {
            equipos? eActual = (from e in _equiposContexto.equipos
                              where e.marca_id == id
                              select e).FirstOrDefault();
            if (eActual == null)
            {
                return NotFound();
            }

            eActual.estado = "I";

            _equiposContexto.Entry(eActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(eActual);

            /* codigo borrar   
             equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();
            if(equipo == null)
            {
                return NotFound();
            }

            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);*/
        }
    }
}
