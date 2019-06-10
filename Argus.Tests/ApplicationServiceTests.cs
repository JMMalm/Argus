using Argus.Core;
using Argus.Core.Application;
using Argus.Core.Data;
using Argus.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Argus.Tests
{
	[TestClass]
	[TestCategory("Repository")]
	public class ApplicationServiceTests
	{
		private static IConfiguration _config;
		private static IGenericRepository<Application> _repository;
		private static IApplicationService _applicationService;

		/// <summary>
		/// Sets up needed objects and facilitates their re-use in tests.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = TestAssistant.GetConfig();
			_repository = new GenericRepository<Application>(_config);
			_applicationService = new ApplicationService(_repository);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetApplications_ReturnsCollection()
		{
			var expectedCount = 1;

			var result = _applicationService.GetApplications();

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Application>));
			Assert.IsTrue(result.ToList().Count >= expectedCount);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void GetApplications_Moq_ReturnsCollection()
		{
			var expectedCount = 1;
			Mock<IApplicationService> mockService = new Mock<IApplicationService>();
			mockService
				.Setup(m => m.GetApplications())
				.Returns(GetTestApplications());

			var result = mockService.Object.GetApplications();

			Assert.IsInstanceOfType(result, typeof(IEnumerable<Application>));
			Assert.IsTrue(result.ToList().Count >= expectedCount);
		}

		[TestMethod]
		[TestCategory("Integration")]
		public void GetApplicationById_IdExists_ReturnsCorrectApplication()
		{
			var expectedApplicationId = 1;

			var result = _applicationService.GetById(expectedApplicationId);

			Assert.IsInstanceOfType(result, typeof(Application));
			Assert.AreEqual(expectedApplicationId, result.Id);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void GetApplicationById_Moq_IdExists_ReturnsCorrectApplication()
		{
			var expectedApplicationId = 1;
			Application expectedApplication = GetTestApplications().ToList()[1];
			Mock<IApplicationService> mockService = new Mock<IApplicationService>();
			mockService
				.Setup(m => m.GetById(It.Is<int>(id => id == 1)))
				.Returns(expectedApplication);

			var result = mockService.Object.GetById(expectedApplicationId);

			Assert.IsInstanceOfType(result, typeof(Application));
			Assert.AreEqual(expectedApplicationId, result.Id);
		}

		[TestMethod]
		[TestCategory("Unit")]
		public void GetApplicationById_Moq_IdDoesNotExist_ReturnsNull()
		{
			var expectedApplicationId = 1;
			Application expectedApplication = GetTestApplications().ToList()[1];
			Mock<IApplicationService> mockService = new Mock<IApplicationService>();
			mockService
				.Setup(m => m.GetById(It.Is<int>(id => id != 1)))
				.Returns(value: null);

			var result = mockService.Object.GetById(expectedApplicationId);

			Assert.IsNull(result);
		}

		private IEnumerable<Application> GetTestApplications()
		{
			return new List<Application>
			{
				new Application { Id = 0, Name = "Application_0", ProductOwnerName = "John doe", TeamName = "Team_1", IsEnabled = true },
				new Application { Id = 1, Name = "Application_1", ProductOwnerName = "Jane doe", TeamName = "Team_2", IsEnabled = true },
				new Application { Id = 1, Name = "Application_2", ProductOwnerName = "Rick Sanchez", TeamName = "Team_3", IsEnabled = false }
			};
		}
	}
}
