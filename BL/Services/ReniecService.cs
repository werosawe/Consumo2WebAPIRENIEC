using System;
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
    }
}
