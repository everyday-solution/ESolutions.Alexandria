using ESolutions.Alexandria.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Persistence
{
	public class StoreClient : IStoreClient
	{
		//Fields
		#region documentClient
		private readonly IDocumentMetaClient documentClient = null;
		#endregion

		#region blobClient
		private readonly IDocumentBlobClient blobClient = null;
		#endregion

		//Constructors
		#region StoreClient
		public StoreClient(IDocumentMetaClient documentClient, IDocumentBlobClient blobClient)
		{
			this.documentClient = documentClient;
			this.blobClient = blobClient;
		}
		#endregion

		//Methods
		#region LoadSingleDocumentAsync
		public async Task<IDocument> LoadSingleDocumentAsync(Guid documentId)
		{
			return await this.documentClient.GetDocumentAsync(documentId);
		}
		#endregion

		#region CreateDocument
		IRawDocument IStoreClient.CreateDocument()
		{
			return new DocumentMeta();
		}
		#endregion

		#region SaveDocumentAsync
		async Task IStoreClient.SaveDocumentAsync(IDocument document)
		{
			await this.blobClient.SaveFileAsync(document);
			await this.documentClient.SaveDocumentAsync(document);
		}
		#endregion

		#region SearchAsync
		async Task<IEnumerable<IDocument>> IStoreClient.SearchAsync(params String[] searchTerms)
		{
			return await this.documentClient.SearchAsync(searchTerms);
		}
		#endregion

		#region LoadAttachmentAsync
		async Task<Stream> IStoreClient.LoadAttachmentAsync(IDocument document)
		{
			return await this.blobClient.GetFileAsync(document);
		}
		#endregion
	}
}
