using Argus.Core;
using Argus.Core.Application;
using Argus.Core.Data;
using Argus.Core.Enums;
using Argus.Core.Issue;
using Argus.Infrastructure.Repositories;
using Argus.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Argus.Tests
{
	[TestClass]
	[TestCategory("Controller")]
	public class HomeControllerTests
	{
		private static IConfiguration _config;
		private static IGenericRepository<IEntity> _repository;
		private static IApplicationService _applicationService;
		private static IIssueService _issueService;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_applicationService = new ApplicationService(new GenericRepository<Application>(_config, "Argus"));
			_issueService = new IssueService(new GenericRepository<Issue>(_config, "Argus"));
		}
		
		[TestMethod]
		[TestCategory("Integration")]
		public void Index_ModelIsNotNull()
		{
			var controller = new HomeController(_config, _applicationService, _issueService);

			var result = controller.Index() as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsFalse(string.IsNullOrWhiteSpace(controller.ViewBag.FontAwesomeKey));
			Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<App>));
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Index_Moq_ModelExcludedDisabledApps_ModelHasUrgentPriorityIssue()
		{
			DateTime date = new DateTime(2019, 6, 6);
			int expectedModelCount = 2;
			int expectedUrgentPriorityCount = 1;

			Mock<IApplicationService> mockApplicationService = new Mock<IApplicationService>();
			mockApplicationService
				.Setup(m => m.GetApplications())
				.Returns(new List<Application>
				{
					new Application { Id = 1, Name = "Application_1", Url = "www.Application_1.com", IsEnabled = true },
					new Application { Id = 2, Name = "Application_2", Url = "www.Application_2.com", IsEnabled = false },
					new Application { Id = 3, Name = "Application_3", Url = "www.Application_3.com", IsEnabled = true }
				});
			Mock<IIssueService> mockIssueService = new Mock<IIssueService>();
			mockIssueService
				.Setup(m => m.GetIssuesByDate(date, date.AddDays(1)))
				.Returns(new List<Issue>
				{
					new Issue { Id = 1, ApplicationId = 1, DateSubmitted = date, Priority = Priority.Normal },
					new Issue { Id = 2, ApplicationId = 2, DateSubmitted = date, Priority = Priority.Normal },
					new Issue { Id = 3, ApplicationId = 3, DateSubmitted = date, Priority = Priority.Urgent }
				});
			Mock<HomeController> mockController = new Mock<HomeController>(_config, mockApplicationService, mockIssueService);
			mockController
				.Setup(m => m.Index())
				.Returns(new ViewResult());

			var controller = new HomeController(_config, _applicationService, _issueService);
			var result = controller.Index() as ViewResult;
			var resultModel = result.Model as IEnumerable<App>;

			Assert.IsNotNull(result.Model);
			Assert.IsFalse(string.IsNullOrWhiteSpace(controller.ViewBag.FontAwesomeKey));
			Assert.IsInstanceOfType(result.Model, typeof(IEnumerable<App>));
			// Ensure only enabled apps are displayed:
			Assert.AreEqual(resultModel.Count(), expectedModelCount);
			// Ensure Index properly vetted Urgent priority issue.
			Assert.AreEqual(resultModel.Where(a => a.HasUrgentPriority).Count(), expectedUrgentPriorityCount);
		}
	}
}
