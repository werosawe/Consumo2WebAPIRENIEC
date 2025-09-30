using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Consumo2WebAPIRENIEC.BE.Entities;
using Consumo2WebAPIRENIEC.BL.Formatting;
using Consumo2WebAPIRENIEC.BL.Services;

namespace Consumo2WebAPIRENIEC
{
    public partial class Form1 : Form
    {
        private readonly ReniecService _reniecService;

        public Form1()
        {
            InitializeComponent();
            _reniecService = new ReniecService();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            try
            {
                var request = new ReniecConsultaRequest
                {
                    Canal = "ESCRITORIO",
                    NumeroDocumento = "00000000",
                    TipoDocumento = "DNI"
                };

                ReniecConsultaResponse response = await _reniecService.ConsultarCiudadanoAsync(request).ConfigureAwait(true);

                if (response == null)
                {
                    MessageBox.Show("La respuesta de la API no contiene datos.", "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!response.Exito)
                {
                    MessageBox.Show(string.IsNullOrEmpty(response.Mensaje) ? "La API devolvió un error." : response.Mensaje, "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (response.Resultado == null)
                {
                    MessageBox.Show("No se encontraron datos del ciudadano.", "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                MessageBox.Show(ReniecFormatter.FormatearCiudadano(response.Resultado), "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Se produjo un error de comunicación con RENIEC: " + httpEx.Message, "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("La consulta excedió el tiempo máximo de espera configurado.", "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error inesperado: " + ex.Message, "RENIEC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }
    }
}
