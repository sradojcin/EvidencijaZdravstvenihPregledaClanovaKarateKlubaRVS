using System;
using System.Linq;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaClanUnosKlasa
    {
        private string _stringKonekcije;
        private int _clanID;
        private string _jmbg;
        private string _ime;
        private string _prezime;
        private DateTime _datumRodjenja;
        private string _poslednjaGreska;

        public int ClanID
        {
            get { return _clanID; }
            set { _clanID = value; }
        }

        public string JMBG
        {
            get { return _jmbg; }
            set { _jmbg = value; }
        }

        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }

        public string Prezime
        {
            get { return _prezime; }
            set { _prezime = value; }
        }

        public DateTime DatumRodjenja
        {
            get { return _datumRodjenja; }
            set { _datumRodjenja = value; }
        }

        public string PoslednjaGreska
        {
            get { return _poslednjaGreska; }
        }

        public FormaClanUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public bool DodajClana()
        {
            if (!ValidirajPodatke(false))
            {
                return false;
            }

            ClanKlasa clan = new ClanKlasa();
            clan.JMBG = _jmbg;
            clan.Ime = _ime;
            clan.Prezime = _prezime;
            clan.DatumRodjenja = _datumRodjenja;

            ClanDBKlasa clanDb = new ClanDBKlasa(_stringKonekcije);
            return clanDb.DodajClana(clan);
        }

        public bool IzmeniClana()
        {
            if (!ValidirajPodatke(true))
            {
                return false;
            }

            ClanKlasa clan = new ClanKlasa();
            clan.ClanID = _clanID;
            clan.JMBG = _jmbg;
            clan.Ime = _ime;
            clan.Prezime = _prezime;
            clan.DatumRodjenja = _datumRodjenja;

            ClanDBKlasa clanDb = new ClanDBKlasa(_stringKonekcije);
            return clanDb.IzmeniClana(clan);
        }

        public bool ObrisiClana()
        {
            if (_clanID <= 0)
            {
                _poslednjaGreska = "Clan za brisanje nije ispravno izabran.";
                return false;
            }

            _poslednjaGreska = string.Empty;
            ClanDBKlasa clanDb = new ClanDBKlasa(_stringKonekcije);
            return clanDb.ObrisiClana(_clanID);
        }

        public bool ValidirajPodatke(bool validirajID)
        {
            if (validirajID && _clanID <= 0)
            {
                _poslednjaGreska = "Clan za izmenu nije ispravno izabran.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_jmbg))
            {
                _poslednjaGreska = "JMBG je obavezan.";
                return false;
            }

            string jmbg = _jmbg.Trim();
            if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
            {
                _poslednjaGreska = "JMBG mora sadrzati tacno 13 cifara.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_ime))
            {
                _poslednjaGreska = "Ime je obavezno.";
                return false;
            }

            if (_ime.Trim().Length < 2)
            {
                _poslednjaGreska = "Ime mora imati najmanje 2 karaktera.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(_prezime))
            {
                _poslednjaGreska = "Prezime je obavezno.";
                return false;
            }

            if (_prezime.Trim().Length < 2)
            {
                _poslednjaGreska = "Prezime mora imati najmanje 2 karaktera.";
                return false;
            }

            if (_datumRodjenja == DateTime.MinValue)
            {
                _poslednjaGreska = "Datum rodjenja je obavezan.";
                return false;
            }

            if (_datumRodjenja.Date > DateTime.Today)
            {
                _poslednjaGreska = "Datum rodjenja ne moze biti u buducnosti.";
                return false;
            }

            if (_datumRodjenja.Date < new DateTime(1900, 1, 1))
            {
                _poslednjaGreska = "Datum rodjenja nije u dozvoljenom opsegu.";
                return false;
            }

            _jmbg = jmbg;
            _ime = _ime.Trim();
            _prezime = _prezime.Trim();
            _poslednjaGreska = string.Empty;
            return true;
        }
    }
}
