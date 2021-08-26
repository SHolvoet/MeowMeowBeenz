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
using System.Windows.Media;

namespace MeowMeowBeenz.Views.GroepView
{
    class GroepViewModel : ObservableObject
    {
        // props en variabelen
        private GroepModel _groep;
        public GroepModel Groep
        {
            get { return _groep; }
            set
            {
                _groep = value;
                OnPropertyChanged();
            }
        }

        private List<GebruikerModel> _leden;

        public List<GebruikerModel> Leden
        {
            get { return _leden; }
            set
            {
                _leden = value;
                OnPropertyChanged();
            }
        }

        private GebruikerModel _geselecteerdeGebruiker;

        public GebruikerModel GeselecteerdeGebruiker
        {
            get { return _geselecteerdeGebruiker; }
            set
            {
                _geselecteerdeGebruiker = value;
                OnPropertyChanged();
            }
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

        //constructor
        public GroepViewModel(GroepModel groep, GebruikerModel gebruiker)
        {
            Groep = groep;
            Leden = GroepService.HaalLedenOp(Groep);
            IngelogdeGebruiker = gebruiker;
        }

        //commands en methodes
        public DelegateCommand<Window> AnnulerenCommand
        {
            get => new DelegateCommand<Window>(Annuleer);
        }

        private void Annuleer(Window window)
        {
            window.Close();
        }

        public DelegateCommand BekijkGebruikerCommand
        {
            get => new DelegateCommand(BekijkGebruiker);
        }

        private void BekijkGebruiker()
        {
            var gbView = new BeoordelingsView.BeoordelingsView(GeselecteerdeGebruiker, IngelogdeGebruiker);
            gbView.ShowDialog();
            Leden = GroepService.HaalLedenOp(Groep);
            Groep.Punten = GroepService.GroepsPunten(GroepService.HaalLedenOp(Groep));
            OnPropertyChanged("Groep");
        }
    }
}
