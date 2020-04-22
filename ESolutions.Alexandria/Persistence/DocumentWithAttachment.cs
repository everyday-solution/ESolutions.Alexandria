using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Persistence
{
	public class DocumentWithAttachment
	{
		//Properties
		#region Document
		public DocumentMeta Document
		{
			get;
			private set;
		}
		#endregion

		#region Attachment
		public Stream Attachment
		{
			get;
			private set;
		}
		#endregion

		//Constructors
		#region DocumentWithAttachment
		public DocumentWithAttachment(DocumentMeta document, Stream attachment)
		{
			this.Document = document;
			this.Attachment = attachment;
		}
		#endregion
	}
}
