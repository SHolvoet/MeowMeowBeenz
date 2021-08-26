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
using MeowMeowBeenz.Models;

namespace MeowMeowBeenz.Views.AanpassenView
{
    /// <summary>
    /// Interaction logic for AanpassenView.xaml
    /// </summary>
    public partial class AanpassenView : Window
    {
        private AanpassenViewModel viewModel; 
        public AanpassenView(GebruikerModel gebruiker)
        {
            InitializeComponent();
            viewModel = new AanpassenViewModel(gebruiker);
            DataContext = viewModel;
        }
        private void PasswordBox_OnPasswordChangedOld(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            var passwordBox = (PasswordBox)sender;

            // Password kenbaar maken in viewmodel om daar te kunnen gebruiken. idem bij volgende twee passwordmethodes
            viewModel.OldWachtwoord = passwordBox.Password;
        }

        private void PasswordBox_OnPasswordChangedNew(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            var passwordBox = (PasswordBox)sender;

            viewModel.NewWachtwoord = passwordBox.Password;
        }

        private void PasswordBox_OnPasswordChangedConfirmed(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            var passwordBox = (PasswordBox)sender;

            viewModel.ConfirmedWachtwoord = passwordBox.Password;
        }

    }
}

