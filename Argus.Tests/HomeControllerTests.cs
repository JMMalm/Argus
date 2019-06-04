using Argus.Core;
using Argus.Infrastructure.Repositories;
using Argus.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Argus.Tests
{
	[TestClass]
	[TestCategory("Controller")]
	public class HomeControllerTests
	{
		private static IConfiguration _config;
		private static IAppRepository _appRepo;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_appRepo = new AppRepository(_config);
		}
		
		[TestMethod]
		[TestCategory("Integration")]
		public void Index_ModelIsNotNull()
		{
			var controller = new HomeController(_config, _appRepo);

			var result = controller.Index() as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsFalse(string.IsNullOrWhiteSpace(controller.ViewBag.FontAwesomeKey));
			Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<App>));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Index_Moq_ModelIsNotNull()
		{
			Mock<IAppRepository> mockRepo = new Mock<IAppRepository>();
			mockRepo
				.Setup(m => m.GetAppDataByDate(It.Is<DateTime>(viewDate => viewDate >= DateTime.MinValue)))
				.Returns(new List<App>
				{
					new App
					{
						Name = "Application 1",
						IssueCount = 1,
						QueryString = "www.Application_1.com"
					}
				});
			var controller = new HomeController(_config, mockRepo.Object);

			var result = controller.Index() as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsFalse(string.IsNullOrWhiteSpace(controller.ViewBag.FontAwesomeKey));
			Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<App>));
		}
	}
}
