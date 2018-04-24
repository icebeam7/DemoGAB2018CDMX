using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using DemoGAB2018CDMX.Helpers;

namespace DemoGAB2018CDMX.Servicios
{
    public static class ServicioText
    {
        private static HttpClient Cliente = new HttpClient();

        public static async Task<string> TraducirTexto(string texto)
        {
            string url = Constantes.TextServiceURL + Uri.EscapeDataString(texto);

            Cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constantes.TextServiceApiKey);
            var response = await Cliente.GetAsync(url);
            var datos = await response.Content.ReadAsStringAsync();
            var xml = XDocument.Parse(datos);
            return xml.Root.Value;
        }
    }
}
