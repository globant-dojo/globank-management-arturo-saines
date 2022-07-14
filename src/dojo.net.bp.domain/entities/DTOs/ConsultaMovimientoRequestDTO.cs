using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class ConsultaMovimientoRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo NumeroCuenta es obligatorio")]
        public string NumeroCuenta { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo TipoCuenta es obligatorio")]
        public string TipoCuenta { get; set; }

        [Required(ErrorMessage = "El campo FechaInicioBusqueda es obligatorio")]
        public DateOnly? FechaInicioBusqueda { get; set; }

        [Required(ErrorMessage = "El campo FechaFinBusqueda es obligatorio")]
        public DateOnly? FechaFinBusqueda { get; set; }

        public string TipoMovimiento { get; set; } = string.Empty;
    }
}
