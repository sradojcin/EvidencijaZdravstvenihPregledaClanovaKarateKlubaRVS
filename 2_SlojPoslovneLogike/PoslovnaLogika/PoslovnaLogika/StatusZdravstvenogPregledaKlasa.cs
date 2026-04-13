using System;
using System.Data;
using System.IO;

namespace PoslovnaLogika
{
	public class StatusZdravstvenogPregledaKlasa
	{
		// konstruktor
		public StatusZdravstvenogPregledaKlasa()
		{

		}

		// public metode
		public string OdrediStatus(DateTime? datumPregledaParametar)
		{
			if (!datumPregledaParametar.HasValue)
			{
				return "Nema pregleda";
			}

			int brojMeseci = DajRokVazenjaPregledaUMesecima();
			DateTime datumIsteka = datumPregledaParametar.Value.AddMonths(brojMeseci);

			if (datumIsteka >= DateTime.Today)
			{
				return "Aktivan";
			}

			return "Neaktivan";
		}

		// private metode
		private int DajRokVazenjaPregledaUMesecima()
		{
			string putanja = DajPutanjuXmlFajla();

			DataSet ds = new DataSet();
			ds.ReadXml(putanja);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				throw new Exception("XML fajl sa podesavanjima pregleda je prazan ili neispravan.");
			}

			return Convert.ToInt32(ds.Tables[0].Rows[0]["RokVazenjaPregledaUMesecima"]);
		}

		private string DajPutanjuXmlFajla()
		{
			string osnovniDirektorijum = AppDomain.CurrentDomain.BaseDirectory;

			string putanja = Path.Combine(osnovniDirektorijum, "XML", "PodesavanjaPregleda.xml");

			if (File.Exists(putanja))
			{
				return putanja;
			}

			string putanjaUBin = Path.Combine(osnovniDirektorijum, "bin", "XML", "PodesavanjaPregleda.xml");

			if (File.Exists(putanjaUBin))
			{
				return putanjaUBin;
			}

			throw new FileNotFoundException("XML fajl sa podesavanjima pregleda nije pronadjen.");
		}
	}
}