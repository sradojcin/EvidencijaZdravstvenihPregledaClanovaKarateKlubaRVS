using System;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace PoslovnaLogika
{
    public class KategorisanjeClanaKlasa
    {
        private readonly string _urlServisa;

        public KategorisanjeClanaKlasa(string urlServisa)
        {
            _urlServisa = (urlServisa ?? string.Empty).Trim();
        }

        public string DajKategoriju(DateTime datumRodjenja)
        {
            if (string.IsNullOrWhiteSpace(_urlServisa))
            {
                throw new InvalidOperationException("URL SOAP servisa za kategorije nije podesen.");
            }

            int godine = DateTime.Today.Year - datumRodjenja.Year;

            if (datumRodjenja.Date > DateTime.Today.AddYears(-godine))
            {
                godine--;
            }

            using (KategorijeClanovaSoapKlijent klijent = new KategorijeClanovaSoapKlijent())
            {
                klijent.Url = FormirajUrlServisa();
                klijent.Timeout = 5000;

                string rezultat = klijent.DajNazivKategorije(godine);

                if (string.IsNullOrWhiteSpace(rezultat))
                {
                    throw new InvalidOperationException(
                        "SOAP servis za kategorije nije vratio naziv kategorije za broj godina " + godine + ".");
                }

                return rezultat.Trim();
            }
        }

        private string FormirajUrlServisa()
        {
            if (_urlServisa.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase))
            {
                return _urlServisa;
            }

            return _urlServisa.TrimEnd('/') + "/KategorijeClanovaSoapServis.asmx";
        }

        [WebServiceBinding(Name = "KategorijeClanovaSoapServisSoap", Namespace = "http://tempuri.org/")]
        private sealed class KategorijeClanovaSoapKlijent : SoapHttpClientProtocol
        {
            [SoapDocumentMethod("http://tempuri.org/DajNazivKategorije",
                RequestNamespace = "http://tempuri.org/",
                ResponseNamespace = "http://tempuri.org/",
                Use = SoapBindingUse.Literal,
                ParameterStyle = SoapParameterStyle.Wrapped)]
            public string DajNazivKategorije(int brojGodina)
            {
                object[] rezultati = Invoke("DajNazivKategorije", new object[] { brojGodina });
                return (string)rezultati[0];
            }
        }
    }
}
