using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Stockholm_Syndrome_Web.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Corp> Alliance { get; set; }
		public DbSet<EveCharacter> EveCharacters { get; set; }
		public DbSet<Ops> Ops { get; set; }
		public DbSet<OpsTag> Tags { get; set; }
		public DbSet<Structure> Structures { get; set; }
		public DbSet<ExtractionData> ExtractionData { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>()
				.Ignore(c => c.Email)
				.Ignore(c => c.EmailConfirmed)
				.Ignore(c => c.PhoneNumber)
				.Ignore(c => c.PhoneNumberConfirmed)
				.Ignore(c => c.TwoFactorEnabled)
				.HasMany(e => e.EveCharacter)
				.WithOne(f => f.User)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Ops>()
				.HasMany(t => t.OpTags)
				.WithOne(o => o.Ops)
				.OnDelete(DeleteBehavior.Cascade);
				
		}
	}
}
