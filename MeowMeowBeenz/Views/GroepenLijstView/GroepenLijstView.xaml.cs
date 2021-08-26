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
using MeowMeowBeenz.Core;
using MeowMeowBeenz.Models;

namespace MeowMeowBeenz.Views.GroepenLijstView
{
    /// <summary>
    /// Interaction logic for GroepenLijstView.xaml
    /// </summary>
    public partial class GroepenLijstView : Window
    {
        public GroepenLijstView(GebruikerModel gebruiker)
        {
            InitializeComponent();
            DataContext = new GroepenLijstViewModel(gebruiker);
        }
    }
}
