using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using KategorijeClanovaServis.Models;

namespace KategorijeClanovaServis.Services
{
    public class KategorijeClanovaXmlServis
    {
        private readonly string _koreniDirektorijum;

        public KategorijeClanovaXmlServis(string koreniDirektorijum)
        {
            _koreniDirektorijum = koreniDirektorijum;
        }

        public List<KategorijaClanaXmlModel> DajSveKategorije()
        {
            List<KategorijaClanaXmlModel> lista = new List<KategorijaClanaXmlModel>();

            string putanja = DajPutanjuXmlFajla();

            DataSet ds = new DataSet();
            ds.ReadXml(putanja);

            if (ds.Tables.Count == 0)
            {
                return lista;
            }

            foreach (DataRow red in ds.Tables[0].Rows)
            {
                KategorijaClanaXmlModel kategorija = new KategorijaClanaXmlModel
                {
                    SifraKategorije = red["SifraKategorije"].ToString() ?? string.Empty,
                    NazivKategorije = red["NazivKategorije"].ToString() ?? string.Empty,
                    MinGodina = Convert.ToInt32(red["MinGodina"]),
                    MaxGodina = Convert.ToInt32(red["MaxGodina"])
                };

                lista.Add(kategorija);
            }

            return lista;
        }

        public string DajNazivKategorije(int brojGodina)
        {
            List<KategorijaClanaXmlModel> kategorije = DajSveKategorije();

            KategorijaClanaXmlModel kategorija = kategorije.Find(
                delegate (KategorijaClanaXmlModel stavka)
                {
                    return brojGodina >= stavka.MinGodina && brojGodina <= stavka.MaxGodina;
                });

            return kategorija == null ? string.Empty : kategorija.NazivKategorije;
        }

        private string DajPutanjuXmlFajla()
        {
            string putanjaURadnomDirektorijumu = Path.Combine(_koreniDirektorijum, "XML", "SpisakKategorija.xml");

            if (File.Exists(putanjaURadnomDirektorijumu))
            {
                return putanjaURadnomDirektorijumu;
            }

            string putanjaUProjektu = Path.GetFullPath(Path.Combine(_koreniDirektorijum, "..", "..", "..", "XML", "SpisakKategorija.xml"));

            if (File.Exists(putanjaUProjektu))
            {
                return putanjaUProjektu;
            }

            throw new FileNotFoundException("XML fajl sa kategorijama nije pronadjen.");
        }
    }
}
