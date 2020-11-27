using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ESolutions.Alexandria.Contracts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;

namespace ESolutions.Alexandria.PdfFileReader
{
	public class PdfFileReader : IFileReader
	{
		//Properties
		#region FileExtension
		public String FileExtension
		{
			get
			{
				return ".pdf";
			}
		}
		#endregion

		//Methods
		#region ReadFulltext
		public String ReadFulltext(Stream data)
		{
			var document = PdfReader.Open(data, PdfDocumentOpenMode.ReadOnly);
			var textParts = new List<String>();
			foreach (var runner in document.Pages)
			{
				var cObject = ContentReader.ReadContent(runner);
				var pageWord = this.ExtractText(cObject);
				textParts.AddRange(pageWord);
			}

			var result = String.Join(' ', textParts);

			return result;
		}
		#endregion

		#region ReadTextFromPage
		private IEnumerable<String> ExtractText(CObject cObject)
		{
			var textList = new List<String>();
			if (cObject is COperator)
			{
				var cOperator = cObject as COperator;
				if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
					cOperator.OpCode.Name == OpCodeName.TJ.ToString())
				{
					foreach (var cOperand in cOperator.Operands)
					{
						textList.AddRange(this.ExtractText(cOperand));
					}
				}
			}
			else if (cObject is CSequence)
			{
				var cSequence = cObject as CSequence;
				foreach (var element in cSequence)
				{
					textList.AddRange(this.ExtractText(element));
				}
			}
			else if (cObject is CString)
			{
				var cString = cObject as CString;
				textList.Add(cString.Value);
			}
			return textList;
		}
		#endregion
	}
}
