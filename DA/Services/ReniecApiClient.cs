using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Consumo2WebAPIRENIEC.BE.Entities;
using Consumo2WebAPIRENIEC.Utilitario.Http;
using Newtonsoft.Json;

namespace Consumo2WebAPIRENIEC.DA.Services
{
    public class ReniecApiClient
    {
        private const string ConsultaReniecEndpoint = "api/reniec/consulta";
        private readonly HttpClient _httpClient;

        public ReniecApiClient()
            : this(HttpClientProvider.GetReniecClient())
        {
        }

        public ReniecApiClient(HttpClient httpClient)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            _httpClient = httpClient;
        }

        public async Task<ReniecConsultaResponse> ConsultarAsync(ReniecConsultaRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string cuerpo = JsonConvert.SerializeObject(request);

            using (var contenido = new StringContent(cuerpo, Encoding.UTF8, "application/json"))
            using (HttpResponseMessage respuesta = await _httpClient.PostAsync(ConsultaReniecEndpoint, contenido).ConfigureAwait(false))
            {
                string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!respuesta.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format("Error {0}: {1}", (int)respuesta.StatusCode, contenidoRespuesta));
                }

                if (string.IsNullOrWhiteSpace(contenidoRespuesta))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<ReniecConsultaResponse>(contenidoRespuesta);
            }
        }
    }
}
