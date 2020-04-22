using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Contracts
{
	public interface IDocumentHandler
	{
		IRawDocument CreateDocument();

		Task SaveDocumentAsync(IDocument document);

		Task<IEnumerable<IDocument>> SearchAsync(params String[] searchTerms);

		Task<Stream> LoadAttachmentAsync(IDocument document);
	}
}
