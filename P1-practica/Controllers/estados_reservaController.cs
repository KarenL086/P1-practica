using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class estados_reservaController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public estados_reservaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_reserva> listado = (from er in _equiposContexto.estados_reserva
                                              select er).ToList();

            if (listado.Count == 0)
            {
                return NotFound();
            }

            return Ok(listado);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstado_Reserva([FromBody] estados_reserva er)
        {
            try
            {
                _equiposContexto.estados_reserva.Add(er);
                _equiposContexto.SaveChanges();
                return Ok(er);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEstado_Reserva(int id, [FromBody] estados_reserva Modificar)
        {
            estados_reserva? Actual = (from er in _equiposContexto.estados_reserva
                                   where er.estado_res_id == id
                                   select er).FirstOrDefault();
            if (Actual == null)
            {
                return NotFound();
            }
            Actual.estado_res_id = Modificar.estado_res_id;
            Actual.estado = Modificar.estado;

            _equiposContexto.Entry(Actual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(Actual);
        }
        //En este caso se borrara el dato (no solo se cambiara el dato)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarEstado_reserva(int id)
        {
            estados_reserva? estados_reserva = (from er in _equiposContexto.estados_reserva
                                    where er.estado_res_id == id
                                    select er).FirstOrDefault();
            if (estados_reserva == null)
            {
                return NotFound();
            }

            _equiposContexto.estados_reserva.Attach(estados_reserva);
            _equiposContexto.estados_reserva.Remove(estados_reserva);
            _equiposContexto.SaveChanges();

            return Ok(estados_reserva);
        }
    }
}
