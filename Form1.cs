using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace Consumo2WebAPIRENIEC
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        static Form1()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pkioc.reniec.gob.pe/afiliacionpp/backend-afiliacion/")
            };
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string op = Interaction.InputBox("Ingrese el número de OP que desea consultar:", "Consulta de afiliación", string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(op))
            {
                MessageBox.Show("Debe ingresar un número de OP para realizar la consulta.", "Dato requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button1.Enabled = false;

            try
            {
                var aprobadasTask = FetchPayloadAsync<List<AprobadaDto>>($"afiliados/aprobadas/jne/{op}");
                var adhesionTask = FetchPayloadAsync<List<AdhesionDto>>($"afiliados/adhesion/jne/{op}");

                await Task.WhenAll(aprobadasTask, adhesionTask);

                var aprobadas = aprobadasTask.Result ?? new List<AprobadaDto>();
                var adhesiones = adhesionTask.Result ?? new List<AdhesionDto>();

                if (aprobadas.Count == 0 && adhesiones.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros para el número de OP indicado.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var sb = new StringBuilder();
                sb.AppendLine($"Afiliaciones aprobadas encontradas: {aprobadas.Count}");
                foreach (var item in aprobadas)
                {
                    sb.AppendLine($" - {item.ApellidoPaterno} {item.ApellidoMaterno}, {item.Nombres} ({item.NumeroDocumento})");
                }

                sb.AppendLine();
                sb.AppendLine($"Adhesiones encontradas: {adhesiones.Count}");
                foreach (var item in adhesiones)
                {
                    sb.AppendLine($" - {item.ApellidoPaterno} {item.ApellidoMaterno}, {item.Nombres} ({item.NumeroDocumento})");
                }

                MessageBox.Show(sb.ToString(), "Resultados de la consulta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("La consulta superó el tiempo máximo de espera. Intente nuevamente más tarde.", "Tiempo de espera agotado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Se produjo un problema de red al consultar la API: {ex.Message}", "Error de red", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Respuesta inesperada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private static async Task<TPayload> FetchPayloadAsync<TPayload>(string relativeUrl)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(relativeUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"La API devolvió el estado {(int)response.StatusCode} ({response.ReasonPhrase}).");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        throw new InvalidOperationException("La API devolvió una respuesta vacía.");
                    }

                    // Intentamos primero deserializar envoltorios comunes
                    try
                    {
                        var envelope = JsonSerializer.Deserialize<ApiResponse<TPayload>>(content, _jsonOptions);
                        if (envelope != null)
                        {
                            var payload = envelope.Payload;
                            if (payload != null)
                            {
                                return payload;
                            }

                            if (!string.IsNullOrWhiteSpace(envelope.FriendlyMessage))
                            {
                                throw new InvalidOperationException(envelope.FriendlyMessage);
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // Ignoramos y tratamos de deserializar directamente el contenido
                    }

                    try
                    {
                        var payload = JsonSerializer.Deserialize<TPayload>(content, _jsonOptions);
                        if (payload == null)
                        {
                            throw new InvalidOperationException("La respuesta de la API no contiene datos.");
                        }

                        return payload;
                    }
                    catch (JsonException ex)
                    {
                        throw new InvalidOperationException("No se pudo interpretar la respuesta de la API.", ex);
                    }
                }
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException("La solicitud a la API se agotó.", ex);
            }
        }

        private class ApiResponse<T>
        {
            public bool? Success { get; set; }
            public string Message { get; set; }
            public string Mensaje { get; set; }
            public T Data { get; set; }
            public T Objeto { get; set; }
            public T Resultado { get; set; }
            public T Datos { get; set; }

            [JsonIgnore]
            public T Payload => Data != null ? Data : Objeto != null ? Objeto : Resultado != null ? Resultado : Datos;

            [JsonIgnore]
            public string FriendlyMessage => !string.IsNullOrWhiteSpace(Message) ? Message : Mensaje;
        }

        public class PersonaBaseDto
        {
            public string TipoDocumento { get; set; }
            public string NumeroDocumento { get; set; }
            public string Nombres { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string Estado { get; set; }

            [JsonExtensionData]
            public Dictionary<string, JsonElement> Extras { get; set; }
        }

        public class AprobadaDto : PersonaBaseDto
        {
        }

        public class AdhesionDto : PersonaBaseDto
        {
        }
    }
}
