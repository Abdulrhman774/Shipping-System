using DAL.Contracts;
using Domain.Entities;
using Domain.Entities.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.Context
{
    public class ShippingDbContext : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
        {
        }

        #region DbSet for entities

        public DbSet<TbRefreshToken> RefreshTokens { get; set; }
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
        #endregion


        #region DbSet for views
        public DbSet<VwCitiesCountries> VwCitiesCountries { get; set; }
        #endregion

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

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           return base.SaveChangesAsync(cancellationToken);
        }
        
    }
}
