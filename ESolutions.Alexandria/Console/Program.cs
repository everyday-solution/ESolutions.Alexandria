using ESolutions.Alexandria.Logic;
using ESolutions.Alexandria.PdfFileReader;
using ESolutions.Alexandria.Persistence;
using ESolutions.Core.Console;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESolutions.Alexandria.Console
{
	class Program
	{
		//Fields
		#region cosmosClient
		private static CosmosClient cosmosClient = null;
		#endregion

		#region handler
		private static DocumentHandler handler = null;
		#endregion

		#region config
		private static Settings config = null;
		#endregion

		//Methods
		#region Main
		public static async Task Main(String[] args)
		{
			try
			{
				Program.config = new ConfigurationBuilder()
					.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
					.AddJsonFile("appsettings.json", false)
					.Build()
					.GetSection("Settings")
					.Get<Settings>();

				Program.cosmosClient = await CosmosClient.CreateAsync(config.Cosmos.EndpointUri, config.Cosmos.PrimaryKey, config.Cosmos.DatabaseId, config.Cosmos.ContainerId);
				var blobClient = BlobClient.Create(config.Blob.ConnectionString, config.Blob.ContainerName);
				var storeClient = new StoreClient(Program.cosmosClient, blobClient);
				var pdfFileReader = new PdfFileReader.PdfFileReader();
				Program.handler = new DocumentHandler(storeClient, pdfFileReader);

				var menu = new List<MenuItem>()
				{
					new MenuItem('a', "Write file", Program.WriteFiles),
					new MenuItem('b', "Read files", Program.ReadFiles)
				};

				ConsoleMenu.Run(args, menu, new List<ArgumentOperation>());
			}
			catch (Exception ex)
			{
				System.Console.Write(ex.Message);
			}
			finally
			{
				Program.cosmosClient.Dispose();
			}
		}
		#endregion

		#region ReadFiles
		private static void ReadFiles(IEnumerable<String> args)
		{
			var tags = Program.GetTagList().ToArray();

			Task.Run(async () =>
			{
				System.Console.WriteLine("Searching...");
				var results = await handler.SearchAsync(tags);
				System.Console.WriteLine($"Found {results.Count()} documents");
				var index = 1;
				foreach (var runner in results)
				{
					System.Console.WriteLine($"Loading document {index} of {results.Count()}...");
					var doc = await Program.handler.LoadAttachmentAsync(runner);

					var fileName = Path.Combine(config.BaseDirectoryInfo.FullName, $"down_{runner.OriginalFileName}");

					var jsonFileName = new FileInfo(fileName + ".json");
					using (var file = jsonFileName.CreateText())
					{
						var json = JsonSerializer.Serialize(doc);
					}

					var pdfFileName = new FileInfo(fileName + ".pdf");
					using (var outputFile = pdfFileName.Create())
					{
						doc.Data.CopyTo(outputFile);
						doc.Data.Close();
					}

					System.Console.WriteLine($"Saved document {pdfFileName.Name}");
					index++;
				}

				System.Console.WriteLine("All documents loaded");
			}).Wait();
		}
		#endregion

		#region WriteFiles
		private static void WriteFiles(IEnumerable<String> args)
		{
			var pdfFile = Program.GetFile();
			if (pdfFile != null)
			{
				var tags = Program.GetTagList();
				Task.Run(async () =>
				{
					using var data = pdfFile.OpenRead();
					System.Console.WriteLine("Uploading...");
					await Program.handler.SaveAsync(data, pdfFile.Name, tags, pdfFile.CreationTimeUtc);

					System.Console.WriteLine("Document saved!");
				}).Wait();
			}
		}
		#endregion

		#region GetTagList
		private static List<String> GetTagList()
		{
			System.Console.WriteLine($"Enter tags. Leave blank and hit enter to continue:");
			var tags = new List<String>();
			var input = System.Console.ReadLine();
			while (input != String.Empty)
			{
				tags.Add(input);
				input = System.Console.ReadLine();
			}

			return tags;
		}
		#endregion

		#region GetFile
		public static FileInfo GetFile()
		{
			FileInfo file = null;
			var filesInBaseDirectory = config.BaseDirectoryInfo.EnumerateFiles("*.pdf").ToList();
			var index = 1;
			foreach (var runner in filesInBaseDirectory)
			{
				System.Console.WriteLine($"{index}) {runner.Name}");
				index++;
			}

			System.Console.WriteLine("x) Cancel");
			var userInput = System.Console.ReadLine();
			if (userInput != "x")
			{
				var selectedIndex = Int32.Parse(userInput) - 1;
				file = filesInBaseDirectory[selectedIndex];
			}
			return file;
		}
		#endregion
	}
}
