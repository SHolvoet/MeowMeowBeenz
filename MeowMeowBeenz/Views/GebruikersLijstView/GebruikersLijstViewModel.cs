using MeowMeowBeenz.Core;
using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using MeowMeowBeenz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace MeowMeowBeenz.Views.GebruikersLijstView
{
    class GebruikersLijstViewModel : ObservableObject
    {
        // props en variabelen
        private List<GebruikerModel> _leden;

        public List<GebruikerModel> Leden
        {
            get { return _leden; }
            private set
            {
                _leden = value;
                OnPropertyChanged();
            }
        }

        private GebruikerModel _geselecteerdeGebruiker;

        public GebruikerModel GeselecteerdeGebruiker
        {
            get { return _geselecteerdeGebruiker; }
            set { _geselecteerdeGebruiker = value; }
        }

        private GebruikerModel _ingelogdeGebruiker;
        public GebruikerModel IngelogdeGebruiker
        {
            get { return _ingelogdeGebruiker; }
            set
            {
                _ingelogdeGebruiker = value;
                OnPropertyChanged();
            }
        }

        // constructor
        public GebruikersLijstViewModel(GebruikerModel gebruiker)
        {
            Leden = new List<GebruikerModel>();
            Leden = GebruikerService.HaalGebruikersOp();
            IngelogdeGebruiker = gebruiker;

        }

        // commands en methodes
        public DelegateCommand BekijkGebruikerCommand
        {
            get => new DelegateCommand(BekijkGebruiker);
        }

        private void BekijkGebruiker()
        {
            var gbWindow = new BeoordelingsView.BeoordelingsView(GeselecteerdeGebruiker, IngelogdeGebruiker);
            gbWindow.ShowDialog();
            Leden = GebruikerService.HaalGebruikersOp();
        }

        public DelegateCommand<Window> AnnuleerCommand
{
            get => new DelegateCommand<Window>(Annuleren);
        }

        private void Annuleren(Window window)
        {
            window.Close();
        }
    }
}
