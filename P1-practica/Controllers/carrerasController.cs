using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class carrerasController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoC = (from c in _equiposContexto.carreras
                                       join f in _equiposContexto.facultades on c.facultad_id equals f.facultad_id
                                       select new
                                       {
                                           c.carrera_id, c.nombre_carrera, c.facultad_id, facultad = f.nombre_facultad
                                       }).ToList();

            if (listadoC.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoC);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AgregarCarrera([FromBody] carreras carrera)
        {
            try
            {
                _equiposContexto.carreras.Add(carrera);
                _equiposContexto.SaveChanges();
                return Ok(carrera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarCarrera(int id, [FromBody] carreras cModificar)
        {
            carreras? cActual = (from c in _equiposContexto.carreras
                                   where c.facultad_id == id
                                   select c).FirstOrDefault();
            if (cActual == null)
            {
                return NotFound();
            }
            cActual.carrera_id = cModificar.carrera_id;
            cActual.nombre_carrera= cModificar.nombre_carrera;
            cActual.facultad_id = cModificar.facultad_id;

            _equiposContexto.Entry(cActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(cActual);
        }
        //En este caso se borrara el dato (no solo se cambiara el estado)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarCarrera(int id)
        {
            carreras? carrera = (from c in _equiposContexto.carreras
                                    where c.carrera_id == id
                                    select c).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }

            _equiposContexto.carreras.Attach(carrera);
            _equiposContexto.carreras.Remove(carrera);
            _equiposContexto.SaveChanges();

            return Ok(carrera);
        }
    }
}
