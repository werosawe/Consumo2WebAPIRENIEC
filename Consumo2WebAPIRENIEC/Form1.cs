using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Consumo2WebAPIRENIEC.BE.Entities;
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
            const string parametro = "IT19";
            const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.stg";

            button1.Enabled = false;

            try
            {
                IReadOnlyList<ReniecAfiliacion> afiliaciones = await _reniecService.ObtenerAfiliacionesAprobadasAsync(parametro, token);
                IReadOnlyList<ReniecAdhesion> adhesiones = await _reniecService.ObtenerAdhesionesAsync(parametro, token);

                var mensaje = new StringBuilder();
                AgregarDetalleAfiliaciones(mensaje, afiliaciones);
                mensaje.AppendLine();
                AgregarDetalleAdhesiones(mensaje, adhesiones);

                MessageBox.Show(mensaje.ToString(), "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al consultar los servicios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private static void AgregarDetalleAfiliaciones(StringBuilder mensaje, IReadOnlyList<ReniecAfiliacion> afiliaciones)
        {
            mensaje.AppendLine("Afiliaciones aprobadas JNE:");

            if (afiliaciones == null || afiliaciones.Count == 0)
            {
                mensaje.AppendLine("  (sin registros)");
                return;
            }

            for (int i = 0; i < afiliaciones.Count; i++)
            {
                ReniecAfiliacion afiliacion = afiliaciones[i];
                mensaje.AppendLine(string.Format("  {0}. DNI: {1}", i + 1, ObtenerValor(afiliacion?.Dni)));
                mensaje.AppendLine("      ID único: " + ObtenerValor(afiliacion?.IdUnico));
                mensaje.AppendLine("      Fecha afiliación: " + ObtenerValor(afiliacion?.FechaAfiliacion));
                mensaje.AppendLine("      Código operación: " + ObtenerValor(afiliacion?.CodigoOperacion));
                mensaje.AppendLine("      Ficha: " + ObtenerValor(afiliacion?.FichaAfiliacion));
            }
        }

        private static void AgregarDetalleAdhesiones(StringBuilder mensaje, IReadOnlyList<ReniecAdhesion> adhesiones)
        {
            mensaje.AppendLine("Adhesiones JNE:");

            if (adhesiones == null || adhesiones.Count == 0)
            {
                mensaje.AppendLine("  (sin registros)");
                return;
            }

            for (int i = 0; i < adhesiones.Count; i++)
            {
                ReniecAdhesion adhesion = adhesiones[i];
                mensaje.AppendLine(string.Format("  {0}. DNI: {1}", i + 1, ObtenerValor(adhesion?.Dni)));
                mensaje.AppendLine("      ID único: " + ObtenerValor(adhesion?.IdUnico));
                mensaje.AppendLine("      Fecha adhesión: " + ObtenerValor(adhesion?.FechaAdhesion));
                mensaje.AppendLine("      Código operación: " + ObtenerValor(adhesion?.CodigoOperacion));
                mensaje.AppendLine("      Ficha: " + ObtenerValor(adhesion?.FichaAdhesion));
            }
        }

        private static string ObtenerValor(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? "(sin dato)" : valor;
        }
    }
}
