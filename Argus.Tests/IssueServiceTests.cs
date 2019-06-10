using Argus.Core;
using Argus.Core.Data;
using Argus.Core.Issue;
using Argus.Infrastructure;
using Argus.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Argus.Tests
{
	[TestClass]
	[TestCategory("Repository")]
	public class IssueServiceTests
	{
		private static IConfiguration _config;
		private static IGenericRepository<Issue> _repository;
		private static IIssueService _issueService;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_repository = new GenericRepository<Issue>(_config);
			_issueService = new IssueService(_repository);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssues_ReturnsCollection()
		{
			var expectedCount = 1;

			var result = _issueService.GetIssues();

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Issue>));
			Assert.IsTrue(result.ToList().Count >= expectedCount);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssueById_ReturnsIssue_IdsMatch()
		{
			var expectedId = 5;

			Issue result = _issueService.GetIssueById(expectedId);

			Assert.AreEqual(expectedId, result.Id);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssuesByDate_ReturnsIssues_IssuesHaveCorrectDate()
		{
			var expectedStartDate = new DateTime(2019, 06, 06);
			var expectedEndDate = expectedStartDate.AddDays(1);

			var result = _issueService.GetIssuesByDate(expectedStartDate, expectedEndDate);

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Issue>));
			result.ToList().ForEach(i =>
			{
				Assert.IsTrue(expectedStartDate < i.DateSubmitted
					&& expectedEndDate > i.DateSubmitted);
			});
		}
	}
}
