using MeowMeowBeenz.Core;
using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Data.SqlClient;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MeowMeowBeenz.Views.RegistrerenView
{
	class RegistrerenViewModel : ObservableObject
	{
		// props en variabelen
		private GebruikerModel _gebruiker;

		public GebruikerModel Gebruiker
		{
			get => _gebruiker;
			set
			{
				_gebruiker = value;
				OnPropertyChanged();
			}
		}

		// terug een foutboodschap
		private string _foutBoodschap;

		public string FoutBoodschap
		{
			get { return _foutBoodschap; }
			set
			{
				_foutBoodschap = value;
				OnPropertyChanged();
			}
		}

		private string _wachtwoord;

		public string Wachtwoord
		{
			private get { return _wachtwoord; }
			set
			{
				_wachtwoord = value;
				OnPropertyChanged();
			}
		}

		private string _comfirmedwachtwoord;
		public string ConfirmedWachtwoord
		{
			private get => _comfirmedwachtwoord;
			set
			{
				_comfirmedwachtwoord = value;
				OnPropertyChanged();
			}
		}

		// constructor
		public RegistrerenViewModel()
		{
			Gebruiker = new GebruikerModel();
		}

		// commands en methodes
		public DelegateCommand<Window> RegistrerenCommand
		{
			get => new DelegateCommand<Window>(Registreren);
		}

		// hier controleren we de ingegeven gegevens voor te registreren gebruiker
		// de nieuwe gebruiker kan alleen maar worden aangemaakt wanneer alle controles in orde zijn
		// dat wil zeggen, de gebruikersnaam moet uniek zijn, het emailadres moet geldig en uniek zijn
		// de voornaam en familienaam mogen enkel letters en een koppelteken bevatten (voor namen als Jean-Marie en zo)
		// het wachtwoord moet minstens 7 tekens bevatten en het controlewachtwoord en het wacthwoord moeten hetzelfde zijn
		// de controlemethodes die een bool retourneren bevinden zich in de gebruikerklasse zelf
		private void Registreren(Window window)
		{
			if (Gebruiker.GebruikersnaamUniek() == false)
			{
				FoutBoodschap = "Deze gebruikersnaam bestaat al, gelieve een andere te kiezen";
				return;
			}
			if (Gebruiker.GeldigEmailadres() == false)
			{
				FoutBoodschap = "Je account moet een geldig email adres bevatten";
				return;
			}
            if (Gebruiker.EmailUniek() == false)
            {
				FoutBoodschap = "Dit e-mail adres is al in gebruik, gelieve een ander te gebruiken";
				return;
            }
			if (Wachtwoord.Length < 7)
			{
				FoutBoodschap = "Het wachtwoord moet minstens 7 tekens bevatten";
				return;
			}
			if (Wachtwoord != ConfirmedWachtwoord)
			{
				FoutBoodschap = "Het wachtwoord moet twee keer hetzelfde zijn";
				return;
			}
			if (Gebruiker.NamenOk() == false)
			{
				FoutBoodschap = "De voornaam en familienaam mogen enkel\nuit letters (en koppeltekens) bestaan";
				return;
			}
			if (string.IsNullOrWhiteSpace(Gebruiker.Geslacht))
			{
				FoutBoodschap = "Je moet een geslacht kiezen";
				return;
			}

			//wanneer alle gegevens ok zijn wordt een bevestiging gevraagd of de persoon zeker is of hij/zij het account wil aanmaken
			DialogResult dg = MessageBox.Show("Bevestigen?", "Registreer gebruiker", MessageBoxButtons.YesNo);

			if (dg == DialogResult.No)
			{
				MessageBox.Show("De gebruiker is niet aangemaakt!", "Registreer gebruiker");
				return;
			}
			// gegevens gebruiker naar db sturen
			GebruikerToevoegen(Gebruiker);
			// de tijdelijke gebruiker wordt weer leegemaakt, want de gegevens zijn opgeslagen in de lijst met gebruikers
			Gebruiker = null;
			MessageBox.Show("Gegevens geregistreerd, welkom!", "Registreer gebruiker");
			window.Close();
		}

		public DelegateCommand<Window> AnnulerenCommand
		{
			get => new DelegateCommand<Window>(Annuleren);
		}

		private void Annuleren(Window window)
		{
			Gebruiker = null;
			window.Close();
		}

		private void GebruikerToevoegen(GebruikerModel gebruiker)
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var dbGebruiker = new Gebruiker()
                {
                    Gebruikersnaam = Gebruiker.Voornaam,
                    Voornaam = Gebruiker.Voornaam,
                    Familienaam = Gebruiker.Familienaam,
                    Geslacht = Gebruiker.Geslacht,
                    Emailadres = Gebruiker.Emailadres,
                    Wachtwoord = Wachtwoord
                };

                db.Gebruiker.Add(dbGebruiker);
                db.SaveChanges();
            }

		}
	}
}
