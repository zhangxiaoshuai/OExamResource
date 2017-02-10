using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZipClass.Help
{
	public class ZipFileClass
	{
		public ZipFileClass()
		{
		}

		public byte[] UnZipFile(string path1, string path2)
		{
			byte[] numArray;
			string str = (new UnZipClassTrue()).UnZipTrue(path1, "641182", path2);
			List<string> list = Directory.GetFiles(str).ToList<string>();
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				Crc32 crc32 = new Crc32();
				ZipOutputStream zipOutputStream = new ZipOutputStream(memoryStream);
				zipOutputStream.SetLevel(6);
				foreach (string str1 in list)
				{
					FileStream fileStream = new FileStream(str1, FileMode.Open);
					try
					{
						numArray = new byte[checked(fileStream.Length)];
						fileStream.Read(numArray, 0, (int)numArray.Length);
						ZipEntry zipEntry = new ZipEntry(str1)
						{
							DateTime = DateTime.Now,
							Size = fileStream.Length
						};
						crc32.Reset();
						crc32.Update(numArray);
						zipEntry.Crc = crc32.Value;
						zipOutputStream.PutNextEntry(zipEntry);
						zipOutputStream.Write(numArray, 0, (int)numArray.Length);
					}
					finally
					{
						if (fileStream != null)
						{
							((IDisposable)fileStream).Dispose();
						}
					}
				}
				zipOutputStream.Finish();
				numArray = new byte[checked(memoryStream.Length)];
				memoryStream.Position = (long)0;
				memoryStream.Read(numArray, 0, (int)numArray.Length);
			}
			finally
			{
				if (memoryStream != null)
				{
					((IDisposable)memoryStream).Dispose();
				}
			}
			Directory.Delete(str, true);
			return numArray;
		}
	}
}