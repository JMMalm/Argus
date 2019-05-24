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
		private readonly AppRepository _apprRepo;

		public HomeController(IConfiguration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("Configuration cannot be null.");
			}
			_config = config;
			_apprRepo = new AppRepository(_config);
		}

		public IActionResult Index()
		{
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
