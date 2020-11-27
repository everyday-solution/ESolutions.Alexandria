using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESolutions.Alexandria.Contracts
{
	public interface IFileReader
	{
		String FileExtension { get; }

		String ReadFulltext(Stream data);
	}
}
