using System.Collections.Generic;
using System.Web.Services;
using KategorijeClanovaServis.Models;
using KategorijeClanovaServis.Services;

namespace KategorijeClanovaServis
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class KategorijeClanovaSoapServis : WebService
    {
        [WebMethod]
        public List<KategorijaClanaXmlModel> DajSveKategorije()
        {
            KategorijeClanovaXmlServis servis = new KategorijeClanovaXmlServis(Server.MapPath("~/"));
            return servis.DajSveKategorije();
        }

        [WebMethod]
        public string DajNazivKategorije(int brojGodina)
        {
            KategorijeClanovaXmlServis servis = new KategorijeClanovaXmlServis(Server.MapPath("~/"));
            return servis.DajNazivKategorije(brojGodina);
        }
    }
}
