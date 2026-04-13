using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaZdravstveniPregledPregledKlasa
    {
        private string _stringKonekcije;

        public FormaZdravstveniPregledPregledKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public ZdravstveniPregledListaKlasa DajSvePreglede()
        {
            SPZdravstveniPregledDBKlasa spPregled = new SPZdravstveniPregledDBKlasa(_stringKonekcije);
            return spPregled.DajSveZdravstvenePreglede();
        }

        public ZdravstveniPregledListaKlasa DajPregledeZaClana(int clanIDParametar)
        {
            SPZdravstveniPregledDBKlasa spPregled = new SPZdravstveniPregledDBKlasa(_stringKonekcije);
            return spPregled.DajPregledeZaClana(clanIDParametar);
        }
    }
}
