using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.BE.Entities
{
    public sealed class ReniecConsultaResponse
    {
        [JsonProperty("success")]
        public bool Exito { get; set; }

        [JsonProperty("message")]
        public string Mensaje { get; set; }

        [JsonProperty("data")]
        public ReniecCiudadano Resultado { get; set; }
    }
}
