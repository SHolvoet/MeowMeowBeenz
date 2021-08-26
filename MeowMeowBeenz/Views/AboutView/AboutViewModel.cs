using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MeowMeowBeenz.Core;


namespace MeowMeowBeenz.Views.AboutView
{
    class AboutViewModel : ObservableObject
    {

        private string _theones;

        public string Theones
        {
            get { return _theones; }
            set
            {
                _theones = value;
                OnPropertyChanged();
            }
        }

        public AboutViewModel()
        {
            Theones = "1. Ones don't get a rhyme,\n because they're garbage";
        }
        
        //Fives have lives. Fours have chores. Threes have fleas. Twos have blues and Ones don't get a rhyme, because they're garbage.

        public DelegateCommand<Window> TerugCommand
        {
            get => new DelegateCommand<Window>(Terug);
        }

        private void Terug(Window window)
        {
            window.Close();
        }
    }
}
