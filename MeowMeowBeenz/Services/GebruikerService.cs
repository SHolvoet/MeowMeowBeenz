using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMeowBeenz.Services
{
    public static class GebruikerService
    {
        public static double GebruikersPunten(GebruikerModel gebruiker)
        {
            int aantal = 0;
            double punten = 0;
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var puntenVanGebruikers = db.Database.SqlQuery<GebruikerPunten>($"SELECT * FROM GebruikerPunten " +
                                                 $"WHERE GebruikerPunten.OntvangerId = {gebruiker.Id}").ToList();
                if (puntenVanGebruikers == null)
                {
                    return 0;
                }
                foreach (var punt in puntenVanGebruikers)
                {
                    aantal++;
                    punten += (double)punt.Punten;
                }
            }
            double resultaat = punten / aantal > 0 ? punten / aantal : 0;
            return resultaat; 
        }

        public static List<GebruikerModel> HaalGebruikersOp()
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var Leden = new List<GebruikerModel>();
                var dbGebruikers = db.Gebruiker.ToList();

                if (dbGebruikers == null)
                {
                    return null;
                }
                foreach (var lid in dbGebruikers)
                {
                    var gebruiker = new GebruikerModel(lid.Id, lid.Gebruikersnaam, lid.Voornaam, lid.Familienaam, lid.Emailadres, lid.Geslacht, lid.Wachtwoord);
                    Leden.Add(gebruiker);
                }
                return Leden;
            }
        }

        public static string ProfielfotoPad(GebruikerModel gebruiker)
        {
            var pad = "";

            using (var db = new MeowMeowBeenzDbEntities())
            {
                var Gebruikers = db.Gebruiker.ToList();
                foreach (var gebr in Gebruikers)
                {
                    if(gebruiker.Id == gebr.Id)
                    {
                        pad = gebr.Profielfoto;
                    }
                }
            }
            return pad;
        }

        public static GebruikerModel RefreshGebruikerGegevens(GebruikerModel oudeGebruiker)
        {
            GebruikerModel gerefreshedeGebruiker;
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var dbGebruiker = db.Gebruiker.First(gebruiker => gebruiker.Id == oudeGebruiker.Id);
                gerefreshedeGebruiker = new GebruikerModel(dbGebruiker.Id, dbGebruiker.Gebruikersnaam, dbGebruiker.Voornaam, dbGebruiker.Familienaam,
                    dbGebruiker.Emailadres, dbGebruiker.Geslacht, dbGebruiker.Wachtwoord);              
            }
            return gerefreshedeGebruiker;
        }
    }
}
