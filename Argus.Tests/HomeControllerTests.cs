using Argus.Core;
using Argus.Core.Application;
using Argus.Core.Data;
using Argus.Core.Enums;
using Argus.Core.Issue;
using Argus.Infrastructure.Repositories;
using Argus.MVC.Controllers;
using Argus.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
		private static IApplicationService _applicationService;
		private static IIssueService _issueService;
		private static Mock<ILogger<HomeController>> _logger;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_applicationService = new ApplicationService(new GenericRepository<Application>(_config));
			_issueService = new IssueService(new GenericRepository<Issue>(_config));
			_logger = new Mock<ILogger<HomeController>>();
		}
		
		[TestMethod]
		[TestCategory("Integration")]
		public void Index_ModelIsNotNull()
		{
			int expectedMinimumModelCount = 1;
			var controller = new HomeController(_config, _applicationService, _issueService, _logger.Object);

			var result = controller.Index() as ViewResult;
			var resultModel = (result as ViewResult).Model as IEnumerable<ApplicationModel>;

			Assert.IsNotNull(result.Model);
			Assert.IsFalse(string.IsNullOrWhiteSpace(controller.ViewBag.FontAwesomeKey));
			Assert.IsTrue(resultModel.Count() >= expectedMinimumModelCount);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void Index_Moq_ModelExcludedDisabledApps_ModelHasUrgentPriorityIssue()
		{
			DateTime date = new DateTime(2019, 6, 6);
			int expectedModelCount = 2;
			int expectedUrgentPriorityCount = 1;
			Mock<IApplicationService> mockApplicationService = TestData.GetMockApplicationService();
			Mock<IIssueService> mockIssueService = TestData.GetMockIssueService(date);
			HomeController mockController = new HomeController(_config, mockApplicationService.Object, mockIssueService.Object, _logger.Object);

			var result = mockController.Index() as ViewResult;
			var resultModel = (result as ViewResult).Model as IEnumerable<ApplicationModel>;

			mockApplicationService.Verify(a => a.GetApplications(), Times.Once);
			mockIssueService.Verify(i => i.GetIssuesByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);

			Assert.IsNotNull(result);
			Assert.IsFalse(string.IsNullOrWhiteSpace(mockController.ViewBag.FontAwesomeKey));
			// Ensure only enabled apps are displayed:
			Assert.AreEqual(resultModel.Count(), expectedModelCount);
			// Ensure Index properly vetted Urgent priority issue.
			Assert.AreEqual(resultModel.Where(a => a.HasUrgentPriority).Count(), expectedUrgentPriorityCount);
		}
	}
}
