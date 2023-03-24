using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using P1_practica.Models;

namespace P1_practica.Controllers
{
    public class marcaController : Controller
    {
        private readonly equiposContext _equiposContexto;

        public marcaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        //CRUD
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listadoMarcas = (from m in _equiposContexto.marcas
                                            select m).ToList();

            if (listadoMarcas.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoMarcas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarca([FromBody] marcas marca)
        {
            try
            {
                _equiposContexto.marcas.Add(marca);
                _equiposContexto.SaveChanges();
                return Ok(marca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] marcas mModificar)
        {
            marcas? mActual = (from m in _equiposContexto.marcas
                                     where m.id_marcas == id
                                     select m).FirstOrDefault();
            if (mActual == null)
            {
                return NotFound();
            }
            mActual.id_marcas  = mModificar.id_marcas;
            mActual.nombre_marca = mModificar.nombre_marca;
            mActual.estados = mModificar.estados;

            _equiposContexto.Entry(mActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(mActual);
        }
        //Se cambiara el estado a I (inactivo)
        [HttpPut]
        [Route("Desacrivar_marca/{id}")]
        public IActionResult Desactivar_marca(int id)
        {
            marcas? mActual = (from m in _equiposContexto.marcas
                              where m.id_marcas == id
                              select m).FirstOrDefault();
            if (mActual == null)
            {
                return NotFound();
            }
            
            mActual.estados = "I";

            _equiposContexto.Entry(mActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(mActual);
        }
    }
}
