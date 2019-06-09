using System;

namespace Argus.Core.Data
{
	public interface IEntity
	{
		int Id { get; set; }

		DateTime DateModified { get; set; }
	}
}
