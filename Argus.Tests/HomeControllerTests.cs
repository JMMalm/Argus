using Argus.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Argus.Tests
{
	[TestClass]
	public class HomeControllerTests
	{
		private static IConfiguration _config;

		[ClassInitialize]
		public static void Initialize(TestContext context)
		{
			_config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
				.Build();
		}
		
		[TestMethod]
		[TestCategory("Integration")]
		public void Index_ModelIsNotNull()
		{
			var controller = new HomeController(_config);

			ViewResult result = controller.Index() as ViewResult;

			Assert.IsNotNull(result.Model);
		}
	}
}
