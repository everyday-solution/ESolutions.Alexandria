using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESolutions.Alexandria.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace ESolutions.Alexandria.Persistence
{
	public class CosmosClient : IDocumentMetaClient, IDisposable
	{
		//Fields
		#region client
		private Microsoft.Azure.Cosmos.CosmosClient cosmosClient = null;
		#endregion

		#region Container
		private Container cosmosContainer = null;
		#endregion

		//Constructor
		#region CosmosClient
		private CosmosClient()
		{
		}
		#endregion

		#region Create
		public static async Task<CosmosClient> CreateAsync(String endpointUri, String primaryKey, String databaseId, String containerId)
		{
			var result = new CosmosClient();

			result.cosmosClient = new Microsoft.Azure.Cosmos.CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = "LibOfAlexandriaConsole" });
			var databaseResponse = await result.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
			var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerId, "/id");
			result.cosmosContainer = containerResponse.Container;

			return result;
		}
		#endregion

		#region Dispose
		public void Dispose()
		{
			this.cosmosClient.Dispose();
		}
		#endregion

		//Methods
		#region SaveDocumentAsync
		public async Task<IDocument> SaveDocumentAsync(IDocument document)
		{
			var result = await this.cosmosContainer.CreateItemAsync(
				document, 
				new PartitionKey(document.Guid.ToString()));
			return result.Resource;
		}
		#endregion

		#region GetDocumentAsync
		public async Task<IDocument> GetDocumentAsync(Guid guid)
		{
			var result = await this.cosmosContainer.GetItemLinqQueryable<DocumentMeta>()
				.Where(runner => runner.Guid == guid)
				.ToFeedIterator()
				.ReadNextAsync(CancellationToken.None);
			return result.Resource.FirstOrDefault();
		}
		#endregion

		#region SearchAsync
		public async Task<IEnumerable<IDocument>> SearchAsync(params String[] searchTermn)
		{
			var lowerSearchTerm = searchTermn.Select(runner => runner.ToLower()).ToArray();
			IQueryable<DocumentMeta> searchResult = this.cosmosContainer.GetItemLinqQueryable<DocumentMeta>();

			foreach (var runner in lowerSearchTerm)
			{
				searchResult = searchResult.Where(x => x.Tags.Contains(runner));
			}

			var iterator = searchResult.ToFeedIterator();

			var result = new List<DocumentMeta>();

			while (iterator.HasMoreResults)
			{
				var item = await iterator.ReadNextAsync(CancellationToken.None);
				result.AddRange(item.Resource);
			}

			return result;
		}
		#endregion
	}
}
