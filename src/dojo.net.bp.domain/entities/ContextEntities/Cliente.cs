using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dojo.net.bp.domain.entities.ContextEntities
{
    public class Cliente : Persona
    {
        public Int64 ClienteId { get; set; }
        public string Contrasena { get; set; } = string.Empty;
        public bool Estado { get; set; }

        public virtual List<Cuenta> Cuentas { get; set; }

        public Cliente()
        { 
            this.Cuentas = new List<Cuenta>();
        }
    }
}
