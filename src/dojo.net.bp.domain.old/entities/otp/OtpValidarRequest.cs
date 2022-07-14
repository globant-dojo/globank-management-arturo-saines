using System.Text.Json.Serialization;

namespace dojo.net.bp.domain.entities.otp
{
    public class OtpValidarRequest
    {
        [JsonIgnore]

        public string? Aplicacion { get; set; }

        [JsonIgnore]
        public string? Servicio { get; set; }
        public string? Identificacion { get; set; }

        [JsonIgnore]
        public string? Llave { get; set; }

        [JsonIgnore]
        public string? CodigoRetorno { get; set; }

        [JsonIgnore]
        public string? Mensaje { get; set; }

        [JsonIgnore]
        public string? Canal { get; set; }

        [JsonIgnore]
        public string? Opid { get; set; }

        [JsonIgnore]
        public string? Terminal { get; set; }

        public string Otp { get; set; }

    }
}
