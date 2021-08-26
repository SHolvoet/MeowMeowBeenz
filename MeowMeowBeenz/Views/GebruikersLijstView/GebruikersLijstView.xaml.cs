using MeowMeowBeenz.Models;
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

namespace MeowMeowBeenz.Views.GebruikersLijstView
{
    /// <summary>
    /// Interaction logic for GebruikersLijstView.xaml
    /// </summary>
    public partial class GebruikersLijstView : Window
    {
        public GebruikersLijstView(GebruikerModel gebruiker)
        {
            InitializeComponent();
            DataContext = new GebruikersLijstViewModel(gebruiker);
        }
    }
}
