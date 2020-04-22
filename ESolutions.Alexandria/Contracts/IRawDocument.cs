using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESolutions.Alexandria.Contracts
{
	public interface IRawDocument : IDocument
	{
		new Guid Guid { get; set; }
		new DateTime ArchiveTimestampUtc { get; set; }
		new DateTime DocumentTimestampUtc { get; set; }
	}
}
