using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Common
{
	internal class LoadFileHelper
	{
		private HttpWebRequest request;

		private HttpLoadFile httpLoadFile;

		private int taskIndex;

		private string errMsg;

		public LoadFileHelper(HttpLoadFile httpLoadFile, int taskIndex)
		{
			this.httpLoadFile = httpLoadFile;
			this.taskIndex = taskIndex;
		}

		protected void ErrorNotify()
		{
			if (this.Error != null)
			{
				HttpLoadFileEventArgs args = new HttpLoadFileEventArgs()
				{
					TaskIndex = this.taskIndex,
					IsFinish = false,
					ErrMsg = this.errMsg,
					SourceUrl = this.httpLoadFile.SourceUrl,
					SaveFilePath = this.httpLoadFile.SaveFilePath,
					TaskCount = this.httpLoadFile.TaskCount
				};
				this.Error(this, args);
			}
		}

		protected void FinishNotify()
		{
			if (this.Finished != null)
			{
				HttpLoadFileEventArgs args = new HttpLoadFileEventArgs()
				{
					TaskIndex = this.taskIndex,
					IsFinish = true,
					ErrMsg = "No Error!",
					SourceUrl = this.httpLoadFile.SourceUrl,
					SaveFilePath = this.httpLoadFile.SaveFilePath,
					TaskCount = this.httpLoadFile.TaskCount
				};
				this.Finished(this, args);
			}
		}

		public void LoadTaskFile()
		{
			Stream ns = null;
			byte[] nbytes = new byte[512];
			int nreadsize = 0;
			FileStream fs = new FileStream(this.httpLoadFile.TaskFileList[this.taskIndex], FileMode.Create);
			try
			{
				this.request = (HttpWebRequest)WebRequest.Create(this.httpLoadFile.SourceUrl);
				this.request.AddRange(this.httpLoadFile.TaskStartList[this.taskIndex], this.httpLoadFile.TaskStartList[this.taskIndex] + this.httpLoadFile.TaskSizeList[this.taskIndex]);
				ns = this.request.GetResponse().GetResponseStream();
				for (nreadsize = ns.Read(nbytes, 0, 512); nreadsize > 0; nreadsize = ns.Read(nbytes, 0, 512))
				{
					fs.Write(nbytes, 0, nreadsize);
				}
				fs.Close();
				ns.Close();
				this.FinishNotify();
			}
			catch (HttpListenerException httpListenerException)
			{
				HttpListenerException ex = httpListenerException;
				this.errMsg = string.Concat("第[", this.taskIndex.ToString(), "]条任务线程发生错误，原因：", ex.Message);
				ns.Close();
				fs.Close();
				this.ErrorNotify();
			}
		}

		public event EventHandler<HttpLoadFileEventArgs> Error;

		public event EventHandler<HttpLoadFileEventArgs> Finished;
	}
}