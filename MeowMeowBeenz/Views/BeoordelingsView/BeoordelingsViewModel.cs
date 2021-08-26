using MeowMeowBeenz.Core;
using MeowMeowBeenz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MeowMeowBeenz.Views.AanpassenView;
using MeowMeowBeenz.Database;
using System.Collections;

namespace MeowMeowBeenz.Views.BeoordelingsView
{
    class BeoordelingsViewModel : ObservableObject
    {

        //Props en variabelen
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

        private string _opmerking;

        public string Opmerking
        {
            get { return _opmerking; }
            set
            {
                _opmerking = value;
                OnPropertyChanged();
            }
        }

        private int _score;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        private int _reedsontvangescore;

        public int Reedsontvangescore
        {
            get { return _reedsontvangescore; }
            set
            {
                _reedsontvangescore = value;
                OnPropertyChanged();
            }
        }

        //Constructor
        public BeoordelingsViewModel(GebruikerModel geselecteerdeGebruiker, GebruikerModel ingelogdegebruiker)
        {
            GeselecteerdeGebruiker = geselecteerdeGebruiker;
            IngelogdeGebruiker = ingelogdegebruiker;

            ReedsPuntenGegeven(Reedsontvangescore);
            Score = Reedsontvangescore;
        }

        //Commands en Methodes
        public DelegateCommand<GebruikerModel> ConfirmCommand
        {
            get => new DelegateCommand<GebruikerModel>(Confirm);
        }

        private void Confirm(GebruikerModel gebruiker)
        {

            if (PuntenToekenenAanJezelf() == true)
            {
                Opmerking = "sneaky sneaky, geen punten aan jezelf geven";
                return;
            }
            else if (PuntenZijnHetZelfde() == true)
            {
                Opmerking = "Punten zijn het zelfde gebleven";
                return;
            }
            else if (ScoreIsnull() == true)
            {
                Opmerking = "Geen score gegeven";
                return;
            }
            else
            {
                //Toevoegen van gebruiker in een try catch geplaatst om excetion op tevangen wanneer combinatie geven en ontvanger reedsbestaad
                //doordat dit geconstrained is in database (UNIQUE NONCLUSTERED) zal hij een exeption opsmijten wanneer combinatie reeds bestaad
                //vervolgens kunnen we in de catch enkel de punten veranderen en een mededeling geven dat
                //punten zijn aangepast.
                try
                {
                    using (var db = new MeowMeowBeenzDbEntities())
                    {

                        var dbGebruikerpunten = db.GebruikerPunten.First(GebruikerPunten => GebruikerPunten.OntvangerId == GeselecteerdeGebruiker.Id && GebruikerPunten.GeverId == IngelogdeGebruiker.Id);
                        {
                            dbGebruikerpunten.Punten = Score;
                            Opmerking = "Punten aangepast";
                            db.SaveChanges();
                            
                        }
                    }
                }
                catch (Exception)
                {
                    using (var db = new MeowMeowBeenzDbEntities())
                    {

                        var dbGebruikerpunten = new GebruikerPunten();
                        {
                            dbGebruikerpunten.GeverId = IngelogdeGebruiker.Id;
                            dbGebruikerpunten.OntvangerId = GeselecteerdeGebruiker.Id;
                            dbGebruikerpunten.Punten = Score;

                            db.GebruikerPunten.Add(dbGebruikerpunten);
                            Opmerking = "Punten gegeven";
                            db.SaveChanges();
                        }
                    }    
                }
            }
        }

        public DelegateCommand <string> ScoreCommand
        {
            get { return new DelegateCommand<string>(Scoreweergeven); }
        }

        //In de xaml gebruik gemaakt van commandparameter om op aparte waarde van elke figuur door te geven 
        //Hierdoor kan er met 1 methode het onderscheid gemaakt worden van welke punten er moeten worden toegekent aan de score.
        private void Scoreweergeven(string parameter)
        {
            int value = int.Parse(parameter);

            switch (value)
            {
                case 1:
                {
                 Score = 1;
                 break;
                }
                case 2:
                {
                 Score = 2;
                 break;
                }
                case 3:
                { 
                 Score = 3;
                 break;
                }
                case 4:
                {
                  Score = 4;
                  break;
                }
                case 5:
                {
                 Score = 5;
                 break;
                }
            }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get => new DelegateCommand<Window>(Cancel);
        }

        private void Cancel(Window window)
        {
            // hier wordt het venster afgesloten zonder een nieuwe gebruiker aan te maken
            // de tijdelijke gebruiker in deze klassen wordt leegemaakt omdat deze gegevens niet meer moeten worden bijgehouden
            // want de klant maakt er toch geen nieuwe gebruiker mee
            window.Close();
        }
        public bool PuntenToekenenAanJezelf()
        {
            if (GeselecteerdeGebruiker.Id == IngelogdeGebruiker.Id)
            {
                return true;
            }
            else
                return false;
        }



        public bool PuntenZijnHetZelfde()
        {

            using (var db = new MeowMeowBeenzDbEntities())
            {
                try
                {
                    var dbGebruikerpunten = db.GebruikerPunten.First(GebruikerPunten => GebruikerPunten.OntvangerId == GeselecteerdeGebruiker.Id && GebruikerPunten.GeverId == IngelogdeGebruiker.Id);
                    {
                        if (Score == (int)dbGebruikerpunten.Punten)
                        {
                            return true;
                        }
                        return false;

                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }
        private int ReedsPuntenGegeven(int Score)
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                try
                {
                    var dbGebruikerpunten = db.GebruikerPunten.First(GebruikerPunten => GebruikerPunten.OntvangerId == GeselecteerdeGebruiker.Id && GebruikerPunten.GeverId == IngelogdeGebruiker.Id);
                    {
                        Reedsontvangescore = (int)dbGebruikerpunten.Punten;
                        return Reedsontvangescore;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public bool ScoreIsnull()
        {
            if (Score == 0)
            {
                return true;
            }
            return false;
        }
    }
}
