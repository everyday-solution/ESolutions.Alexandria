using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ESolutions.Alexandria.Contracts;
using ESolutions.Alexandria.Logic;
using ESolutions.Alexandria.PdfFileReader;
using ESolutions.Alexandria.Persistence;
using Xunit;

namespace ESolutions.Alexandria.UnitTests
{
	public class DocumentHandlerTests
	{
		//Classes
		#region TestEnvironment
		private class TestEnvironment
		{
			//Properties
			#region StoreClient
			public UnitTestStoreClient StoreClient { get; private set; }
			#endregion

			#region PdfFileReader
			public PdfFileReader.PdfFileReader PdfFileReader
			{
				get;
				set;
			}
			#endregion

			#region DocumentHandler
			public DocumentHandler DocumentHandler { get; private set; }
			#endregion

			#region Tags
			public IEnumerable<String> Tags
			{
				get;
				set;
			}
			#endregion

			#region FileName
			public String FileName { get; private set; }
			#endregion

			#region FileContent
			public MemoryStream FileContent { get; private set; }
			#endregion

			#region FileDate
			public DateTime FileDate { get; private set; }
			#endregion

			#region FileContentHash
			public Byte[] FileContentHash { get; private set; }
			#endregion

			#region SampleDocument
			public IDocument SampleDocument
			{
				get;
				set;
			}
			#endregion

			//Constructors
			#region TestEnvironment
			public TestEnvironment()
			{
				this.StoreClient = new UnitTestStoreClient();
				this.PdfFileReader = new PdfFileReader.PdfFileReader();
				this.DocumentHandler = new DocumentHandler(this.StoreClient, this.PdfFileReader);
				this.Tags = new String[] { "one", "two", "three" };
				this.FileName = "original.pdf";
				this.FileContent = new MemoryStream(Properties.Resources.Google);
				this.FileDate = new DateTime(2020, 01, 01);

				var bytes = this.FileContent.ToArray();
				this.FileContentHash = System.Security.Cryptography.SHA512.Create().ComputeHash(bytes);
			}
			#endregion

			//Methods
			#region CreateSampleDocument
			public async Task<IDocument> CreateSampleDocument()
			{
				return await this.DocumentHandler.SaveAsync(this.FileContent, this.FileName, this.Tags, this.FileDate);
			}
			#endregion
		}
		#endregion

		//Methods
		#region TestSaveAsync
		[Fact]
		public async void TestSaveAsync()
		{
			var environment = new TestEnvironment();

			var actual = await environment.DocumentHandler.SaveAsync(
				environment.FileContent,
				environment.FileName,
				environment.Tags,
				environment.FileDate);

			Assert.Equal(DateTime.UtcNow.Date, actual.ArchiveTimestampUtc.Date);
			Assert.Equal(environment.FileDate, actual.DocumentTimestampUtc);
			Assert.Equal(environment.FileName, actual.OriginalFileName);
			Assert.Equal(environment.Tags, actual.Tags);
			Assert.Equal(environment.FileContentHash, actual.Hash);
			Assert.Equal(environment.FileContent, actual.Data);
			Assert.Equal(266, actual.Fulltext.Length);

			Assert.Contains(nameof(UnitTestStoreClient.CreateDocument), environment.StoreClient.CalledMethods);
			Assert.Contains(nameof(UnitTestStoreClient.SaveDocumentAsync), environment.StoreClient.CalledMethods);
		}
		#endregion

		#region TestSearchAndLoadAttachment
		[Fact]
		public async void TestSearchAndLoadAttachment()
		{
			var environment = new TestEnvironment();
			var doc1 = await environment.CreateSampleDocument();
			var doc2 = await environment.CreateSampleDocument();

			var documents = await environment.DocumentHandler.SearchAsync("one");
			Assert.Equal(2, documents.Count());
			Assert.Contains(nameof(UnitTestStoreClient.SearchAsync), environment.StoreClient.CalledMethods);

			var document = documents.First();
			Assert.NotNull(document);

			var documentWithAttachments = await environment.DocumentHandler.LoadAttachmentAsync(document);
			Assert.Contains(nameof(UnitTestStoreClient.LoadAttachmentAsync), environment.StoreClient.CalledMethods);
			Assert.Equal(doc1.Data, document.Data);
		}
		#endregion
	}
}
