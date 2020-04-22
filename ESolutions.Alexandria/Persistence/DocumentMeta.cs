using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESolutions.Alexandria.Contracts;

namespace ESolutions.Alexandria.Persistence
{
	public class DocumentMeta : IRawDocument
	{
		[JsonProperty(PropertyName = "id")]
		public String Id
		{
			get;
			set;
		}

		[JsonIgnore]
		public Guid Guid
		{
			get
			{
				return new Guid(this.Id);
			}
			set
			{
				this.ToString();
			}
		}

		[JsonProperty(PropertyName = "archiveDate")]
		public DateTime ArchiveTimestampUtc
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "documentDate")]
		public DateTime DocumentTimestampUtc
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "fileName")]
		public String OriginalFileName
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "tags")]
		public IEnumerable<String> Tags
		{
			get;
			set;
		}

		[JsonProperty(PropertyName = "hash")]
		public Byte[] Hash
		{
			get;
			set;
		}

		[JsonIgnore]
		public Stream Data
		{
			get; set;
		}

		[JsonProperty(PropertyName = "fulltext")]
		public String Fulltext
		{
			get;
			set;
		}
	}
}
