using System;
using System.IO;
using ESolutions.Alexandria.Contracts;

namespace ESolutions.Alexandria.TxtFileReader
{
	public class TxtFileReader : IFileReader
	{
		//Properties
		#region FileExtension
		public String FileExtension
		{
			get
			{
				return ".txt";
			}
		}
		#endregion

		//Methods
		#region ReadFulltext
		public String ReadFulltext(Stream data)
		{
			var result = new StreamReader(data).ReadToEnd();
			return result;
		}
		#endregion
	}
}
