using Argus.Core;
using Argus.Infrastructure.Repositories;
using Argus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Argus.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IConfiguration _config;
		private readonly IApplicationRepository _applicationRepo;
		private readonly IIssueRepository _issueRepository;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="applicationRepo"></param>
		/// <param name="issueRepository"></param>
		/// <remarks>
		///		The repositories are registered as services in Startup.cs, so that's where they come from.
		/// </remarks>
		public HomeController(IConfiguration config, IApplicationRepository applicationRepo, IIssueRepository issueRepository)
		{
			_config = config ??
				throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
			_applicationRepo = applicationRepo ??
				throw new ArgumentNullException(nameof(applicationRepo), "Repository cannot be null.");
			_issueRepository = issueRepository ??
				throw new ArgumentNullException(nameof(issueRepository), "Repository cannot be null.");
		}

		public IActionResult Index()
		{
			// Get the Font Awesome CDN key from secrets.json.
			ViewBag.FontAwesomeKey = _config.GetValue<string>("fontawesome-cdn-key");

			// Hard-coded date because we'll only have a subset of data for a particular date.
			var appsModel = GetApplicationIssues(new DateTime(2019, 6, 6));

			return View(appsModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		private IEnumerable<App> GetApplicationIssues(DateTime date)
		{
			var applications = _applicationRepo.GetApplications()?
				.Where(a => a.IsEnabled == true)
				.OrderBy(a => a.Id);

			var issues = _issueRepository.GetIssuesByDate(date, date.AddDays(1));

			List<App> appsModel = new List<App>();
			foreach(var application in applications)
			{
				appsModel.Add(new App
				{
					Name = application.Name,
					IssueCount = issues.Where(i => i.ApplicationId == application.Id).Count(),
					HasUrgentPriority = issues.Any(
						i => i.ApplicationId == application.Id
						&& i.Priority == Priority.Urgent),
					Url = application.Url
				});
			}

			return appsModel;
		}
	}
}
