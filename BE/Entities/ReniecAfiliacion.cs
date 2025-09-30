using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.BE.Entities
{
    public sealed class ReniecAfiliacion
    {
        [JsonProperty("dni")]
        public string Dni { get; set; }

        [JsonProperty("id_unico")]
        public string IdUnico { get; set; }

        [JsonProperty("fecha_afiliacion")]
        public string FechaAfiliacion { get; set; }

        [JsonProperty("codOp")]
        public string CodigoOperacion { get; set; }

        [JsonProperty("ficha_afiliacion")]
        public string FichaAfiliacion { get; set; }
    }
}
