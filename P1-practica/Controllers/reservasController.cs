using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class reservasController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoR = (from r in _equiposContexto.reservas
                            join e in _equiposContexto.equipos on r.equipo_id equals e.id_equipos
                            join u in _equiposContexto.usuarios on r.usuario_id equals u.usuario_id
                            select new
                            {
                                r.reserva_id, equipo = e.nombre, usuario = u.nombre, r.fecha_salida, r.hora_salida, r.tiempo_reserva, r.fecha_retorno, r.hora_retorno
                            }).ToList();

            if (listadoR.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoR);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarReserva([FromBody] reservas reserva)
        {
            try
            {
                _equiposContexto.reservas.Add(reserva);
                _equiposContexto.SaveChanges();
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] reservas rModificar)
        {
            reservas? rActual = (from r in _equiposContexto.reservas
                                   where r.reserva_id == id
                                   select r).FirstOrDefault();
            if (rActual == null)
            {
                return NotFound();
            }
            rActual.reserva_id = rModificar.reserva_id;
            rActual.equipo_id = rModificar.equipo_id;
            rActual.usuario_id = rModificar.usuario_id;
            rActual.fecha_salida = rModificar.fecha_salida;
            rActual.hora_salida = rModificar.hora_salida;
            rActual.tiempo_reserva = rModificar.tiempo_reserva;
            rActual.fecha_retorno = rModificar.fecha_retorno;
            rActual.hora_retorno = rModificar.hora_retorno;

            _equiposContexto.Entry(rActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(rActual);
        }
        //En este caso se borrara el dato (no solo se cambiara el dato)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarReserva(int id)
        {
            reservas? reserva = (from r in _equiposContexto.reservas
                                    where r.reserva_id == id
                                    select r).FirstOrDefault();
            if (reserva == null)
            {
                return NotFound();
            }

            _equiposContexto.reservas.Attach(reserva);
            _equiposContexto.reservas.Remove(reserva);
            _equiposContexto.SaveChanges();

            return Ok(reserva);
        }
    }
}
