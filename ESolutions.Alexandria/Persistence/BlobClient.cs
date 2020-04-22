using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;
using ESolutions.Alexandria.Contracts;
using Microsoft.Azure.Storage;

namespace ESolutions.Alexandria.Persistence
{
	public class BlobClient : IDocumentBlobClient
	{
		//Fields
		#region storageAccount
		private CloudStorageAccount storageAccount = null;
		#endregion

		#region blobClient
		private CloudBlobClient blobServiceClient = null;
		#endregion

		#region blobContainer
		private CloudBlobContainer blobContainer = null;
		#endregion

		//Constructors
		#region BlobClient
		private BlobClient()
		{

		}
		#endregion

		#region Create
		public static BlobClient Create(String connectionString, String containerName)
		{
			var result = new BlobClient();

			CloudStorageAccount.TryParse(connectionString, out result.storageAccount);
			result.blobServiceClient = result.storageAccount.CreateCloudBlobClient();
			result.blobContainer = result.blobServiceClient.GetContainerReference(containerName);;

			return result;
		}
		#endregion

		//Methods
		#region GetFileName
		private String GetFileName(IDocument document)
		{
			return document.Guid.ToString() + ".pdf";
		}
		#endregion

		#region SaveFileAsync
		public async Task SaveFileAsync(IDocument document)
		{
			var filename = this.GetFileName(document);
			var blobClient = this.blobContainer.GetBlockBlobReference(filename);
			await blobClient.UploadFromStreamAsync(document.Data);
		}
		#endregion

		#region GetFileAsync
		public async Task<Stream> GetFileAsync(IDocument document)
		{
			var filename = this.GetFileName(document);
			var blobClient = this.blobContainer.GetBlockBlobReference(filename);

			var stream = new MemoryStream();
			await blobClient.DownloadToStreamAsync(stream);

			return stream;
		}
		#endregion
	}
}
