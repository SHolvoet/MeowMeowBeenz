using MeowMeowBeenz.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMeowBeenz.Models
{
    public class GroepModel
    {
        // props en variabelen
		public int Id { get; private set; }
		public string Groepsnaam { get; set; }
        public double Punten { get; set; }

        // constructor
        public GroepModel()
        {
            Punten = GroepService.GroepsPunten(GroepService.HaalLedenOp(this));
        }

        public GroepModel(int id, string naam)
        {
            Id = id;
            Groepsnaam = naam;
            Punten = GroepService.GroepsPunten(GroepService.HaalLedenOp(this));
        }
    }
}
