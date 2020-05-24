using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Stockholm_Syndrome_Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication;
using Serilog;
using EVE.SingleSignOn.Core;
using Stockholm_Syndrome_Web.Models;

namespace Stockholm_Syndrome_Web
{
	public class Startup
	{
		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			_environment = environment;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			var keysFolder = Path.Combine(_environment.ContentRootPath, "Keys");
			services.AddDataProtection()
				// This helps surviving a server restart
				.PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
				// This helps surviving a site update: each app has its own store, building the site creates a new app
				.SetApplicationName("StockholmSyndrome")
				.SetDefaultKeyLifetime(TimeSpan.FromDays(90));

			services.AddHttpClient();
			services.AddSingleton<ISingleSignOnClient, SingleSignOnClient>();

			services.Configure<SSOUserLogin>(Configuration.GetSection("SSOUserLogin"));
			services.Configure<SSOCorp>(Configuration.GetSection("SSOCorp"));

			services.AddAuthentication()
				.AddDiscord(options =>
				{
					options.ClientId = "618390003940327424";
					options.ClientSecret = "FDey9Rm8mgX4IstO9FF5no9kfhVG3kSN";
					options.CallbackPath = "/Api/Discord";
					options.SaveTokens = true;
					options.Scope.Add("identify");
					options.Scope.Add("guilds.join");
					options.Events.OnRemoteFailure = (RemoteFailureContext context) =>
					{
						context.HandleResponse();
						context.Response.Redirect("/");
						return Task.CompletedTask;
					};
				});


			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddDefaultUI()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;
				// Username settings
				options.User.AllowedUserNameCharacters = string.Empty;
			});

			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}
}
