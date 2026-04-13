using System;
using System.Collections.Generic;
using System.Data;
using KlasePodataka;

namespace PoslovnaLogika
{
	public class FiltriranjeClanovaPoStatusuKlasa
	{
		// atribut
		private string _stringKonekcije;

		// konstruktor
		public FiltriranjeClanovaPoStatusuKlasa(string noviStringKonekcije)
		{
			_stringKonekcije = noviStringKonekcije;
		}

		// public metoda
		public List<ClanStatusPregledaDTO> FiltrirajPoStatusu(string statusParametar)
		{
			List<ClanStatusPregledaDTO> rezultat = new List<ClanStatusPregledaDTO>();

			ClanStatusPregledaDBKlasa dbKlasa = new ClanStatusPregledaDBKlasa(_stringKonekcije);
			DataSet ds = dbKlasa.FiltrirajClanovePoStatusu(statusParametar);

			if (ds.Tables.Count == 0)
			{
				return rezultat;
			}

			foreach (DataRow red in ds.Tables[0].Rows)
			{
				ClanStatusPregledaDTO dto = new ClanStatusPregledaDTO();

				dto.ClanID = Convert.ToInt32(red["ClanID"]);
				dto.JMBG = red["JMBG"].ToString();
				dto.Ime = red["Ime"].ToString();
				dto.Prezime = red["Prezime"].ToString();
				dto.DatumRodjenja = Convert.ToDateTime(red["DatumRodjenja"]);
				dto.DatumPregleda = red["PoslednjiPregled"] != DBNull.Value
					? Convert.ToDateTime(red["PoslednjiPregled"])
					: (DateTime?)null;
				dto.StatusPregleda = red["StatusZdravstvenogPregleda"].ToString();

				rezultat.Add(dto);
			}

			return rezultat;
		}
	}
}
