using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.BE.Entities
{
    public sealed class ReniecConsultaRequest
    {
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }

        [JsonProperty("numeroDocumento")]
        public string NumeroDocumento { get; set; }

        [JsonProperty("canal")]
        public string Canal { get; set; }
    }
}
