namespace KategorijeClanovaServis.Models
{
    public class KategorijaClanaXmlModel
    {
        public string SifraKategorije { get; set; } = string.Empty;
        public string NazivKategorije { get; set; } = string.Empty;
        public int MinGodina { get; set; }
        public int MaxGodina { get; set; }
    }
}
