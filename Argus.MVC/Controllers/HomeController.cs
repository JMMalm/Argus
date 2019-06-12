using Argus.Core.Application;
using Argus.Core.Enums;
using Argus.Core.Issue;
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
		private readonly IApplicationService _applicationService;
		private readonly IIssueService _issueService;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="applicationRepo"></param>
		/// <param name="issueRepository"></param>
		/// <remarks>
		///		The repositories are registered as services in Startup.cs, so that's where they come from.
		/// </remarks>
		public HomeController(IConfiguration config, IApplicationService applicationService, IIssueService issueService)
		{
			_config = config ??
				throw new ArgumentNullException("Configuration cannot be null.");
			_applicationService = applicationService ??
				throw new ArgumentNullException(nameof(applicationService), "Application Service cannot be null.");
			_issueService = issueService ??
				throw new ArgumentNullException(nameof(issueService), "Issue Service cannot be null.");
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

		[HttpGet]
		public IActionResult GetApplicationUpdates(int[] applicationIds, bool isTest)
		{
			try
			{
				var applications = GetApplicationIssues(new DateTime(2019, 6, 6))
						.Where(a => applicationIds.Contains(a.Id))
						.ToList();

				//Modify some data to test client-side updating, because we have limited data to work with.
				if (isTest && applications?.Count() >= 2)
				{
					applications[0].IssueCount = 3;
					applications[1].IssueCount = 2;
					applications[2].HasUrgentPriority = !applications[2].HasUrgentPriority;
				}

				return Json(applications);
			}
			catch (Exception ex)
			{
				// TODO: Add error logging.
				return Error();
			}
		}

		private IEnumerable<ApplicationModel> GetApplicationIssues(DateTime date)
		{
			var applications = _applicationService.GetApplications()?
				.Where(a => a.IsEnabled == true)
				.OrderBy(a => a.Id);

			var issues = _issueService.GetIssuesByDate(date, date.AddDays(1));

			List<ApplicationModel> appsModel = new List<ApplicationModel>();
			foreach(var application in applications)
			{
				appsModel.Add(new ApplicationModel
				{
					Id = application.Id,
					Name = application.Name,
					IssueCount = issues.Where(i => i.ApplicationId == application.Id).Count(),
					HasUrgentPriority = issues.Any(
						i => i.ApplicationId == application.Id
						&& i.Priority == Priority.Urgent),
					ProductOwnerName = application.ProductOwnerName,
					TeamName = application.TeamName,
					Url = application.Url
				});
			}

			return appsModel;
		}
	}
}
