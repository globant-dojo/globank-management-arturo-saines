using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dojo.net.bp.domain.entities.ContextEntities
{
    public class Cuenta
    {
        [Key]
        public Int64 CuentaId { get; set; }
        public Int64 ClienteId { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
        public decimal SaldoDisponible { get; set; }
        public bool Estado { get; set; }


        public virtual Cliente Cliente { get; set; }
        public virtual List<Movimiento> Movimientos { get; set; }

        public Cuenta()
        { 
            this.Cliente = new Cliente();
            this.Movimientos=new List<Movimiento>();
        }
    }
}
