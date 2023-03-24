using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class usuariosController : Controller
    {
        private readonly equiposContext _equiposContexto;
        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoF = (from u in _equiposContexto.usuarios
                            join c in _equiposContexto.carreras on u.carrera_id equals c.carrera_id
                            select new {
                                u.usuario_id, u.nombre, u.documento, u.tipo,u.carnet, c.nombre_carrera
                            }).ToList();

            if (listadoF.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoF);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuarios([FromBody] usuarios usuario)
        {
            try
            {
                _equiposContexto.usuarios.Add(usuario);
                _equiposContexto.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarUsuarios(int id, [FromBody] usuarios uModificar)
        {
            usuarios? uActual = (from u in _equiposContexto.usuarios
                                   where u.usuario_id == id
                                   select u).FirstOrDefault();
            if (uActual == null)
            {
                return NotFound();
            }
            uActual.usuario_id = uModificar.usuario_id;
            uActual.nombre = uModificar.nombre;
            uActual.documento = uModificar.documento;
            uActual.tipo = uModificar.tipo;
            uActual.carnet = uModificar.carnet;
            uActual.carrera_id = uModificar.carrera_id;

            _equiposContexto.Entry(uActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(uActual);
        }
        //En este caso se borrara el dato (no solo se cambiara el dato)
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult ElimiarUsuario(int id)
        {
            usuarios? usuario = (from u in _equiposContexto.usuarios
                                    where u.usuario_id == id
                                    select u).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            _equiposContexto.usuarios.Attach(usuario);
            _equiposContexto.usuarios.Remove(usuario);
            _equiposContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}
