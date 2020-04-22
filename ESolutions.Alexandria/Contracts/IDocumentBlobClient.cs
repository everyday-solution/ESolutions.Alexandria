using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Contracts
{
	public interface IDocumentBlobClient
	{
		Task<Stream> GetFileAsync(IDocument document);
		Task SaveFileAsync(IDocument document);
	}
}
