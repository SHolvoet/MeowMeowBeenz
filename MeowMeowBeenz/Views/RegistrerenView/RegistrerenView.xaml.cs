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

namespace MeowMeowBeenz.Views.RegistrerenView
{
    /// <summary>
    /// Interaction logic for RegisterenView.xaml
    /// </summary>
    public partial class RegistrerenView : Window
    {

        private RegistrerenViewModel viewModel = new RegistrerenViewModel();
        public RegistrerenView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                return;
            }

            var passwordBox = (PasswordBox)sender;

            // Password kenbaar maken in viewmodel om daar te kunnen gebruiken. Idem bij volgende password methode
            viewModel.Wachtwoord = passwordBox.Password;
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
