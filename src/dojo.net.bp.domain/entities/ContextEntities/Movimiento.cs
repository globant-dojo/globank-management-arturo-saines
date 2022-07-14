using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dojo.net.bp.domain.entities.ContextEntities
{
    public class Movimiento
    {
        [Key]
        public Int64 MovimientoId { get; set; }
        public Int64 CuentaId { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public DateTime FechaMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }


        public virtual Cuenta Cuenta { get; set; }
    }
}
