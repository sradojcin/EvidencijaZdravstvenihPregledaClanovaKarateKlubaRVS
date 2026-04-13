using System;
using System.Configuration;
using System.Web;
using KlasePodataka;
using PrezentacionaLogika;

namespace KorisnickiInterfejs
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly string _stringKonekcije =
            ConfigurationManager.ConnectionStrings["VP2026KarateKlubZdravstveniPregledV1"]?.ConnectionString
            ?? ConfigurationManager.ConnectionStrings["NasaKonekcija"]?.ConnectionString
            ?? @"Data Source=.\SQLEXPRESS01;Initial Catalog=VP2026KarateKlubZdravstveniPreglediV1;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPrijava_Click(object sender, EventArgs e)
        {
            try
            {
                FormaLoginKlasa formaLogin = new FormaLoginKlasa(_stringKonekcije);
                formaLogin.KorisnickoIme = txtKorisnickoIme.Text.Trim();
                formaLogin.Lozinka = txtLozinka.Text.Trim();

                KorisnikKlasa korisnik = formaLogin.Prijava();

                if (korisnik == null)
                {
                    lblStatus.Text = string.IsNullOrWhiteSpace(formaLogin.PoslednjaGreska)
                        ? "Pogresno korisnicko ime ili lozinka."
                        : HttpUtility.HtmlEncode(formaLogin.PoslednjaGreska);
                    return;
                }

                Session["KorisnikID"] = korisnik.KorisnikID;
                Session["KorisnickoIme"] = korisnik.KorisnickoIme;
                Session["ImePrezime"] = korisnik.Ime + " " + korisnik.Prezime;

                Response.Redirect("Pocetna.aspx");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Greska: " + ex.Message;
            }
        }
    }
}
