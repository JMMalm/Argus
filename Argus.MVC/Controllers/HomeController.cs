using Argus.Core;
using Argus.Infrastructure.Repositories;
using Argus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Argus.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly IConfiguration _config;
		private readonly IAppRepository _apprRepo;

		public HomeController(IConfiguration config, IAppRepository appRepo)
		{
			_config = config ??
				throw new ArgumentNullException(nameof(config), "Configuration cannot be null.");
			_apprRepo = appRepo ??
				throw new ArgumentNullException(nameof(appRepo), "Repository cannot be null.");
		}

		public IActionResult Index()
		{
			// Get the Font Awesome CDN key from secrets.json.
			ViewBag.FontAwesomeKey = _config.GetValue<string>("fontawesome-cdn-key");

			// Hard-coded date because we'll only have a subset of data for a particular date.
			var appData = _apprRepo.GetAppDataByDate(new DateTime(2019, 05, 23));

			return View(appData);
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
	}
}
