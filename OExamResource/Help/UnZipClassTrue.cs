using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;

namespace ZipClass.Help
{
	public class UnZipClassTrue
	{
		public UnZipClassTrue()
		{
		}

		public string UnZipTrue(string path, string password, string path2)
		{
			ZipEntry nextEntry;
			Secret secret = new Secret();
			//string str = secret.MD5Decrypt("TURNTECH", ConfigurationManager.AppSettings["Syncdown"]);
            string str = "downfile";
            Guid guid = Guid.NewGuid();
			object[] objArray = new object[] { str, "\\", path2, "\\", guid };
			string str1 = string.Concat(objArray);
			ZipInputStream zipInputStream = new ZipInputStream((new WebClient()).OpenRead(path));
			try
			{
				zipInputStream.Password = md5.encrypt(password);
				ZipEntry zipEntry = zipInputStream.GetNextEntry();
				do
				{
					if (!Directory.Exists(str1))
					{
						Directory.CreateDirectory(str1);
					}
					FileStream fileStream = new FileStream(string.Concat(str1, "\\", zipEntry.Name), FileMode.Create, FileAccess.Write);
					try
					{
						int num = 2048;
						byte[] numArray = new byte[2048];
						do
						{
							num = zipInputStream.Read(numArray, 0, (int)numArray.Length);
							fileStream.Write(numArray, 0, num);
						}
						while (num > 0);
					}
					finally
					{
						if (fileStream != null)
						{
							((IDisposable)fileStream).Dispose();
						}
					}
					nextEntry = zipInputStream.GetNextEntry();
					zipEntry = nextEntry;
				}
				while (nextEntry != null);
			}
			finally
			{
				if (zipInputStream != null)
				{
					((IDisposable)zipInputStream).Dispose();
				}
			}
			return str1;
		}
	}
}