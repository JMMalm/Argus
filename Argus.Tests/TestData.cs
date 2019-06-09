using Argus.Core.Application;
using Argus.Core.Enums;
using Argus.Core.Issue;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Argus.Tests
{
	public static class TestData
	{
		public static Mock<IApplicationService> GetMockApplicationService()
		{
			Mock<IApplicationService> mockApplicationService = new Mock<IApplicationService>();
			mockApplicationService
				.Setup(m => m.GetApplications())
				.Returns(new List<Application>
				{
					new Application { Id = 1, Name = "Application_1", Url = "www.Application_1.com", IsEnabled = true },
					new Application { Id = 2, Name = "Application_2", Url = "www.Application_2.com", IsEnabled = false },
					new Application { Id = 3, Name = "Application_3", Url = "www.Application_3.com", IsEnabled = true }
				});
			return mockApplicationService;
		}

		public static Mock<IIssueService> GetMockIssueService(DateTime date)
		{
			Mock<IIssueService> mockIssueService = new Mock<IIssueService>();
			mockIssueService
				.Setup(m => m.GetIssuesByDate(date, date.AddDays(1)))
				.Returns(new List<Issue>
				{
					new Issue { Id = 1, ApplicationId = 1, DateSubmitted = date, Priority = Priority.Normal },
					new Issue { Id = 2, ApplicationId = 2, DateSubmitted = date, Priority = Priority.Normal },
					new Issue { Id = 3, ApplicationId = 3, DateSubmitted = date, Priority = Priority.Urgent }
				});
			return mockIssueService;
		}
	}
}
