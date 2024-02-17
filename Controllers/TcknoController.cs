using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using practicesNet.Models;
using practicesNet.Services;
using System.Text;
using System.Xml.Linq;

namespace practicesNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TcknoController : ControllerBase
    {
        [HttpPost]
        public async Task<string> TcControl([FromBody] Tckno model)
        {
            
            string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
                                      <soap12:Body>
                                        <TCKimlikNoDogrula xmlns=""http://tckimlik.nvi.gov.tr/WS"">
                                          <TCKimlikNo>{model.tckno}</TCKimlikNo>
                                          <Ad>{model.ad}</Ad>
                                          <Soyad>{model.soyad}</Soyad>
                                          <DogumYili>{model.dogumyili}</DogumYili>
                                        </TCKimlikNoDogrula>
                                      </soap12:Body>
                                    </soap12:Envelope>";

            string url = "https://tckimlik.nvi.gov.tr/Service/KPSPublic.asmx";

            using (HttpClient httpClient = new HttpClient())
            {
                StringContent content = new StringContent(soapRequest, Encoding.UTF8, "application/soap+xml");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                string soapResponse = await response.Content.ReadAsStringAsync();
                XDocument xmlDoc = XDocument.Parse(soapResponse);

                // "TCKimlikNoDogrulaResult" öğesine XPath kullanarak erişme
                XElement resultElement = xmlDoc.Root
                    .Element("{http://www.w3.org/2003/05/soap-envelope}Body")
                    .Element("{http://tckimlik.nvi.gov.tr/WS}TCKimlikNoDogrulaResponse")
                    .Element("{http://tckimlik.nvi.gov.tr/WS}TCKimlikNoDogrulaResult");

                // "TCKimlikNoDogrulaResult" öğesinin içeriğini kontrol ederek true veya false döndürme
                if (resultElement != null)
                {
                    string result = resultElement.Value;
                    if (result == "true")
                    {
                        return "true";
                    }
                    else if (result == "false")
                    {
                        return "false";
                    }
                }

                // Eğer herhangi bir sonuç alınamazsa, null döndür
                return null;
            }
        }

    }
}