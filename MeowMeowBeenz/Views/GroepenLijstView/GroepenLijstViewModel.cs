using MeowMeowBeenz.Core;
using MeowMeowBeenz.Models;
using MeowMeowBeenz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MeowMeowBeenz.Views.GroepenLijstView
{
    class GroepenLijstViewModel : ObservableObject
    {
        // props en variabelen
        private GroepModel _geselecteerdeGroep;

        public GroepModel GeselecteerdeGroep
        {
            get { return _geselecteerdeGroep; }
            set
            {
                _geselecteerdeGroep = value;
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

        private List<GroepModel> _groepen;

        public List<GroepModel> Groepen
        {
            get { return _groepen; }
            set
            {
                _groepen = value;
                OnPropertyChanged();
            }
        }

        // constructor
        public GroepenLijstViewModel(GebruikerModel gebruiker)
        {
            Groepen = GroepService.HaalGroepenOp();
            IngelogdeGebruiker = gebruiker;
            
        }

        // commands en methodes
        public DelegateCommand<Window> AnnuleerCommand
        {
            get => new DelegateCommand<Window>(Annuleren);
        }

        private void Annuleren(Window window)
        {
            window.Close();
        }

        public DelegateCommand BekijkGroepCommand
        {
            get => new DelegateCommand(BekijkGeselecteerdeGroep);
        }

        private void BekijkGeselecteerdeGroep()
        {
            var groepWindow = new GroepView.GroepView(GeselecteerdeGroep, IngelogdeGebruiker);
            groepWindow.ShowDialog();
            Groepen = GroepService.HaalGroepenOp();
        }
    }
}
