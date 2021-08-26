using MeowMeowBeenz.Models;
using MeowMeowBeenz.Views.GebruikerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeowMeowBeenz.Views.BeoordelingsView
{
    /// <summary>
    /// Interaction logic for BeoordelingsView.xaml
    /// </summary>
    public partial class BeoordelingsView : Window
    {
        
        public BeoordelingsView(GebruikerModel gebruiker, GebruikerModel ingelogdegebruiker)
        {
            InitializeComponent();
            DataContext = new BeoordelingsViewModel(gebruiker, ingelogdegebruiker);
        }

    }
}
