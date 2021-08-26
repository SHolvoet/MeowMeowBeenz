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



namespace MeowMeowBeenz.Views.GroepView
{
    /// <summary>
    /// Interaction logic for GroepView.xaml
    /// </summary>
    public partial class GroepView : Window
    {
        private GroepViewModel viewModel;

        public GroepView(GroepModel Groep, GebruikerModel gebruiker)
        {
            InitializeComponent();
            viewModel = new GroepViewModel(Groep, gebruiker);
            DataContext = viewModel;
        }
    }
}
