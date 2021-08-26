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
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using MeowMeowBeenz.Services;

namespace MeowMeowBeenz.Views.GebruikerView
{
    class GebruikerViewModel : ObservableObject
    {
        // props
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


        // constructor
        public GebruikerViewModel(GebruikerModel gebruiker)
        {
            GeselecteerdeGebruiker  =   gebruiker;
        }

        // commands en methodes
        public DelegateCommand BeoordelenCommand
        {
            get => new DelegateCommand(Beoordelen);
        }

        private void Beoordelen()
        {
            var gebruikersLijst = new GebruikersLijstView.GebruikersLijstView(GeselecteerdeGebruiker);
            gebruikersLijst.ShowDialog();
        }

        public DelegateCommand<GroepModel> GroepenCommand
        {
            get => new DelegateCommand<GroepModel>(BekijkGroepen);
        }

        private void BekijkGroepen(GroepModel groep)
        {
            var groepenLijstView = new GroepenLijstView.GroepenLijstView(GeselecteerdeGebruiker);
            groepenLijstView.ShowDialog();
        }

        public DelegateCommand InstellingenCommand
        {
            get => new DelegateCommand(GebruikerInstellingen);
        }

        private void GebruikerInstellingen()
        {
            var aanpassenView = new AanpassenView.AanpassenView(GeselecteerdeGebruiker);
            aanpassenView.ShowDialog();
            GeselecteerdeGebruiker = GebruikerService.RefreshGebruikerGegevens(GeselecteerdeGebruiker);
        }

        public DelegateCommand AboutCommand
        {
            get => new DelegateCommand(AboutGebruiker);
        }

        private void AboutGebruiker()
        {
            var aboutView = new AboutView.AboutView();
            aboutView.ShowDialog();
        }

        public DelegateCommand<Window> LogoutCommand
        {
            get => new DelegateCommand<Window>(Logout);
        }
        private void Logout(Window window)
        {
            window.Close();
        }

        public DelegateCommand KiesFotoCommand
        {
            get => new DelegateCommand(KiesFoto);
        }
        private void KiesFoto()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Kies een profielfoto";
            ofd.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            DialogResult result = ofd.ShowDialog();
            if (DlgResult(result) == true)
            {
                string path = Environment.CurrentDirectory;
                var fileName = ofd.SafeFileName;
                string dbImagePathName = $"/bin/Debug/Photos/{fileName}";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.Copy(ofd.FileName, $"{path}\\Photos\\{fileName}", true);
                
                UploadFotoPath(dbImagePathName);
                GeselecteerdeGebruiker = GebruikerService.RefreshGebruikerGegevens(GeselecteerdeGebruiker);
            }
        }

        public DelegateCommand<Window> QuitCommand
        {
            get => new DelegateCommand<Window>(Quit);
        }
        private void Quit(Window window)
        {
            window.Close();
        }

        private void UploadFotoPath(string path)
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var dbGebruiker = db.Gebruiker.First(gebruiker => gebruiker.Id == GeselecteerdeGebruiker.Id);

                dbGebruiker.Profielfoto = path;
                db.SaveChanges();
            }
        }

        private bool? DlgResult(DialogResult result)
        {
            switch (result)
            {
                case DialogResult.OK:
                case DialogResult.Yes:
                    return true;
                case DialogResult.None:
                case DialogResult.Cancel:
                    return false;
                case DialogResult.No:
                case DialogResult.Abort:
                    return null;
                default:
                    throw new ApplicationException("Unexpected dialog result.");
            }
        }
    }
}
