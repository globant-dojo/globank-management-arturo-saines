using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class CuentaDTO
    {
        public Int64 CuentaId { get; set; }
        public Int64 ClienteId { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
        public decimal SaldoDisponible { get; set; }
        public bool Estado { get; set; }


        public virtual ClienteDTO Cliente { get; set; }
        //public virtual List<Movimiento> Movimientos { get; set; }
    }
}
