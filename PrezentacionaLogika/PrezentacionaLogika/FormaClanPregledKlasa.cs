using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaClanPregledKlasa
    {
        private string _stringKonekcije;

        public FormaClanPregledKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public ClanListaKlasa DajSveClanove()
        {
            ClanDBKlasa clanDb = new ClanDBKlasa(_stringKonekcije);
            return clanDb.DajSveClanove();
        }

        public ClanKlasa DajClanaPoID(int clanIDParametar)
        {
            ClanDBKlasa clanDb = new ClanDBKlasa(_stringKonekcije);
            return clanDb.DajClanaPoID(clanIDParametar);
        }
    }
}
