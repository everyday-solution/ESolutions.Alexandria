using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESolutions.Alexandria.Contracts;
using ESolutions.Alexandria.Persistence;

namespace ESolutions.Alexandria.UnitTests
{
	public class UnitTestStoreClient : IStoreClient
	{
		//Fields
		#region documentStore
		private readonly Dictionary<IDocument, Stream> documentStore = new Dictionary<IDocument, Stream>();
		#endregion

		//Properties
		#region CalledMethods
		public List<String> CalledMethods
		{
			get;
			set;
		} = new List<String>();
		#endregion

		//Methods
		#region CreateDocument
		public IRawDocument CreateDocument()
		{
			this.CalledMethods.Add(nameof(CreateDocument));
			return new DocumentMeta();
		}
		#endregion

		#region LoadAttachmentAsync
		public async Task<Stream> LoadAttachmentAsync(IDocument document)
		{
			return await Task.Run<Stream>(() =>
			{
				this.CalledMethods.Add(nameof(LoadAttachmentAsync));
				return this.documentStore[document];
			});
		}
		#endregion

		#region SaveDocumentAsync
		public async Task SaveDocumentAsync(IDocument document)
		{
			await Task.Run(() =>
			{
				this.CalledMethods.Add(nameof(SaveDocumentAsync));
				this.documentStore.Add(document, document.Data);
			});
		}
		#endregion

		#region SearchAsync
		public async Task<IEnumerable<IDocument>> SearchAsync(params String[] searchTerms)
		{
			return await Task.Run<IEnumerable<IDocument>>(() =>
			{
				this.CalledMethods.Add(nameof(SearchAsync));
				var results = this.documentStore
					.Where(document => document.Key.Tags.Any(tag => searchTerms.Contains(tag)))
					.Select(runner => runner.Key)
					.ToList();
				return results;
			});
		}
		#endregion
	}
}
