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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MeowMeowBeenzDbEntities : DbContext
    {
        public MeowMeowBeenzDbEntities()
            : base("name=MeowMeowBeenzDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Gebruiker> Gebruiker { get; set; }
        public virtual DbSet<GebruikerGroep> GebruikerGroep { get; set; }
        public virtual DbSet<GebruikerPunten> GebruikerPunten { get; set; }
        public virtual DbSet<Groep> Groep { get; set; }
    }
}
