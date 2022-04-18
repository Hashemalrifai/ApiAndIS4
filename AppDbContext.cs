using System;
using Microsoft.EntityFrameworkCore;

namespace ApiAndIS4
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<WeatherForecast> WeatherForecasts { get; set; }
	}
}

