using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Argus.MVC.ViewComponents
{
	public class ChangeLogViewComponent : ViewComponent
	{
		public ChangeLogViewComponent()
		{

		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			string changeLogContent = string.Empty;
			string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/content/changelog.html");

			using (var reader = File.OpenText(filePath))
			{
				changeLogContent = await reader.ReadToEndAsync();
			}

			return View("Default", changeLogContent);
		}
	}
}
