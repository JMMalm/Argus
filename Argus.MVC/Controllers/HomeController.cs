using Argus.Core.Application;
using Argus.Core.Enums;
using Argus.Core.Issue;
using Argus.Infrastructure.Logging;
using Argus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger _logger;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="applicationRepo"></param>
		/// <param name="issueRepository"></param>
		/// <remarks>
		///		The repositories are registered as services in Startup.cs, so that's where they come from.
		/// </remarks>
		public HomeController(IConfiguration config, IApplicationService applicationService, IIssueService issueService, ILogger<HomeController> logger)
		{
			_config = config ??
				throw new ArgumentNullException("Configuration cannot be null.");
			_applicationService = applicationService ??
				throw new ArgumentNullException(nameof(applicationService), "Application Service cannot be null.");
			_issueService = issueService ??
				throw new ArgumentNullException(nameof(issueService), "Issue Service cannot be null.");
			_logger = logger;
		}

		public IActionResult Index(int sortOption = 0)
		{
			IEnumerable<ApplicationModel> appsModel = null;

			try
			{
				SortOption sortSelection = GetValidSortOption(sortOption);

				// Get the Font Awesome CDN key from secrets.json.
				ViewBag.FontAwesomeKey = _config.GetValue<string>("fontawesome-cdn-key");
				ViewBag.IssueSubmissionUrl = _config.GetValue<string>("IssueSubmissionUrl");
				ViewBag.SortSelection = (int)sortSelection;

				// Hard-coded date because we'll only have a subset of data for a particular date.
				appsModel = GetApplicationIssues(new DateTime(2019, 6, 6), sortSelection);
			}
			catch (Exception ex)
			{
				// Use StaticLogger until file-writing is possible via ILogger without a 3rd-party package.
				_logger.LogError(ex, "Error in HomeController.Index()!");
				StaticLogger.Write(ex);

				return Error();
			}

			return View(appsModel);
		}

		public IActionResult About()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public IActionResult GetApplicationUpdates(int[] applicationIds, int sortOption = 0, bool isTest = false)
		{
			try
			{
				SortOption sortSelection = GetValidSortOption(sortOption);
				var applications = GetApplicationIssues(new DateTime(2019, 6, 6), sortSelection)
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
				// Use StaticLogger until file-writing is possible via ILogger without a 3rd-party package.
				_logger.LogError(ex, "Error in HomeController.GetApplicationUpdates()!");
				StaticLogger.Write(ex);

				return Error();
			}
		}

		private IEnumerable<ApplicationModel> GetApplicationIssues(DateTime date,SortOption sortSelection = SortOption.Alphabetical)
		{
			var applications = _applicationService.GetApplications()?
				.Where(a => a.IsEnabled == true)
				.OrderBy(a => a.Name);

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

			if (sortSelection == SortOption.Priority)
			{
				appsModel = (
					from a in appsModel
					orderby a.HasUrgentPriority descending, a.IssueCount descending, a.Name
					select a
				).ToList();
			}

			return appsModel;
		}

		private SortOption GetValidSortOption(int sortOption)
		{
			SortOption sortSelection = (int)SortOption.Alphabetical;
			try
			{
				sortSelection =
					(SortOption)Enum.GetValues(typeof(SortOption))
						.Cast<int>()
						.FirstOrDefault(o => o == sortOption);
			}
			catch(Exception ex)
			{
				// This is a minor error but log it anyway.
				StaticLogger.Write(ex);
			}

			return sortSelection;
		}
	}
}
