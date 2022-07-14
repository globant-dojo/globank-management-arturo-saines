using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class InsertMovimientoRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo NumeroCuenta es obligatorio")]
        public string NumeroCuenta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo TipoCuenta es obligatorio")]
        public string TipoCuenta { get; set; }

        [Required(ErrorMessage = "El campo Valor es obligatorio")]
        public decimal Valor { get; set; }


        public string TipoMovimiento { get; set; } = string.Empty;

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;
    }
}
