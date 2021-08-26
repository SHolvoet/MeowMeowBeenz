using MeowMeowBeenz.Core;
using MeowMeowBeenz.Database;
using MeowMeowBeenz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MeowMeowBeenz.Models
{
    public class GebruikerModel : ObservableObject
    {
        // props en variabelen
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }
        public string Emailadres { get; set; }
        public string Geslacht { get; set; }
        public string Wachtwoord { get; set; }
        public double Punten { get; private set; }
        private string _profielfoto;

        public string Profielfoto
        {
            get { return _profielfoto; }
            set
            {
                _profielfoto = value;
                OnPropertyChanged();
            }
        }

        // constructors
        public GebruikerModel()
        {
            ZetWaardesUitDatabase();
        }

        public GebruikerModel(int id, string gbNaam, string vNaam, string fNaam, string email, string geslacht, string wachtwoord)
        {
            Id = id;
            Gebruikersnaam = gbNaam;
            Voornaam = vNaam;
            Familienaam = fNaam;
            Emailadres = email;
            Geslacht = geslacht;
            Wachtwoord = wachtwoord;
            ZetWaardesUitDatabase();
        }

        // methodes
        private void ZetWaardesUitDatabase()
        {
            Punten = GebruikerService.GebruikersPunten(this);
            Profielfoto = GebruikerService.ProfielfotoPad(this);
        }

        // deze methodes retourneren een bool om te controleren of de gegevens van de gebruiker juist zijn
        // als al deze true retourneren kan de nieuwe gebruiker worden aangemaakt of bestaande gebruiker worden aangepast
        public bool GeldigEmailadres()
        {
            if (string.IsNullOrWhiteSpace(Emailadres) || !Emailadres.Contains("@"))
            {
                return false;
            }

            var eindigtMetBeOfom = Emailadres.EndsWith(".be") || Emailadres.EndsWith(".com");
            if (eindigtMetBeOfom == false)
            {
                return false;
            }
            return true;
        }

        public bool EmailUniek()
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var ledenAdressen = db.Database.SqlQuery<Gebruiker>("SELECT * FROM Gebruiker").ToList();
                foreach (var gebruiker in ledenAdressen)
                {
                    if (Emailadres == gebruiker.Emailadres)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool GeldigWachtwoord()
        {
            if (string.IsNullOrWhiteSpace(Wachtwoord) || Wachtwoord.Length < 7)
            {
                return false;
            }
            return true;
        }

        public bool NamenOk()
        {
            if (string.IsNullOrWhiteSpace(Voornaam) || string.IsNullOrWhiteSpace(Familienaam))
            {
                return false;
            }
            // Met regex kijkt het programma of de input enkel uit de tekens tussen de haakjes [] bevat
            // de voornaam en familienaam kunnen enkel uit letters en koppeltekens bestaan
            if (!Regex.IsMatch(Voornaam, @"^[a-zA-Z-]+$") || !Regex.IsMatch(Familienaam, @"^[a-zA-Z-]+$"))
            {
                return false;
            }
            return true;
        }

        public bool GebruikersnaamUniek()
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var ledenNamen = db.Database.SqlQuery<Gebruiker>("SELECT * FROM Gebruiker").ToList();
                foreach (var gebruiker in ledenNamen)
                {
                    if (Gebruikersnaam == gebruiker.Gebruikersnaam)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
