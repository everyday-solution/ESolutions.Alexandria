using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESolutions.Alexandria.Contracts
{
	public interface IDocument
	{
		Guid Guid { get; }
		DateTime ArchiveTimestampUtc { get; }
		DateTime DocumentTimestampUtc { get; }
		String OriginalFileName { get; set; }
		IEnumerable<String> Tags { get; set; }
		Byte[] Hash { get; set; }
		Stream Data { get; set; }
		String Fulltext { get; set; }
	}
}
