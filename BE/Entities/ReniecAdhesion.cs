using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.BE.Entities
{
    public sealed class ReniecAdhesion
    {
        [JsonProperty("dni")]
        public string Dni { get; set; }

        [JsonProperty("id_unico")]
        public string IdUnico { get; set; }

        [JsonProperty("fecha_adhesion")]
        public string FechaAdhesion { get; set; }

        [JsonProperty("codOp")]
        public string CodigoOperacion { get; set; }

        [JsonProperty("ficha_adhesion")]
        public string FichaAdhesion { get; set; }
    }
}
