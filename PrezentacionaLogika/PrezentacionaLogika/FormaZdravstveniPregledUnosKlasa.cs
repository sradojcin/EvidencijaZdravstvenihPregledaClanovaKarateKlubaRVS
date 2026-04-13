using System;
using KlasePodataka;

namespace PrezentacionaLogika
{
    public class FormaZdravstveniPregledUnosKlasa
    {
        private string _stringKonekcije;
        private int _zdravstveniPregledID;
        private int _clanID;
        private DateTime _datumPregleda;
        private string _napomena;
        private string _poslednjaGreska;

        public int ZdravstveniPregledID
        {
            get { return _zdravstveniPregledID; }
            set { _zdravstveniPregledID = value; }
        }

        public int ClanID
        {
            get { return _clanID; }
            set { _clanID = value; }
        }

        public DateTime DatumPregleda
        {
            get { return _datumPregleda; }
            set { _datumPregleda = value; }
        }

        public string Napomena
        {
            get { return _napomena; }
            set { _napomena = value; }
        }

        public string PoslednjaGreska
        {
            get { return _poslednjaGreska; }
        }

        public FormaZdravstveniPregledUnosKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public bool DodajPregled()
        {
            if (!ValidirajPodatke(false))
            {
                return false;
            }

            ZdravstveniPregledKlasa pregled = new ZdravstveniPregledKlasa();
            pregled.ClanID = _clanID;
            pregled.DatumPregleda = _datumPregleda;
            pregled.Napomena = _napomena;

            SPZdravstveniPregledDBKlasa spPregled = new SPZdravstveniPregledDBKlasa(_stringKonekcije);
            return spPregled.DodajZdravstveniPregled(pregled);
        }

        public bool IzmeniPregled()
        {
            if (!ValidirajPodatke(true))
            {
                return false;
            }

            ZdravstveniPregledKlasa pregled = new ZdravstveniPregledKlasa();
            pregled.ZdravstveniPregledID = _zdravstveniPregledID;
            pregled.ClanID = _clanID;
            pregled.DatumPregleda = _datumPregleda;
            pregled.Napomena = _napomena;

            SPZdravstveniPregledDBKlasa spPregled = new SPZdravstveniPregledDBKlasa(_stringKonekcije);
            return spPregled.IzmeniZdravstveniPregled(pregled);
        }

        public bool ObrisiPregled()
        {
            if (_zdravstveniPregledID <= 0)
            {
                _poslednjaGreska = "Pregled za brisanje nije ispravno izabran.";
                return false;
            }

            _poslednjaGreska = string.Empty;
            SPZdravstveniPregledDBKlasa spPregled = new SPZdravstveniPregledDBKlasa(_stringKonekcije);
            return spPregled.ObrisiZdravstveniPregled(_zdravstveniPregledID);
        }

        public bool ValidirajPodatke(bool validirajID)
        {
            if (validirajID && _zdravstveniPregledID <= 0)
            {
                _poslednjaGreska = "Pregled za izmenu nije ispravno izabran.";
                return false;
            }

            if (_clanID <= 0)
            {
                _poslednjaGreska = "Clan mora biti izabran.";
                return false;
            }

            if (_datumPregleda == DateTime.MinValue)
            {
                _poslednjaGreska = "Datum pregleda je obavezan.";
                return false;
            }

            if (_datumPregleda.Date > DateTime.Today)
            {
                _poslednjaGreska = "Datum pregleda ne moze biti u buducnosti.";
                return false;
            }

            if (_datumPregleda.Date < new DateTime(2000, 1, 1))
            {
                _poslednjaGreska = "Datum pregleda nije u dozvoljenom opsegu.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_napomena) && _napomena.Trim().Length > 500)
            {
                _poslednjaGreska = "Napomena moze imati najvise 500 karaktera.";
                return false;
            }

            _napomena = string.IsNullOrWhiteSpace(_napomena) ? string.Empty : _napomena.Trim();
            _poslednjaGreska = string.Empty;
            return true;
        }
    }
}
