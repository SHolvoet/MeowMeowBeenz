using MeowMeowBeenz.Database;
using MeowMeowBeenz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMeowBeenz.Services
{
    static class GroepService
    {
        public static List<GebruikerModel> HaalLedenOp(GroepModel groep)
        {
            var Leden = new List<GebruikerModel>();
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var leden = db.Database.SqlQuery<Gebruiker>
                ($"SELECT * FROM Gebruiker AS gb " +
                $"INNER JOIN GebruikerGroep AS grgb ON gb.Id = grgb.GebruikerId " +
                $"INNER JOIN Groep AS gr ON grgb.GroepId = gr.Id WHERE gr.Id = {groep.Id} ORDER BY gb.Id").ToList();

                foreach (var lid in leden)
                {
                    var gebruiker = new GebruikerModel(lid.Id, lid.Gebruikersnaam, lid.Voornaam, lid.Familienaam, lid.Emailadres, lid.Geslacht, lid.Wachtwoord);
                    Leden.Add(gebruiker);
                }
            }
            return Leden;
        }

        public static List<GroepModel> HaalGroepenOp()
        {
            using (var db = new MeowMeowBeenzDbEntities())
            {
                var Groepen = new List<GroepModel>();
                var groepen = db.Groep.ToList();

                foreach (var groep in groepen)
                {
                    var Groep = new GroepModel(groep.Id, groep.Groepsnaam);
                    Groepen.Add(Groep);
                }
                return Groepen;
            }
        }

        public static double GroepsPunten(List<GebruikerModel> leden)
        {
            int aantal = 0;
            double punten = 0;
            using (var db = new MeowMeowBeenzDbEntities())
            {
                foreach (var lid in leden)
                {
                    aantal++;
                    punten += GebruikerService.GebruikersPunten(lid);
                }
            }
            return punten / aantal;
        }
    }
}
