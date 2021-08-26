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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MeowMeowBeenz.Models;

namespace MeowMeowBeenz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new MainWindowViewModel();
        public MainWindow()
            
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

            // Password kenbaar maken in viewmodel om daar te kunnen gebruiken.
            viewModel.Wachtwoord = passwordBox.Password;
            
        }
        
    }
}