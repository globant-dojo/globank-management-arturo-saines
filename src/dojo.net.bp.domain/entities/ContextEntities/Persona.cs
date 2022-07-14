using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dojo.net.bp.domain.entities.ContextEntities
{
    public class Persona
    {
        //[Key]
        public Int64 PersonaId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombresCompletos { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        //public int Edad { get; set; }
        public string DireccionDomicilio { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
