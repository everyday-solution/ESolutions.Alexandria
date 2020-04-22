using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ESolutions.Alexandria.Console
{
	[JsonObject("Settings")]
	internal class Settings
	{
		#region Cosmos
		[JsonProperty(PropertyName = "cosmos")]
		public CosmosSettings Cosmos
		{
			get;
			set;
		}
		#endregion

		#region Blob
		[JsonProperty(PropertyName = "blob")]
		public BlobSettings Blob
		{
			get;
			set;
		}
		#endregion

		#region BaseDirectory
		[JsonProperty(PropertyName = "baseDirectory")]
		public String BaseDirectory
		{
			get;
			set;
		}
		#endregion

		#region BaseDirectoryInfo
		[JsonIgnore]
		public DirectoryInfo BaseDirectoryInfo
		{
			get
			{
				return new DirectoryInfo(this.BaseDirectory);
			}
		}
		#endregion
	}

	public class CosmosSettings
	{
		#region EndpointUri
		[JsonProperty(PropertyName = "endpointUri")]
		public String EndpointUri
		{
			get;
			set;
		}
		#endregion

		#region PrimaryKey
		[JsonProperty(PropertyName = "primaryKey")]
		public String PrimaryKey
		{
			get;
			set;
		}
		#endregion

		#region DatabaseId
		[JsonProperty(PropertyName = "databaseId")]
		public String DatabaseId
		{
			get;
			set;
		}
		#endregion

		#region ContainerId
		[JsonProperty(PropertyName = "containerId")]
		public String ContainerId
		{
			get;
			set;
		}
		#endregion
	}

	public class BlobSettings
	{
		#region ConnectionString
		[JsonProperty(PropertyName = "connectionString")]
		public String ConnectionString
		{
			get;
			set;
		}
		#endregion

		#region ContainerName
		[JsonProperty(PropertyName = "containerName")]
		public String ContainerName
		{
			get;
			set;
		}
		#endregion
	}
}
