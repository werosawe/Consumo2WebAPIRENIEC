using System;
using System.Net;
using System.Net.Http;

namespace Consumo2WebAPIRENIEC.Utilitario.Http
{
    public static class HttpClientProvider
    {
        private static readonly Lazy<HttpClient> _reniecClient = new Lazy<HttpClient>(CreateReniecClient, true);

        public static HttpClient GetReniecClient()
        {
            return _reniecClient.Value;
        }

        private static HttpClient CreateReniecClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://pkioc.reniec.gob.pe/afiliacionpp/backend-afiliacion/")
            };

            client.Timeout = TimeSpan.FromSeconds(15);

            return client;
        }
    }
}
