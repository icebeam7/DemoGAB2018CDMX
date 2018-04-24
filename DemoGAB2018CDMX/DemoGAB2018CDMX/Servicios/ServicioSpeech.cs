using System;
using System.IO;
using System.Xml.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PCLStorage;
using Newtonsoft.Json;
using DemoGAB2018CDMX.Helpers;
using DemoGAB2018CDMX.Modelos;

namespace DemoGAB2018CDMX.Servicios
{
    public static class ServicioSpeech
    {
        private static HttpClient Cliente = new HttpClient();

        private static async Task<string> ObtenerToken()
        {
            Cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constantes.SpeechServiceApiKey);
            var token = await Cliente.PostAsync(Constantes.FetchTokenURL, null);
            return await token.Content.ReadAsStringAsync();
        }

        public static async Task<string> ReconocerVozTexto()
        {
            try
            {
                var archivo = await FileSystem.Current.LocalStorage.GetFileAsync(Constantes.AudioFile);

                using (var stream = await archivo.OpenAsync(PCLStorage.FileAccess.Read))
                {
                    var token = await ObtenerToken();
                    var respuesta = await ObtenerTextoDeVoz(stream, token);
                    var speech = JsonConvert.DeserializeObject<ResultadoSpeech>(respuesta);
                    return speech.DisplayText;
                }
            }
            catch (Exception ex)
            {
                return "Error al procesar la grabación";
            }
        }

        public static async Task<byte[]> ConvertirTexto_Voz(string texto)
        {
            try
            {
                var token = await ObtenerToken();
                var bytes = await ObtenerVozDeTexto(texto, token);
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static async Task<string> ObtenerTextoDeVoz(Stream stream, string token)
        {
            Cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constantes.SpeechServiceApiKey);
            Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var contenido = new StreamContent(stream);
            contenido.Headers.TryAddWithoutValidation("Content-Type", Constantes.AudioContentType);

            var respuesta = await Cliente.PostAsync(Constantes.SpeechServiceURL, contenido);
            return await respuesta.Content.ReadAsStringAsync();
        }

        private static async Task<byte[]> ObtenerVozDeTexto(string texto, string token)
        {
            try
            {
                Cliente.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constantes.SpeechServiceApiKey);
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var ssml = GenerarSsml(texto);
                var contenido = new StringContent(ssml);

                contenido.Headers.TryAddWithoutValidation("Content-Type", Constantes.SsmlXmlContentType);
                contenido.Headers.TryAddWithoutValidation("X-Microsoft-OutputFormat", Constantes.OutputFormat);
                contenido.Headers.TryAddWithoutValidation("X-Search-AppId", Constantes.AppId);
                contenido.Headers.TryAddWithoutValidation("X-Search-ClientID", Constantes.ClientId);
                Cliente.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Constantes.ApplicationName, Constantes.Version));

                var respuesta = await Cliente.PostAsync(Constantes.TextToSpeechServiceURL, contenido);
                var stream = await respuesta.Content.ReadAsStreamAsync();

                using (stream)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] waveBytes = null;
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);

                        waveBytes = ms.ToArray();
                        return waveBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GenerarSsml(string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", Constantes.Locale),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", Constantes.Locale),
                                      new XAttribute(XNamespace.Xml + "gender", Constantes.Gender),
                                      new XAttribute("name", Constantes.VoiceName),
                                      text)));
            return ssmlDoc.ToString();
        }
    }
}
