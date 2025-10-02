using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consumo2WebAPIRENIEC.BE.Entities;
using Consumo2WebAPIRENIEC.DA.Services;

namespace Consumo2WebAPIRENIEC.BL.Services
{
    public class ReniecService
    {
        private readonly ReniecApiClient _reniecApiClient;

        public ReniecService()
            : this(new ReniecApiClient())
        {
        }

        public ReniecService(ReniecApiClient reniecApiClient)
        {
            if (reniecApiClient == null)
            {
                throw new ArgumentNullException("reniecApiClient");
            }

            _reniecApiClient = reniecApiClient;
        }

        public Task<ReniecConsultaResponse> ConsultarCiudadanoAsync(ReniecConsultaRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return _reniecApiClient.ConsultarAsync(request);
        }

        public Task<IReadOnlyList<ReniecAfiliacion>> ObtenerAfiliacionesAprobadasAsync(string parametro, string token)
        {
            if (string.IsNullOrWhiteSpace(parametro))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "parametro");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "token");
            }

            return _reniecApiClient.ObtenerAfiliacionesAprobadasAsync(parametro, token);
        }

        public Task<IReadOnlyList<ReniecAdhesion>> ObtenerAdhesionesAsync(string parametro, string token)
        {
            if (string.IsNullOrWhiteSpace(parametro))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "parametro");
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", "token");
            }

            return _reniecApiClient.ObtenerAdhesionesAsync(parametro, token);
        }
    }
}
