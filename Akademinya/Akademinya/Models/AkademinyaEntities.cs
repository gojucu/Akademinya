namespace Akademinya.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AkademinyaEntities : DbContext
    {
        public AkademinyaEntities()
            : base("name=AkademinyaEntities13")
        {
        }

        public virtual DbSet<AlisverisSepeti> AlisverisSepeti { get; set; }
        public virtual DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public virtual DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public virtual DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public virtual DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public virtual DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public virtual DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public virtual DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public virtual DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public virtual DbSet<aspnet_Users> aspnet_Users { get; set; }
        public virtual DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public virtual DbSet<Islemler> Islemler { get; set; }
        public virtual DbSet<Kartlar> Kartlar { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kurs> Kurs { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Membership)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Paths)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Roles)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Applications>()
                .HasMany(e => e.aspnet_Users)
                .WithRequired(e => e.aspnet_Applications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<aspnet_Membership>()
                .HasOptional(e => e.Uye)
                .WithRequired(e => e.aspnet_Membership);

            modelBuilder.Entity<aspnet_Paths>()
                .HasOptional(e => e.aspnet_PersonalizationAllUsers)
                .WithRequired(e => e.aspnet_Paths);

            modelBuilder.Entity<aspnet_Roles>()
                .HasMany(e => e.aspnet_Users)
                .WithMany(e => e.aspnet_Roles)
                .Map(m => m.ToTable("aspnet_UsersInRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<aspnet_Users>()
                .HasOptional(e => e.aspnet_Membership)
                .WithRequired(e => e.aspnet_Users);

            modelBuilder.Entity<aspnet_Users>()
                .HasOptional(e => e.aspnet_Profile)
                .WithRequired(e => e.aspnet_Users);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventSequence)
                .HasPrecision(19, 0);

            modelBuilder.Entity<aspnet_WebEvent_Events>()
                .Property(e => e.EventOccurrence)
                .HasPrecision(19, 0);

            modelBuilder.Entity<Kartlar>()
                .Property(e => e.Cvc)
                .IsFixedLength();

            modelBuilder.Entity<Kurs>()
                .HasMany(e => e.Islemler)
                .WithRequired(e => e.Kurs)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kurs>()
                .HasMany(e => e.Uye1)
                .WithMany(e => e.Kurs1)
                .Map(m => m.ToTable("UyeKurs").MapLeftKey("KursID").MapRightKey("UyeID"));

            modelBuilder.Entity<Uye>()
                .HasMany(e => e.Islemler)
                .WithRequired(e => e.Uye)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uye>()
                .HasMany(e => e.Kartlar)
                .WithRequired(e => e.Uye)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uye>()
                .HasMany(e => e.Kurs)
                .WithOptional(e => e.Uye)
                .HasForeignKey(e => e.KursSahibiID);
        }
    }
}
