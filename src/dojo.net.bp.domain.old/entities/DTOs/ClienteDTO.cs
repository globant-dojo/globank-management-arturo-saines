using System;
using System.Collections.Generic;
using System.Text;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class ClienteDTO
    {
        //public Int64 PersonaId { get; set; }
        public Int64? ClienteId { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombresCompletos { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int Edad
        {
            get
            {
                int age = 0;
                age = DateTime.Now.Subtract(FechaNacimiento).Days;
                age = age / 365;

                if (age == DateTime.Now.Year) age = 0;

                return age;
            }
        }
        public string DireccionDomicilio { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;
    }
}
