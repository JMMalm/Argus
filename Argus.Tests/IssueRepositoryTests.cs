using Argus.Core;
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
	public class IssueRepositoryTests
	{
		private static IConfiguration _config;
		private static IIssueRepository _issueRepo;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_issueRepo = new IssueRepository(_config);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssues_ReturnsCollection()
		{
			var expectedCount = 1;

			var result = _issueRepo.GetIssues();

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Issue>));
			Assert.IsTrue(result.ToList().Count >= expectedCount);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssueById_ReturnsIssue_IdsMatch()
		{
			var expectedId = 5;

			Issue result = _issueRepo.GetIssueById(expectedId);

			Assert.AreEqual(expectedId, result.Id);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetIssuesByDate_ReturnsIssues_IssuesHaveCorrectDate()
		{
			var expectedStartDate = new DateTime(2019, 06, 06);
			var expectedEndDate = expectedStartDate.AddDays(1);

			var result = _issueRepo.GetIssuesByDate(expectedStartDate, expectedEndDate);

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Issue>));
			result.ToList().ForEach(i =>
			{
				Assert.IsTrue(expectedStartDate < i.DateSubmitted
					&& expectedEndDate > i.DateSubmitted);
			});
		}
	}
}
