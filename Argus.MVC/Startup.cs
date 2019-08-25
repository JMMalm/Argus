using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Argus.Infrastructure.Repositories;
using Argus.Core.Application;
using Argus.Core.Data;
using Argus.Core.Issue;

namespace Argus.MVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddScoped<IGenericRepository<Application>, GenericRepository<Application>>();
			services.AddScoped<IGenericRepository<Issue>, GenericRepository<Issue>>();

			services.AddScoped<IApplicationService, ApplicationService>();
			services.AddScoped<IIssueService, IssueService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			/* ASPNETCORE_ENVIRONMENT is read during startup and stored in IHostingEnvironment.EnvironmentName.
			 * Any value can be used but 3 are supported by default: Development, Staging, and Production. Default is Production.
			 */
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseMiddleware<Middleware.ErrorLogging>(Configuration);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				routes.MapRoute(
					name: "NotFound",
					template: "{*url}",
					defaults: new { controller = "Home", action = "NotFound" });
			});
		}
	}
}
