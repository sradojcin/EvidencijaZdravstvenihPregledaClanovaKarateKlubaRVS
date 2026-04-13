using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using KlasePodataka;
using PoslovnaLogika;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class Clanovi : System.Web.UI.Page
    {
        private readonly string _stringKonekcije =
            ConfigurationManager.ConnectionStrings["VP2026KarateKlubZdravstveniPregledV1"]?.ConnectionString
            ?? ConfigurationManager.ConnectionStrings["NasaKonekcija"]?.ConnectionString
            ?? @"Data Source=.\SQLEXPRESS01;Initial Catalog=VP2026KarateKlubZdravstveniPreglediV1;Integrated Security=True";

        private readonly string _urlServisaZaKategorije =
            ConfigurationManager.AppSettings["KategorijeClanovaServisUrl"] ?? "http://localhost:5039";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KorisnikID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                UcitajClanove();
            }
        }

        private void UcitajClanove()
        {
            try
            {
                FormaClanPregledKlasa pregled = new FormaClanPregledKlasa(_stringKonekcije);
                ClanListaKlasa lista = pregled.DajSveClanove();
                KategorisanjeClanaKlasa kategorisanje = new KategorisanjeClanaKlasa(_urlServisaZaKategorije);
                List<ClanPrikazModel> prikaz = new List<ClanPrikazModel>();
                List<string> servisneGreske = new List<string>();

                foreach (ClanKlasa clan in lista.Lista)
                {
                    string kategorija;

                    try
                    {
                        kategorija = kategorisanje.DajKategoriju(clan.DatumRodjenja);
                    }
                    catch (Exception ex)
                    {
                        kategorija = "N/A";

                        if (!servisneGreske.Contains(ex.Message))
                        {
                            servisneGreske.Add(ex.Message);
                        }
                    }

                    prikaz.Add(new ClanPrikazModel
                    {
                        ClanID = clan.ClanID,
                        JMBG = clan.JMBG,
                        Ime = clan.Ime,
                        Prezime = clan.Prezime,
                        DatumRodjenja = clan.DatumRodjenja.ToString("yyyy-MM-dd"),
                        Kategorija = kategorija
                    });
                }

                gvClanovi.DataSource = prikaz;
                gvClanovi.DataBind();

                if (servisneGreske.Any())
                {
                    lblStatus.Text = "Kategorije nisu ucitane iz servisa: " + servisneGreske.First();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska pri ucitavanju clanova: " + ex.Message;
            }
        }

        protected void btnDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime datumRodjenja;
                if (!DateTime.TryParse(txtDatumRodjenja.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datumRodjenja) &&
                    !DateTime.TryParseExact(txtDatumRodjenja.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumRodjenja))
                {
                    lblStatus.Text = "Datum rodjenja nije u ispravnom formatu.";
                    return;
                }

                FormaClanUnosKlasa forma = new FormaClanUnosKlasa(_stringKonekcije);
                forma.JMBG = txtJMBG.Text.Trim();
                forma.Ime = txtIme.Text.Trim();
                forma.Prezime = txtPrezime.Text.Trim();
                forma.DatumRodjenja = datumRodjenja;

                if (forma.DodajClana())
                {
                    lblStatus.Text = "Clan je uspesno dodat.";
                    OcistiPolja();
                    UcitajClanove();
                }
                else
                {
                    lblStatus.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Dodavanje clana nije uspelo."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnIzmeni_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfClanID.Value))
            {
                lblStatus.Text = "Prvo izaberi clana iz tabele.";
                return;
            }

            try
            {
                DateTime datumRodjenja;
                if (!DateTime.TryParse(txtDatumRodjenja.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datumRodjenja) &&
                    !DateTime.TryParseExact(txtDatumRodjenja.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumRodjenja))
                {
                    lblStatus.Text = "Datum rodjenja nije u ispravnom formatu.";
                    return;
                }

                FormaClanUnosKlasa forma = new FormaClanUnosKlasa(_stringKonekcije);
                forma.ClanID = Convert.ToInt32(hfClanID.Value);
                forma.JMBG = txtJMBG.Text.Trim();
                forma.Ime = txtIme.Text.Trim();
                forma.Prezime = txtPrezime.Text.Trim();
                forma.DatumRodjenja = datumRodjenja;

                if (forma.IzmeniClana())
                {
                    lblStatus.Text = "Clan je uspesno izmenjen.";
                    OcistiPolja();
                    UcitajClanove();
                }
                else
                {
                    lblStatus.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Izmena clana nije uspela."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnObrisi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfClanID.Value))
            {
                lblStatus.Text = "Prvo izaberi clana iz tabele.";
                return;
            }

            try
            {
                FormaClanUnosKlasa forma = new FormaClanUnosKlasa(_stringKonekcije);
                forma.ClanID = Convert.ToInt32(hfClanID.Value);

                if (forma.ObrisiClana())
                {
                    lblStatus.Text = "Clan je uspesno obrisan.";
                    OcistiPolja();
                    UcitajClanove();
                }
                else
                {
                    lblStatus.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Brisanje clana nije uspelo."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnOcisti_Click(object sender, EventArgs e)
        {
            OcistiPolja();
        }

        private void OcistiPolja()
        {
            hfClanID.Value = "";
            txtJMBG.Text = "";
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtDatumRodjenja.Text = "";
            txtKategorija.Text = "";
        }

        protected void gvClanovi_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfClanID.Value = gvClanovi.SelectedDataKey.Value.ToString();
            txtJMBG.Text = gvClanovi.SelectedRow.Cells[1].Text;
            txtIme.Text = gvClanovi.SelectedRow.Cells[2].Text;
            txtPrezime.Text = gvClanovi.SelectedRow.Cells[3].Text;
            txtDatumRodjenja.Text = gvClanovi.SelectedRow.Cells[4].Text;
            txtKategorija.Text = gvClanovi.SelectedRow.Cells[5].Text;
        }

        public class ClanPrikazModel
        {
            public int ClanID { get; set; }
            public string JMBG { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string DatumRodjenja { get; set; }
            public string Kategorija { get; set; }
        }
    }
}
