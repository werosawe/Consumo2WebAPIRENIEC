using System.Collections.Generic;
using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.BE.Entities
{
    public sealed class ReniecCiudadano
    {
        [JsonProperty("documento")]
        public string NumeroDocumento { get; set; }

        [JsonProperty("nombreCompleto")]
        public string NombreCompleto { get; set; }

        [JsonProperty("estadoCivil")]
        public string EstadoCivil { get; set; }

        [JsonProperty("ubigeo")]
        public string UbigeoDireccion { get; set; }

        [JsonProperty("afiliaciones")]
        public List<ReniecAfiliacion> Afiliaciones { get; set; } = new List<ReniecAfiliacion>();

        [JsonProperty("adhesiones")]
        public List<ReniecAdhesion> Adhesiones { get; set; } = new List<ReniecAdhesion>();
    }
}
