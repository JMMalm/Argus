using Argus.Core;
using Argus.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Argus.Tests
{
	[TestClass]
	public class ApplicationRepositoryTests
	{
		private static IConfiguration _config;
		private static IApplicationRepository _applicationRepo;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile(@"c:\code\argus\argus.MVC\secrets.json", optional: false, reloadOnChange: true)
				.Build();
			_applicationRepo = new ApplicationRepository(_config);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetApplications_ReturnsCollection()
		{
			var expectedCount = 1;

			var result = _applicationRepo.GetApplications();

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Application>));
			Assert.IsTrue(result.ToList().Count >= expectedCount);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetApplications_ReturnsCorrectApplication()
		{
			var expectedApplicationId = 1;

			var result = _applicationRepo.GetApplicationById(expectedApplicationId);

			Assert.IsInstanceOfType(result, typeof(Application));
			Assert.AreEqual(expectedApplicationId, result.Id);
		}
	}
}
