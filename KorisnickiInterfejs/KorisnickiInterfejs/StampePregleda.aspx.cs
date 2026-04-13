using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using KlasePodataka;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class StampePregleda : System.Web.UI.Page
    {
        private readonly string _stringKonekcije =
            ConfigurationManager.ConnectionStrings["VP2026KarateKlubZdravstveniPregledV1"]?.ConnectionString
            ?? ConfigurationManager.ConnectionStrings["NasaKonekcija"]?.ConnectionString
            ?? @"Data Source=.\SQLEXPRESS01;Initial Catalog=VP2026KarateKlubZdravstveniPreglediV1;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KorisnikID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblDatumGenerisanja.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                UcitajClanove();
                UcitajPrikazIzUpita();
            }
        }

        private void UcitajClanove()
        {
            try
            {
                FormaClanPregledKlasa forma = new FormaClanPregledKlasa(_stringKonekcije);
                ClanListaKlasa lista = forma.DajSveClanove();

                ddlClanovi.DataSource = lista.Lista;
                ddlClanovi.DataTextField = "Prezime";
                ddlClanovi.DataValueField = "ClanID";
                ddlClanovi.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska pri ucitavanju clanova: " + ex.Message;
            }
        }

        private void UcitajPrikazIzUpita()
        {
            string clanIDTekst = Request.QueryString["clanID"];

            if (string.IsNullOrWhiteSpace(clanIDTekst))
            {
                lblParametarPrikaza.Text = "Svi pregledi u sistemu";
                UcitajSvePreglede();
                return;
            }

            int clanID;
            if (!int.TryParse(clanIDTekst, out clanID))
            {
                lblStatus.Text = "Prosledjeni clan nije ispravan.";
                lblParametarPrikaza.Text = "Neispravan parametar";
                UcitajSvePreglede();
                return;
            }

            if (ddlClanovi.Items.FindByValue(clanID.ToString()) != null)
            {
                ddlClanovi.SelectedValue = clanID.ToString();
                lblParametarPrikaza.Text = "Clan ID " + clanID + " - " + ddlClanovi.SelectedItem.Text;
            }
            else
            {
                lblParametarPrikaza.Text = "Clan ID " + clanID;
            }

            UcitajPregledeClana(clanID);
        }

        private void UcitajSvePreglede()
        {
            try
            {
                FormaZdravstveniPregledPregledKlasa forma =
                    new FormaZdravstveniPregledPregledKlasa(_stringKonekcije);
                ZdravstveniPregledListaKlasa lista = forma.DajSvePreglede();

                gvStampaPregleda.DataSource = KreirajPrikazPregleda(lista);
                gvStampaPregleda.DataBind();
                lblNaslovPrikaza.Text = "Tabela svih pregleda";
                lblStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska pri ucitavanju pregleda: " + ex.Message;
            }
        }

        private void UcitajPregledeClana(int clanID)
        {
            try
            {
                FormaZdravstveniPregledPregledKlasa forma =
                    new FormaZdravstveniPregledPregledKlasa(_stringKonekcije);
                ZdravstveniPregledListaKlasa lista = forma.DajPregledeZaClana(clanID);

                gvStampaPregleda.DataSource = KreirajPrikazPregleda(lista);
                gvStampaPregleda.DataBind();
                lblNaslovPrikaza.Text = "Tabela pregleda izabranog clana";
                lblStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska pri ucitavanju pregleda clana: " + ex.Message;
            }
        }

        private List<PregledPrikazModel> KreirajPrikazPregleda(ZdravstveniPregledListaKlasa lista)
        {
            FormaClanPregledKlasa formaClan = new FormaClanPregledKlasa(_stringKonekcije);
            List<PregledPrikazModel> prikaz = new List<PregledPrikazModel>();

            foreach (ZdravstveniPregledKlasa pregled in lista.Lista)
            {
                ClanKlasa clan = formaClan.DajClanaPoID(pregled.ClanID);

                prikaz.Add(new PregledPrikazModel
                {
                    ZdravstveniPregledID = pregled.ZdravstveniPregledID,
                    ClanID = pregled.ClanID,
                    Clan = clan != null ? clan.Prezime + " " + clan.Ime : string.Empty,
                    DatumPregleda = pregled.DatumPregleda.ToString("yyyy-MM-dd"),
                    Napomena = pregled.Napomena
                });
            }

            return prikaz;
        }

        protected void btnPrikaziSve_Click(object sender, EventArgs e)
        {
            Response.Redirect("StampePregleda.aspx");
        }

        protected void btnFiltrirajClana_Click(object sender, EventArgs e)
        {
            Response.Redirect("StampePregleda.aspx?clanID=" + ddlClanovi.SelectedValue);
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
