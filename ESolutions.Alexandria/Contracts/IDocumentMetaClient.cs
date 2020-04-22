using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Contracts
{
	public interface IDocumentMetaClient
	{
		Task<IDocument> GetDocumentAsync(Guid guid);
		Task<IDocument> SaveDocumentAsync(IDocument document);
		Task<IEnumerable<IDocument>> SearchAsync(params String[] searchTermn);
	}
}
