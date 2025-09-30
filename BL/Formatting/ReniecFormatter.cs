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
                ReniecAfiliacion afiliacion = ciudadano.Afiliaciones[i];
                lineas.Add(string.Format("  {0}. DNI: {1}", i + 1, string.IsNullOrWhiteSpace(afiliacion.Dni) ? "(sin dato)" : afiliacion.Dni));

                if (!string.IsNullOrWhiteSpace(afiliacion.FechaAfiliacion))
                {
                    lineas.Add("      Fecha afiliación: " + afiliacion.FechaAfiliacion);
                }

                if (!string.IsNullOrWhiteSpace(afiliacion.CodigoOperacion))
                {
                    lineas.Add("      Código operación: " + afiliacion.CodigoOperacion);
                }

                if (!string.IsNullOrWhiteSpace(afiliacion.FichaAfiliacion))
                {
                    lineas.Add("      Ficha: " + afiliacion.FichaAfiliacion);
                }
            }
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
                ReniecAdhesion adhesion = ciudadano.Adhesiones[i];
                lineas.Add(string.Format("  {0}. DNI: {1}", i + 1, string.IsNullOrWhiteSpace(adhesion.Dni) ? "(sin dato)" : adhesion.Dni));

                if (!string.IsNullOrWhiteSpace(adhesion.FechaAdhesion))
                {
                    lineas.Add("      Fecha adhesión: " + adhesion.FechaAdhesion);
                }

                if (!string.IsNullOrWhiteSpace(adhesion.CodigoOperacion))
                {
                    lineas.Add("      Código operación: " + adhesion.CodigoOperacion);
                }

                if (!string.IsNullOrWhiteSpace(adhesion.FichaAdhesion))
                {
                    lineas.Add("      Ficha: " + adhesion.FichaAdhesion);
                }
            }
        }
    }
}
