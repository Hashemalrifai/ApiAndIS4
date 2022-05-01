using System;
using System.Reflection;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiAndIS4
{
	public class AppDbContext : IdentityDbContext<ApplicationUser>, IPersistedGrantDbContext, IConfigurationDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

        #region IPersistedGrantDbContext

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<PersistedGrant> PersistedGrants { get ; set ; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get ; set ; }

        #endregion

        #region IConfigurationDbContext

        public DbSet<Client> Clients { get ; set ; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get ; set ; }
        public DbSet<IdentityResource> IdentityResources { get ; set ; }
        public DbSet<ApiResource> ApiResources { get ; set ; }
        public DbSet<ApiScope> ApiScopes { get ; set ; }

        #endregion


        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigurePersistentGrantDbContext(modelBuilder);
            ConfigureConfigurationDbContext(modelBuilder);

        }

        private void ConfigurePersistentGrantDbContext(ModelBuilder builder)
        {
            var options = new OperationalStoreOptions();
            SetSchemaForAllTables(options, "isgrants");

            builder.ConfigurePersistedGrantContext(options);
        }

        private void ConfigureConfigurationDbContext(ModelBuilder builder)
        {
            var options = new ConfigurationStoreOptions();
            SetSchemaForAllTables(options, "isconfig");

            builder.ConfigureClientContext(options);
            builder.ConfigureResourcesContext(options);
        }

        private void SetSchemaForAllTables<T>(T options, string schema)
        {
            var tableConfigurationType = typeof(TableConfiguration);
            var schemaProperty = tableConfigurationType.GetProperty(nameof(TableConfiguration.Schema));

            var tableConfigurations = options.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => tableConfigurationType.IsAssignableFrom(property.PropertyType))
                .Select(property => property.GetValue(options, null));

            foreach (var table in tableConfigurations)
                schemaProperty.SetValue(table, schema, null);
        }
    }
}

