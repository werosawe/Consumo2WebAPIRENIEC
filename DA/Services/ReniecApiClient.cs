using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private const string AfiliacionesAprobadasEndpoint = "afiliados/aprobadas/jne/";
        private const string AdhesionesEndpoint = "adhesion/jne/";
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

        public Task<IReadOnlyList<ReniecAfiliacion>> ObtenerAfiliacionesAprobadasAsync(string parametro, string token)
        {
            if (string.IsNullOrWhiteSpace(parametro))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "parametro");
            }

            return SendAuthorizedGetAsync<ReniecAfiliacion>(AfiliacionesAprobadasEndpoint + parametro.Trim(), token);
        }

        public Task<IReadOnlyList<ReniecAdhesion>> ObtenerAdhesionesAsync(string parametro, string token)
        {
            if (string.IsNullOrWhiteSpace(parametro))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "parametro");
            }

            return SendAuthorizedGetAsync<ReniecAdhesion>(AdhesionesEndpoint + parametro.Trim(), token);
        }

        private async Task<IReadOnlyList<T>> SendAuthorizedGetAsync<T>(string endpoint, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "token");
            }

            using (var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Trim());

                using (HttpResponseMessage respuesta = await _httpClient.SendAsync(solicitud).ConfigureAwait(false))
                {
                    string contenidoRespuesta = await respuesta.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!respuesta.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException(string.Format("Error {0}: {1}", (int)respuesta.StatusCode, contenidoRespuesta));
                    }

                    if (string.IsNullOrWhiteSpace(contenidoRespuesta))
                    {
                        return Array.Empty<T>();
                    }

                    var resultado = JsonConvert.DeserializeObject<List<T>>(contenidoRespuesta);
                    return (IReadOnlyList<T>)(resultado ?? new List<T>());
                }
            }
        }
    }
}
