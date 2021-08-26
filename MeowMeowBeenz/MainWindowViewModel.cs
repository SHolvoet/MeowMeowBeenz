using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowMeowBeenz.Core;
using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using MeowMeowBeenz.Views.AanpassenView;
using MeowMeowBeenz.Views.RegistrerenView;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Controls;
using MeowMeowBeenz.Views.GebruikerView;
using MeowMeowBeenz.Views.GebruikersLijstView;
using System.Data.SqlClient;
using MeowMeowBeenz.Services;
using System.Windows;
using System.Windows.Forms;

namespace MeowMeowBeenz
{
	public class MainWindowViewModel : ObservableObject
	{
		//Props en variabelen
		private GebruikerModel _teControlerenGebruiker;

		public GebruikerModel TeControlerenGebruiker
		{
			get => _teControlerenGebruiker;
			set
			{
				_teControlerenGebruiker = value;
				OnPropertyChanged();
			}
		}

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
			get => _wachtwoord;
			set
			{
				_wachtwoord = value;
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

        //Constructor
        public MainWindowViewModel()
		{
			TeControlerenGebruiker = new GebruikerModel();
		}

		// Commands en methodes
		public DelegateCommand InloggenCommand
		{
			get { return new DelegateCommand(Inloggen); }
		}

		// hier worden de logingegevens gecontroleerd, er wordt gekeken of het emailedres bestaat
		// en, indien het bestaat, of het ingegeven wachtwoord bij dat account past om te kunnen inloggen
		private void Inloggen()
		{
			Gebruikers = GebruikerService.HaalGebruikersOp();
			if (string.IsNullOrWhiteSpace(TeControlerenGebruiker.Emailadres) && string.IsNullOrWhiteSpace(Wachtwoord))
			{
				FoutBoodschap = "Geen gegevens";
				return ;
			}

            if (Gebruikers.Count > 0)
            {
				foreach (var gebruiker in Gebruikers)
				{
					if (TeControlerenGebruiker.Emailadres == gebruiker.Emailadres && Wachtwoord == gebruiker.Wachtwoord)
					{
						// de foutboodschap wordt ook gewist, stel dat je de eerste keer hebt mistypt en er staat een foutboodschap
						// dan verdwijnt die wanneer je de juiste gegevens nu wel hebt ingegeven en inlogt
						FoutBoodschap = "";

						// hier wordt een nieuw venster geopend met de gegevens van de gebruiker
						// daarom heeft dat venster een parameter om te openen, de gebruiker die wordt bekeken
						var gebruikerView = new GebruikerView(gebruiker);
						TeControlerenGebruiker = new GebruikerModel();
						gebruikerView.ShowDialog();

						return;
					}
				}
			}
			FoutBoodschap = "Gebruikersnaam en/of wachtwoord zijn niet correct";
		}

		public DelegateCommand RegistrerenCommand
		{
			get { return new DelegateCommand(Registreren); }
		}

		// Hier wordt gewoon een nieuw venster aangemaakt waar een nieuwe gebruiker kan worden geregistreerd
		// de foutboodschap wordt ook gewist, stel dat je de eerste keer hebt mistypt en er staat een foutboodschap
		// dan verdwijnt die wanneer je een nieuwe gebruiker wil aanmaken
		private void Registreren()
		{
            var registrerenView = new RegistrerenView();
            FoutBoodschap = "";
            TeControlerenGebruiker = new GebruikerModel();
			registrerenView.ShowDialog();
        }

		public DelegateCommand<Window> QuitCommand
		{
			get => new DelegateCommand<Window>(Quit);
		}

		private void Quit(Window window)
		{
			System.Windows.Application.Current.Shutdown();
		}
	}
}
