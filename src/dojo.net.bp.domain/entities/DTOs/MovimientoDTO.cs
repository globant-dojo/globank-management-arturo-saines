using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class MovimientoDTO
    {
        public Int64 MovimientoId { get; set; }
        public Int64 CuentaId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }


        public virtual CuentaDTO Cuenta { get; set; }
    }
}
