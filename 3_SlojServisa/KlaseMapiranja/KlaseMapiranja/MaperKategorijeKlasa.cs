using System;

namespace KlaseMapiranja
{
    public class MaperKategorijeKlasa
    {
        public MaperKategorijeKlasa()
        {
        }

        public string DajSifruKategorijeZaWebServis(string nazivKategorijeParametar)
        {
            string pomSifra = "";

            switch (nazivKategorijeParametar.Trim().ToLower())
            {
                case "poletarac":
                    pomSifra = "pol";
                    break;
                case "pionir":
                    pomSifra = "pio";
                    break;
                case "nada":
                    pomSifra = "nad";
                    break;
                case "kadet":
                    pomSifra = "kad";
                    break;
                case "junior":
                    pomSifra = "jun";
                    break;
                case "senior":
                    pomSifra = "sen";
                    break;
                default:
                    pomSifra = "";
                    break;
            }

            return pomSifra;
        }

        public string DajNazivKategorijeIzSifreWebServisa(string sifraKategorijeParametar)
        {
            string pomNaziv = "";

            switch (sifraKategorijeParametar.Trim().ToLower())
            {
                case "pol":
                    pomNaziv = "Poletarac";
                    break;
                case "pio":
                    pomNaziv = "Pionir";
                    break;
                case "nad":
                    pomNaziv = "Nada";
                    break;
                case "kad":
                    pomNaziv = "Kadet";
                    break;
                case "jun":
                    pomNaziv = "Junior";
                    break;
                case "sen":
                    pomNaziv = "Senior";
                    break;
                default:
                    pomNaziv = "";
                    break;
            }

            return pomNaziv;
        }
    }
}
