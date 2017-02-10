using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Common
{
	public class HttpLoadFile
	{
		private bool isFinish = false;

		private bool[] isFinishList;

		private string[] taskFileList;

		private int[] taskStartList;

		private int[] taskSizeList;

		private string sourceUrl;

		private int taskCount;

		private string saveFilePath;

		private string errMsg;

		private HttpWebRequest request;

		private bool isMerge;

		private long fileSize = (long)0;

		public string ErrMsg
		{
			get
			{
				return this.errMsg;
			}
		}

		public bool IsFinish
		{
			get
			{
				return this.isFinish;
			}
		}

		public bool[] IsFinishList
		{
			get
			{
				return this.isFinishList;
			}
		}

		public string SaveFilePath
		{
			get
			{
				return this.saveFilePath;
			}
			set
			{
				this.saveFilePath = value;
			}
		}

		public string SourceUrl
		{
			get
			{
				return this.sourceUrl;
			}
			set
			{
				this.sourceUrl = value;
			}
		}

		public int TaskCount
		{
			get
			{
				return this.taskCount;
			}
			set
			{
				this.taskCount = value;
			}
		}

		public string[] TaskFileList
		{
			get
			{
				return this.taskFileList;
			}
		}

		public int[] TaskSizeList
		{
			get
			{
				return this.taskSizeList;
			}
		}

		public int[] TaskStartList
		{
			get
			{
				return this.taskStartList;
			}
		}

		public HttpLoadFile(string sourceUrl, int taskCount, string saveFilePath)
		{
			this.sourceUrl = sourceUrl;
			this.taskCount = taskCount;
			this.saveFilePath = saveFilePath;
		}

		protected void AllErrorNotify()
		{
			if (this.Error != null)
			{
				HttpLoadFileEventArgs args = new HttpLoadFileEventArgs()
				{
					TaskCount = this.taskCount,
					SourceUrl = this.sourceUrl,
					SaveFilePath = this.saveFilePath,
					IsFinish = false,
					ErrMsg = this.errMsg
				};
				this.Error(this, args);
			}
		}

		protected void AllFinishNotify()
		{
			if (this.AllFinished != null)
			{
				HttpLoadFileEventArgs args = new HttpLoadFileEventArgs()
				{
					TaskCount = this.taskCount,
					SourceUrl = this.sourceUrl,
					SaveFilePath = this.saveFilePath,
					IsFinish = true,
					ErrMsg = "No Error!",
					TaskIndex = this.taskCount
				};
				this.AllFinished(this, args);
			}
		}

		public bool DownLoad()
		{
			bool flag;
			try
			{
				this.InitConnection();
				this.DownLoadTask();
				this.isFinish = true;
				this.AllFinishNotify();
				flag = true;
			}
			catch (Exception exception)
			{
				this.isFinish = false;
				this.AllErrorNotify();
				flag = false;
			}
			return flag;
		}

		protected void DownLoadTask()
		{
			try
			{
				Thread[] threadList = new Thread[this.taskCount];
				LoadFileHelper[] helpers = new LoadFileHelper[this.taskCount];
				for (int j = 0; j < this.taskCount; j++)
				{
					helpers[j] = new LoadFileHelper(this, j);
					helpers[j].Error += new EventHandler<HttpLoadFileEventArgs>(this.HttpLoadFile_Error);
					helpers[j].Finished += new EventHandler<HttpLoadFileEventArgs>(this.HttpLoadFile_Finished);
					threadList[j] = new Thread(new ThreadStart(helpers[j].LoadTaskFile));
					threadList[j].Start();
				}
				(new Thread(new ThreadStart(this.MergeFile))).Start();
			}
			catch (Exception exception)
			{
				throw new Exception(string.Concat("开始多线程任务下载出错：", exception.Message));
			}
		}

		~HttpLoadFile()
		{
			GC.SuppressFinalize(this);
		}

		protected void HttpLoadFile_Error(object sender, HttpLoadFileEventArgs e)
		{
			this.errMsg = e.ErrMsg;
			this.SubErrorNotify(e);
		}

		protected void HttpLoadFile_Finished(object sender, HttpLoadFileEventArgs e)
		{
			this.isFinishList[e.TaskIndex] = e.IsFinish;
			this.SubFinishNotity(e);
		}

		protected void InitConnection()
		{
			try
			{
				this.request = (HttpWebRequest)WebRequest.Create(this.sourceUrl);
				this.fileSize = this.request.GetResponse().ContentLength;
				this.request.Abort();
				this.isFinishList = new bool[this.taskCount];
				this.taskFileList = new string[this.taskCount];
				this.taskStartList = new int[this.taskCount];
				this.taskSizeList = new int[this.taskCount];
				this.InitTaskList();
			}
			catch (HttpListenerException httpListenerException)
			{
				throw new Exception(string.Concat("无法连接目标地址,原因", httpListenerException.Message));
			}
		}

		protected void InitTaskList()
		{
			int averageTaskFileSize = (int)this.fileSize / this.taskCount;
			int lasTaskFileSize = averageTaskFileSize + (int)this.fileSize % this.taskCount;
			for (int i = 0; i < this.taskCount; i++)
			{
				this.isFinishList[i] = false;
				this.taskFileList[i] = string.Concat(i.ToString(), ".dat");
				if (i >= this.taskCount - 1)
				{
					this.taskStartList[i] = averageTaskFileSize * i;
					this.taskSizeList[i] = lasTaskFileSize - 1;
				}
				else
				{
					this.taskStartList[i] = averageTaskFileSize * i;
					this.taskSizeList[i] = averageTaskFileSize - 1;
				}
			}
		}

		protected void MergeFile()
		{
			bool flag;
			FileStream fs = null;
			FileStream fstemp = null;
			try
			{
				while (true)
				{
					flag = true;
					this.isMerge = true;
					int i = 0;
					while (i < this.taskCount)
					{
						if (this.isFinishList[i])
						{
							i++;
						}
						else
						{
							this.isMerge = false;
							Thread.Sleep(100);
							break;
						}
					}
					if (this.isMerge)
					{
						break;
					}
				}
				byte[] bytes = new byte[512];
				fs = new FileStream(this.saveFilePath, FileMode.Create);
				for (int k = 0; k < this.taskCount; k++)
				{
					fstemp = new FileStream(this.taskFileList[k], FileMode.Open);
					while (true)
					{
						flag = true;
						int readfile = fstemp.Read(bytes, 0, 512);
						if (readfile <= 0)
						{
							break;
						}
						fs.Write(bytes, 0, readfile);
					}
					fstemp.Close();
				}
				fs.Close();
			}
			catch (IOException oException)
			{
				IOException ex = oException;
				fstemp.Close();
				fs.Close();
				throw new Exception(string.Concat("合并任务文件出错，原因：", ex.Message));
			}
		}

		protected void SubErrorNotify(HttpLoadFileEventArgs args)
		{
			if (this.Error != null)
			{
				this.Error(this, args);
			}
		}

		protected void SubFinishNotity(HttpLoadFileEventArgs args)
		{
			if (this.SubFinished != null)
			{
				this.SubFinished(this, args);
			}
		}

		public event EventHandler<HttpLoadFileEventArgs> AllFinished;

		public event EventHandler<HttpLoadFileEventArgs> Error;

		public event EventHandler<HttpLoadFileEventArgs> SubFinished;
	}
}