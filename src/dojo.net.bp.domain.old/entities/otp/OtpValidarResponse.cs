using System.Text.Json.Serialization;

namespace dojo.net.bp.domain.entities.otp
{
    public class OtpValidarResponse
    {
        [JsonIgnore] public string CodigoRetorno { get; set; }
        [JsonIgnore] public string Mensaje { get; set; }
        public string jwtCliente { get; set; }
    }
}
