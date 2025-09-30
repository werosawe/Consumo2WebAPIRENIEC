using System;
using System.Collections.Generic;
using Consumo2WebAPIRENIEC.BE.Entities;

namespace Consumo2WebAPIRENIEC.BL.Formatting
{
    public static class ReniecFormatter
    {
        public static string FormatearCiudadano(ReniecCiudadano ciudadano)
        {
            if (ciudadano == null)
            {
                return string.Empty;
            }

            var lineas = new List<string>();

            if (!string.IsNullOrWhiteSpace(ciudadano.NumeroDocumento))
            {
                lineas.Add("Documento: " + ciudadano.NumeroDocumento);
            }

            if (!string.IsNullOrWhiteSpace(ciudadano.NombreCompleto))
            {
                lineas.Add("Nombre: " + ciudadano.NombreCompleto);
            }

            if (!string.IsNullOrWhiteSpace(ciudadano.EstadoCivil))
            {
                lineas.Add("Estado civil: " + ciudadano.EstadoCivil);
            }

            if (!string.IsNullOrWhiteSpace(ciudadano.UbigeoDireccion))
            {
                lineas.Add("Ubigeo: " + ciudadano.UbigeoDireccion);
            }

            AgregarAfiliaciones(ciudadano, lineas);
            AgregarAdhesiones(ciudadano, lineas);

            return string.Join(Environment.NewLine, lineas);
        }

        private static void AgregarAfiliaciones(ReniecCiudadano ciudadano, List<string> lineas)
        {
            if (ciudadano.Afiliaciones == null || ciudadano.Afiliaciones.Count == 0)
            {
                return;
            }

            lineas.Add("Afiliaciones:");
            for (int i = 0; i < ciudadano.Afiliaciones.Count; i++)
            {
                AgregarDetalleAfiliacion(lineas, ciudadano.Afiliaciones[i], i + 1);
            }
        }

        private static void AgregarDetalleAfiliacion(List<string> lineas, ReniecAfiliacion afiliacion, int indice)
        {
            if (afiliacion == null)
            {
                return;
            }

            lineas.Add(string.Format("  {0}. DNI: {1}", indice, ObtenerValor(afiliacion.Dni)));
            lineas.Add("      ID único: " + ObtenerValor(afiliacion.IdUnico));
            lineas.Add("      Fecha afiliación: " + ObtenerValor(afiliacion.FechaAfiliacion));
            lineas.Add("      Código operación: " + ObtenerValor(afiliacion.CodigoOperacion));
            lineas.Add("      Ficha: " + ObtenerValor(afiliacion.FichaAfiliacion));
        }

        private static void AgregarAdhesiones(ReniecCiudadano ciudadano, List<string> lineas)
        {
            if (ciudadano.Adhesiones == null || ciudadano.Adhesiones.Count == 0)
            {
                return;
            }

            lineas.Add("Adhesiones:");
            for (int i = 0; i < ciudadano.Adhesiones.Count; i++)
            {
                AgregarDetalleAdhesion(lineas, ciudadano.Adhesiones[i], i + 1);
            }
        }

        private static void AgregarDetalleAdhesion(List<string> lineas, ReniecAdhesion adhesion, int indice)
        {
            if (adhesion == null)
            {
                return;
            }

            lineas.Add(string.Format("  {0}. DNI: {1}", indice, ObtenerValor(adhesion.Dni)));
            lineas.Add("      ID único: " + ObtenerValor(adhesion.IdUnico));
            lineas.Add("      Fecha adhesión: " + ObtenerValor(adhesion.FechaAdhesion));
            lineas.Add("      Código operación: " + ObtenerValor(adhesion.CodigoOperacion));
            lineas.Add("      Ficha: " + ObtenerValor(adhesion.FichaAdhesion));
        }

        private static string ObtenerValor(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? "(sin dato)" : valor;
        }
    }
}
