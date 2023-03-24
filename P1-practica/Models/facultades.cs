using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace P1_practica.Models
{
    public class facultades
    {
        [Key]
        public int facultad_id { get; set; }
        public string? nombre_facultad { get; set; }
    }
}
