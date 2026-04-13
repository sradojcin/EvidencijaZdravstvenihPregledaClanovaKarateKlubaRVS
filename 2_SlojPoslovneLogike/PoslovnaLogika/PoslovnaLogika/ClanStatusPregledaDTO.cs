using System;

namespace PoslovnaLogika
{
	public class ClanStatusPregledaDTO
	{
		public int ClanID { get; set; }

		public string JMBG { get; set; }

		public string Ime { get; set; }

		public string Prezime { get; set; }

		public DateTime DatumRodjenja { get; set; }

		public DateTime? DatumPregleda { get; set; }

		public string StatusPregleda { get; set; }
	}
}