//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeowMeowBeenz.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class GebruikerGroep
    {
        public int Id { get; set; }
        public int GebruikerId { get; set; }
        public int GroepId { get; set; }
    
        public virtual Gebruiker Gebruiker { get; set; }
        public virtual Groep Groep { get; set; }
    }
}
