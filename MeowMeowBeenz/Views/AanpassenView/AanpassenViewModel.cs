using MeowMeowBeenz.Core;
using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MeowMeowBeenz.Views.AanpassenView
{
    class AanpassenViewModel : ObservableObject
    {
        //props en variabelen
        private GebruikerModel _geselecteerdeGebruiker;

        public GebruikerModel GeselecteerdeGebruiker
        {
            get => _geselecteerdeGebruiker;
            set
            {
                _geselecteerdeGebruiker = value;
                OnPropertyChanged();
            }
        }

        private string _oldwachtwoord;
        public string OldWachtwoord
        {
            private get => _oldwachtwoord;
            set
            {
                _oldwachtwoord = value;
                OnPropertyChanged();
            }
        }

        private string _newwachtwoord;
        public string NewWachtwoord
        {
            private get => _newwachtwoord;
            set
            {
                _newwachtwoord = value;
                OnPropertyChanged();
            }
        }

        private string _confirmedwachtwoord;
        public string ConfirmedWachtwoord
        {
            private get => _confirmedwachtwoord;
            set
            {
                _confirmedwachtwoord = value;
                OnPropertyChanged();
            }
        }

        // de gegevens die worden ingetypt worden eerst in een andere tijdelijke gebruiker opgeslaan
        // zo kunnen we eerst controleren of alle gegevens juist zijn voor de die opslaan in de gebruiker zelf
        // anders kunnen we gegevens veranderen of verliezen zonder het te willen
        private GebruikerModel _tempGebruiker;

        public GebruikerModel TempGebruiker
        {
            get { return _tempGebruiker; }
            set
            {
                _tempGebruiker = value;
                OnPropertyChanged();
            }
        }

        private string _foutBoodschap;

        public string FoutBoodschap
        {
            get => _foutBoodschap;
            set
            {
                _foutBoodschap = value;
                OnPropertyChanged();
            }
        }

        private List<GebruikerModel> _gebruikers;

        public List<GebruikerModel> Gebruikers
        {
            get { return _gebruikers; }
            private set
            {
                _gebruikers = value;
                OnPropertyChanged();
            }
        }

        // constructor

        // bij het aanmaken van een instantie van deze klasse maken we de tempgebruiker aan, die nu nog leeg is
        // de gebruiker waarvan we de gegevens willen zien, degene die we in de parameter hebben meegegeven
        // slaan we hier op in geselecteerdegebruiker
        public AanpassenViewModel(GebruikerModel gebruiker)
        {
            GeselecteerdeGebruiker = gebruiker;
            TempGebruiker = new GebruikerModel();
        }

        // commands en methodes
        public DelegateCommand<Window> AanpassenCommand
        {
            get => new DelegateCommand<Window>(GegevensAanpassen);
        }

        private void GegevensAanpassen(Window window)
        {
            FoutBoodschap = "";

            DialogResult dg = MessageBox.Show("Bevestigen?", "Gegevens Aanpassen", MessageBoxButtons.YesNo);
            if (dg == DialogResult.No)
            {
                MessageBox.Show("De gegevens zijn onveranderd gebleven", "Gegevens aanpassen");
                return;
            }

            // hier wordt gecontroleerd of de ingevulde waardes correct zijn en in de lege waardes wordt de bestaande waarde gebruiker
            // als alle input ok is worden de gegevens van de gebruiker in de db aangepast
            if (InputOk(TempGebruiker))
            {
                using (var db = new MeowMeowBeenzDbEntities())
                {
                    var dbGebruiker = db.Gebruiker.First(gebruiker => gebruiker.Id == GeselecteerdeGebruiker.Id);

                    dbGebruiker.Gebruikersnaam = TempGebruiker.Gebruikersnaam;
                    dbGebruiker.Voornaam = TempGebruiker.Voornaam;
                    dbGebruiker.Familienaam = TempGebruiker.Familienaam;
                    dbGebruiker.Geslacht = TempGebruiker.Geslacht;
                    dbGebruiker.Emailadres = TempGebruiker.Emailadres;
                    dbGebruiker.Wachtwoord = TempGebruiker.Wachtwoord;

                    db.SaveChanges();
                }
            }
            else 
            {
                TempGebruiker = new GebruikerModel();
                return;  
            }

            // daarna de gegevens van de tempgebruiker leeggemaakt zodat de textboxen leeg zijn
            // wanneer de persoon naar het aanpasscherm kijkt
            GeselecteerdeGebruiker = TempGebruiker;
            TempGebruiker = new GebruikerModel();

            // hier moeten we onpropertychanged terug oproepen met de geselecteerde gebruiker als parameter
            // zo worden de gegevens die de persoon heeft veranderd getoond op het scherm
            OnPropertyChanged("GeselecteerdeGebruiker");
            MessageBox.Show("De gegevens zijn succesvol aangepast!", "Gegevens aanpassen");
        }

        public DelegateCommand<Window> AnnulerenCommand
        {
            get => new DelegateCommand<Window>(Annuleren);
        }

        public void Annuleren(Window window)
        {
            GeselecteerdeGebruiker = null;
            TempGebruiker = null;
            window.Close();
        }

        // hier wordt gecontroleerd welke veranderingen moeten worden gemaakt in de gebruiker
        private bool InputOk(GebruikerModel temp)
        {
            // als de gebruiker op de aanpasknop heeft gedrukt wordt eerst gecontroleerd of het bestaande wachtwoord is ingegeven
            // als dit niet het geval is kunnen de gegevens niet worden aangepast
            if (OldWachtwoord != GeselecteerdeGebruiker.Wachtwoord)
            {
                FoutBoodschap = "Het bestaande wachtwoord moet correct zijn\nvoor gegevens kunnen worden aangepast";
                return false;
            }
            // kijken of de gebruikersnaam moet veranderd worden, en indien ja of die uniek is
            // als die al bestaat krijgt de gebruiker een foutboodschap en breken we uit deze methode met return;
            // zo worden ook de andere gegevens niet aangepast
            if (!string.IsNullOrWhiteSpace(temp.Gebruikersnaam))
            {
                if (temp.GebruikersnaamUniek() == false)
                {
                    FoutBoodschap = "Deze gebruikersnaam bestaat al";
                    return false;
                }
            }
            else
            {
                temp.Gebruikersnaam = GeselecteerdeGebruiker.Gebruikersnaam;
            }
          
            if (!string.IsNullOrWhiteSpace(temp.Emailadres))
            {
                if (!temp.GeldigEmailadres())
                {
                    FoutBoodschap = "Er is geen geldig e-mail adres ingegeven";
                    return false;
                }
                if (!temp.EmailUniek())
                {
                    FoutBoodschap = "Het ingegeven e-mail adres bestaat al";
                    return false;
                }
            }
            else
            {
                temp.Emailadres = GeselecteerdeGebruiker.Emailadres;
            }

            // kijken of het nieuwe wachtwoord en controlewachtwoord zijn ingevuld
            // als dat zo is wordt gecontroleerd of beide velden gelijk zijn voor het wordt aangepast
            if (!string.IsNullOrWhiteSpace(NewWachtwoord))
            {
                if (NewWachtwoord != ConfirmedWachtwoord)
                {
                    FoutBoodschap = "Beide wachtwoordvelden moeten gelijk\n zijn voor het kan worden aangepast";
                    return false;
                }
                if (NewWachtwoord.Length < 7)
                {
                    FoutBoodschap = "Het nieuw wachtwoord moet\n minstens 7 tekens bevatten";
                    return false;
                }
                temp.Wachtwoord = NewWachtwoord;         
            }
            else
            {
                temp.Wachtwoord = GeselecteerdeGebruiker.Wachtwoord;
            }

            // wanneer de gebruikersnaam, het wachtwoord en het e-mail adres geen problemen hebben opgeleverd
            //is het zeker dat de gegevens kunnen worden veranderd
            if (string.IsNullOrWhiteSpace(temp.Voornaam))
            {
                temp.Voornaam = GeselecteerdeGebruiker.Voornaam;
            }
            if (string.IsNullOrWhiteSpace(temp.Familienaam))
            {
                temp.Familienaam = GeselecteerdeGebruiker.Familienaam;
            }
            if (string.IsNullOrWhiteSpace(temp.Geslacht))
            {
                temp.Geslacht = GeselecteerdeGebruiker.Geslacht;
            }

            if (temp.NamenOk() == false)
            {
                FoutBoodschap = "De voornaam en familienaam mogen enkel letters of een koppelteken bevatten";
                return false;
            }
            TempGebruiker.Id = GeselecteerdeGebruiker.Id;
            return true;
        }
    }
}
