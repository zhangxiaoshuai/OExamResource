using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace Common
{
	public static class FileHelper
	{
		public static void DeleteFile(string fullName)
		{
			if (fullName.Length != 0)
			{
				if (File.Exists(Path.GetFullPath(fullName)))
				{
					File.Delete(Path.GetFullPath(fullName));
				}
			}
		}

		public static bool IsExists(string fullpath)
		{
			return File.Exists(fullpath);
		}

		public static string ResizeImage(HttpPostedFileBase file, int width, int height, string path, string fileName)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string imagePath = string.Concat(path, Path.DirectorySeparatorChar, fileName);
			FileStream stream = new FileStream(Path.GetFullPath(imagePath), FileMode.OpenOrCreate);
			Image OrigImage = Image.FromStream(file.InputStream);
			Bitmap TempBitmap = new Bitmap(width, height);
			Graphics NewImage = Graphics.FromImage(TempBitmap);
			NewImage.CompositingQuality = CompositingQuality.HighQuality;
			NewImage.SmoothingMode = SmoothingMode.HighQuality;
			NewImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
			Rectangle imageRectangle = new Rectangle(0, 0, width, height);
			NewImage.DrawImage(OrigImage, imageRectangle);
			TempBitmap.Save(stream, OrigImage.RawFormat);
			NewImage.Dispose();
			TempBitmap.Dispose();
			OrigImage.Dispose();
			stream.Close();
			stream.Dispose();
			return imagePath;
		}

		public static string ResizeImage(HttpPostedFileBase file, int width, int height, string path)
		{
			string str = FileHelper.ResizeImage(file, width, height, path, file.FileName);
			return str;
		}

		public static string UploadFile(HttpPostedFileBase file, string path)
		{
			return FileHelper.UploadFile(file, path, file.FileName);
		}

		public static string UploadFile(HttpPostedFileBase file, string path, string fileName)
		{
			string str;
			if (null == file)
			{
				str = "";
			}
			else if (file.ContentLength <= 0)
			{
				str = "";
			}
			else if (null != Path.GetExtension(file.FileName))
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				path = string.Concat(path, Path.DirectorySeparatorChar, fileName);
				file.SaveAs(Path.GetFullPath(path));
				str = path;
			}
			else
			{
				str = "";
			}
			return str;
		}
	}
}