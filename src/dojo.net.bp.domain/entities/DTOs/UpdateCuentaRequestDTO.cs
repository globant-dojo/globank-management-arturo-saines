using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dojo.net.bp.domain.entities.DTOs
{
    public class UpdateCuentaRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo Cedula es obligatorio")]
        public string Cedula { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo NumeroCuenta es obligatorio")]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo TipoCuenta es obligatorio")]
        public string TipoCuenta { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; } = 0;
        public decimal SaldoDisponible { get; set; } = 0;
        public bool Estado { get; set; } = true;
    }
}
