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
    public partial class Pregledi : System.Web.UI.Page
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
                UcitajFilterStatusa();
                UcitajStatusTabelu();
                UcitajSvePreglede();
            }
        }

        private void UcitajClanove()
        {
            try
            {
                FormaClanPregledKlasa pregled = new FormaClanPregledKlasa(_stringKonekcije);
                ClanListaKlasa lista = pregled.DajSveClanove();

                ddlClanovi.DataSource = lista.Lista;
                ddlClanovi.DataTextField = "Prezime";
                ddlClanovi.DataValueField = "ClanID";
                ddlClanovi.DataBind();

                ddlClanZaStampu.DataSource = lista.Lista;
                ddlClanZaStampu.DataTextField = "Prezime";
                ddlClanZaStampu.DataValueField = "ClanID";
                ddlClanZaStampu.DataBind();
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska pri ucitavanju clanova: " + ex.Message;
            }
        }

        private void UcitajFilterStatusa()
        {
            ddlFilterStatus.Items.Clear();
            ddlFilterStatus.Items.Add("Svi");
            ddlFilterStatus.Items.Add("Aktivan");
            ddlFilterStatus.Items.Add("Neaktivan");
            ddlFilterStatus.Items.Add("Nema pregleda");
        }

        private void UcitajStatusTabelu()
        {
            try
            {
                FormaZdravstveniStatusPregledKlasa formaStatus =
                    new FormaZdravstveniStatusPregledKlasa(_stringKonekcije);
                List<ClanStatusPregledaDTO> lista;

                if (ddlFilterStatus.SelectedValue != null && ddlFilterStatus.SelectedValue != "Svi")
                {
                    lista = formaStatus.FiltrirajPoStatusu(ddlFilterStatus.SelectedValue);
                }
                else
                {
                    lista = formaStatus.DajSveClanoveSaStatusom();
                }

                KategorisanjeClanaKlasa kategorisanje = new KategorisanjeClanaKlasa(_urlServisaZaKategorije);
                List<StatusPrikazModel> prikaz = new List<StatusPrikazModel>();
                List<string> servisneGreske = new List<string>();

                foreach (ClanStatusPregledaDTO stavka in lista)
                {
                    string kategorija;

                    try
                    {
                        kategorija = kategorisanje.DajKategoriju(stavka.DatumRodjenja);
                    }
                    catch (Exception ex)
                    {
                        kategorija = "N/A";

                        if (!servisneGreske.Contains(ex.Message))
                        {
                            servisneGreske.Add(ex.Message);
                        }
                    }

                    prikaz.Add(new StatusPrikazModel
                    {
                        ClanID = stavka.ClanID,
                        JMBG = stavka.JMBG,
                        Ime = stavka.Ime,
                        Prezime = stavka.Prezime,
                        DatumRodjenja = stavka.DatumRodjenja.ToString("yyyy-MM-dd"),
                        Kategorija = kategorija,
                        DatumPregleda = stavka.DatumPregleda.HasValue
                            ? stavka.DatumPregleda.Value.ToString("yyyy-MM-dd")
                            : "",
                        StatusPregleda = stavka.StatusPregleda
                    });
                }

                gvStatusi.DataSource = prikaz;
                gvStatusi.DataBind();

                if (servisneGreske.Any())
                {
                    lblStatusPregled.Text = "Kategorije nisu ucitane iz servisa: " + servisneGreske.First();
                }
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska pri ucitavanju statusa: " + ex.Message;
            }
        }

        private void UcitajSvePreglede()
        {
            try
            {
                FormaZdravstveniPregledPregledKlasa forma =
                    new FormaZdravstveniPregledPregledKlasa(_stringKonekcije);
                ZdravstveniPregledListaKlasa lista = forma.DajSvePreglede();
                List<PregledPrikazModel> prikaz = new List<PregledPrikazModel>();
                FormaClanPregledKlasa formaClan = new FormaClanPregledKlasa(_stringKonekcije);

                foreach (ZdravstveniPregledKlasa pregled in lista.Lista)
                {
                    ClanKlasa clan = formaClan.DajClanaPoID(pregled.ClanID);

                    prikaz.Add(new PregledPrikazModel
                    {
                        ZdravstveniPregledID = pregled.ZdravstveniPregledID,
                        ClanID = pregled.ClanID,
                        Clan = clan != null ? clan.Prezime + " " + clan.Ime : "",
                        DatumPregleda = pregled.DatumPregleda.ToString("yyyy-MM-dd"),
                        Napomena = pregled.Napomena
                    });
                }

                gvSviPregledi.DataSource = prikaz;
                gvSviPregledi.DataBind();
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska pri ucitavanju pregleda: " + ex.Message;
            }
        }

        protected void btnPrikazi_Click(object sender, EventArgs e)
        {
            UcitajStatusTabelu();
        }

        protected void btnDodajPregled_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime datumPregleda;
                if (!DateTime.TryParse(txtDatumPregleda.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datumPregleda) &&
                    !DateTime.TryParseExact(txtDatumPregleda.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumPregleda))
                {
                    lblStatusPregled.Text = "Datum pregleda nije u ispravnom formatu.";
                    return;
                }

                FormaZdravstveniPregledUnosKlasa forma =
                    new FormaZdravstveniPregledUnosKlasa(_stringKonekcije);
                forma.ClanID = Convert.ToInt32(ddlClanovi.SelectedValue);
                forma.DatumPregleda = datumPregleda;
                forma.Napomena = txtNapomena.Text.Trim();

                if (forma.DodajPregled())
                {
                    lblStatusPregled.Text = "Pregled je uspesno dodat.";
                    OcistiPregledPolja();
                    UcitajStatusTabelu();
                    UcitajSvePreglede();
                }
                else
                {
                    lblStatusPregled.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Dodavanje pregleda nije uspelo."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnIzmeniPregled_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfZdravstveniPregledID.Value))
            {
                lblStatusPregled.Text = "Za izmenu pregleda potrebno je vezati konkretan pregled po ID-u.";
                return;
            }

            try
            {
                DateTime datumPregleda;
                if (!DateTime.TryParse(txtDatumPregleda.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out datumPregleda) &&
                    !DateTime.TryParseExact(txtDatumPregleda.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumPregleda))
                {
                    lblStatusPregled.Text = "Datum pregleda nije u ispravnom formatu.";
                    return;
                }

                FormaZdravstveniPregledUnosKlasa forma =
                    new FormaZdravstveniPregledUnosKlasa(_stringKonekcije);
                forma.ZdravstveniPregledID = Convert.ToInt32(hfZdravstveniPregledID.Value);
                forma.ClanID = Convert.ToInt32(ddlClanovi.SelectedValue);
                forma.DatumPregleda = datumPregleda;
                forma.Napomena = txtNapomena.Text.Trim();

                if (forma.IzmeniPregled())
                {
                    lblStatusPregled.Text = "Pregled je uspesno izmenjen.";
                    OcistiPregledPolja();
                    UcitajStatusTabelu();
                    UcitajSvePreglede();
                }
                else
                {
                    lblStatusPregled.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Izmena pregleda nije uspela."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnObrisiPregled_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfZdravstveniPregledID.Value))
            {
                lblStatusPregled.Text = "Za brisanje pregleda potrebno je vezati konkretan pregled po ID-u.";
                return;
            }

            try
            {
                FormaZdravstveniPregledUnosKlasa forma =
                    new FormaZdravstveniPregledUnosKlasa(_stringKonekcije);
                forma.ZdravstveniPregledID = Convert.ToInt32(hfZdravstveniPregledID.Value);

                if (forma.ObrisiPregled())
                {
                    lblStatusPregled.Text = "Pregled je uspesno obrisan.";
                    OcistiPregledPolja();
                    UcitajStatusTabelu();
                    UcitajSvePreglede();
                }
                else
                {
                    lblStatusPregled.Text = string.IsNullOrWhiteSpace(forma.PoslednjaGreska)
                        ? "Brisanje pregleda nije uspelo."
                        : HttpUtility.HtmlEncode(forma.PoslednjaGreska);
                }
            }
            catch (Exception ex)
            {
                lblStatusPregled.Text = "Greska: " + ex.Message;
            }
        }

        protected void btnOcistiPregled_Click(object sender, EventArgs e)
        {
            OcistiPregledPolja();
        }

        protected void gvSviPregledi_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfZdravstveniPregledID.Value = gvSviPregledi.SelectedDataKey.Value.ToString();
            ddlClanovi.SelectedValue = gvSviPregledi.SelectedRow.Cells[1].Text;
            txtDatumPregleda.Text = gvSviPregledi.SelectedRow.Cells[3].Text;
            string napomena = Server.HtmlDecode(gvSviPregledi.SelectedRow.Cells[4].Text);
            txtNapomena.Text = napomena == "\u00a0" ? "" : napomena;
            lblStatusPregled.Text = "Pregled je izabran za izmenu ili brisanje.";
        }

        protected void btnStampaSvihPregleda_Click(object sender, EventArgs e)
        {
            Response.Redirect("StampePregleda.aspx");
        }

        protected void btnStampaPregledaClana_Click(object sender, EventArgs e)
        {
            Response.Redirect("StampePregleda.aspx?clanID=" + ddlClanZaStampu.SelectedValue);
        }

        private void OcistiPregledPolja()
        {
            hfZdravstveniPregledID.Value = "";
            txtDatumPregleda.Text = "";
            txtNapomena.Text = "";
        }

        public class StatusPrikazModel
        {
            public int ClanID { get; set; }
            public string JMBG { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string DatumRodjenja { get; set; }
            public string Kategorija { get; set; }
            public string DatumPregleda { get; set; }
            public string StatusPregleda { get; set; }
        }

        public class PregledPrikazModel
        {
            public int ZdravstveniPregledID { get; set; }
            public int ClanID { get; set; }
            public string Clan { get; set; }
            public string DatumPregleda { get; set; }
            public string Napomena { get; set; }
        }
    }
}
