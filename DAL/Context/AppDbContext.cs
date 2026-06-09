using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<TbCarrier> TbCarriers { get; set; }

        public virtual DbSet<TbCity> TbCities { get; set; }

        public virtual DbSet<TbCountry> TbCountries { get; set; }

        public virtual DbSet<TbPaymentMethod> TbPaymentMethods { get; set; }

        public virtual DbSet<TbSetting> TbSettings { get; set; }

        public virtual DbSet<TbShippingType> TbShippingTypes { get; set; }

        public virtual DbSet<TbShippment> TbShippments { get; set; }

        public virtual DbSet<TbShippmentStatus> TbShippmentStatuses { get; set; }

        public virtual DbSet<TbSubscriptionPackage> TbSubscriptionPackages { get; set; }

        public virtual DbSet<TbUserReceiver> TbUserReceivers { get; set; }

        public virtual DbSet<TbUserSender> TbUserSenders { get; set; }

        public virtual DbSet<TbUserSubscription> TbUserSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Very important to call the base method to ensure that the Identity model is configured correctly
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateTime>().HaveColumnType("datetime");
        }
    }
}
