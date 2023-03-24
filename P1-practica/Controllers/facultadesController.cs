using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class facultadesController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listadoF = (from f in _equiposContexto.facultades
                                              select f).ToList();

            if (listadoF.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoF);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarFacultad([FromBody] facultades facultad)
        {
            try
            {
                _equiposContexto.facultades.Add(facultad);
                _equiposContexto.SaveChanges();
                return Ok(facultad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarFacultad(int id, [FromBody] facultades fModificar)
        {
            facultades? fActual = (from f in _equiposContexto.facultades
                                        where f.facultad_id == id
                                        select f).FirstOrDefault();
            if (fActual == null)
            {
                return NotFound();
            }
            fActual.facultad_id = fModificar.facultad_id;
            fActual.nombre_facultad = fModificar.nombre_facultad;

            _equiposContexto.Entry(fActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(fActual);
        }
        //En este caso se borrara el dato (no solo se cambiara el dato)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarFacultad(int id)
        { 
             facultades? facultad = (from f in _equiposContexto.facultades
                               where f.facultad_id == id
                               select f).FirstOrDefault();
            if(facultad == null)
            {
                return NotFound();
            }

            _equiposContexto.facultades.Attach(facultad);
            _equiposContexto.facultades.Remove(facultad);
            _equiposContexto.SaveChanges();

            return Ok(facultad);
        }
    }
}
 
