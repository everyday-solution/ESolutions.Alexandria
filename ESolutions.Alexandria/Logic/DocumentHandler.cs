﻿using ESolutions.Alexandria.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Logic
{
	public class DocumentHandler
	{
		//Fields
		#region documentHandler
		readonly IDocumentHandler documentHandler = null;
		#endregion

		#region fileReader
		private readonly IFileReader fileReader = null;
		#endregion

		//Constructors
		#region DocumentHandler
		public DocumentHandler(IDocumentHandler documentHandler, IFileReader fileReader)
		{
			this.documentHandler = documentHandler;
			this.fileReader = fileReader;
		}
		#endregion

		//Methods
		#region SaveAsync
		public async Task<IDocument> SaveAsync(Stream data, String originalFilename, IEnumerable<String> tags, DateTime createDateUtc)
		{
			var bytes = new Byte[data.Length];
			data.Read(bytes, 0, (Int32)data.Length);
			var hash = System.Security.Cryptography.SHA512.Create().ComputeHash(bytes);
			data.Position = 0;
			var newDocument = this.documentHandler.CreateDocument();

			newDocument.Guid = Guid.NewGuid();
			newDocument.ArchiveTimestampUtc = DateTime.UtcNow;
			newDocument.DocumentTimestampUtc = createDateUtc;
			newDocument.OriginalFileName = originalFilename;
			newDocument.Tags = tags;
			newDocument.Hash = hash;
			newDocument.Data = data;
			newDocument.Fulltext = this.fileReader?.ReadFulltext(data);

			await this.documentHandler.SaveDocumentAsync(newDocument);

			return newDocument;
		}
		#endregion

		#region SearchAsync
		public async Task<IEnumerable<IDocument>> SearchAsync(params String[] tags)
		{
			return await this.documentHandler.SearchAsync(tags);
		}
		#endregion

		#region LoadSingleAsync
		public async Task<IDocument> LoadAttachmentAsync(IDocument document)
		{
			var data = await this.documentHandler.LoadAttachmentAsync(document);
			document.Data = data;
			return document;
		}
		#endregion
	}
}