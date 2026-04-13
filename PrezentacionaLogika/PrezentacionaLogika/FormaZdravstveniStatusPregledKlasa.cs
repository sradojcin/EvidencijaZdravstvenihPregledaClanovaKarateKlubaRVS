using System.Collections.Generic;
using PoslovnaLogika;

namespace PrezentacionaLogika
{
    public class FormaZdravstveniStatusPregledKlasa
    {
        private string _stringKonekcije;

        public FormaZdravstveniStatusPregledKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public List<ClanStatusPregledaDTO> DajSveClanoveSaStatusom()
        {
            PregledZdravstvenogStatusaClanovaKlasa pregled =
                new PregledZdravstvenogStatusaClanovaKlasa(_stringKonekcije);

            return pregled.DajPregledClanovaSaStatusom();
        }

        public List<ClanStatusPregledaDTO> FiltrirajPoStatusu(string statusParametar)
        {
            FiltriranjeClanovaPoStatusuKlasa filtriranje =
                new FiltriranjeClanovaPoStatusuKlasa(_stringKonekcije);

            return filtriranje.FiltrirajPoStatusu(statusParametar);
        }
    }
}