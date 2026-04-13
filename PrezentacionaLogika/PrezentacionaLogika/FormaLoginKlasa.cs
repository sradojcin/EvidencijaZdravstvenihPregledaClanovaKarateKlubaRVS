using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaLoginKlasa
    {
        private string _stringKonekcije;
        private string _korisnickoIme;
        private string _lozinka;
        private string _poslednjaGreska;

        public string KorisnickoIme
        {
            get { return _korisnickoIme; }
            set { _korisnickoIme = value; }
        }

        public string Lozinka
        {
            get { return _lozinka; }
            set { _lozinka = value; }
        }

        public string PoslednjaGreska
        {
            get { return _poslednjaGreska; }
        }

        public FormaLoginKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public KorisnikKlasa Prijava()
        {
            if (!ValidirajPodatke())
            {
                return null;
            }

            KorisnikDBKlasa korisnikDb = new KorisnikDBKlasa(_stringKonekcije);
            return korisnikDb.Prijava(_korisnickoIme, _lozinka);
        }

        public bool ValidirajPodatke()
        {
            if (string.IsNullOrWhiteSpace(_korisnickoIme))
            {
                _poslednjaGreska = "Korisnicko ime je obavezno.";
                return false;
            }

            if (_korisnickoIme.Trim().Length < 3)
            {
                _poslednjaGreska = "Korisnicko ime mora imati najmanje 3 karaktera.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_lozinka))
            {
                _poslednjaGreska = "Lozinka je obavezna.";
                return false;
            }

            if (_lozinka.Trim().Length < 3)
            {
                _poslednjaGreska = "Lozinka mora imati najmanje 3 karaktera.";
                return false;
            }

            _poslednjaGreska = string.Empty;
            return true;
        }
    }
}
