using System;

namespace Common
{
	public class HttpLoadFileEventArgs : EventArgs
	{
		private int taskCount;

		private string sourceUrl;

		private string saveFilePath;

		private string errMsg;

		private bool isFinish;

		private int taskIndex;

		public string ErrMsg
		{
			get
			{
				return this.errMsg;
			}
			set
			{
				this.errMsg = value;
			}
		}

		public bool IsFinish
		{
			get
			{
				return this.isFinish;
			}
			set
			{
				this.isFinish = value;
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

		public int TaskIndex
		{
			get
			{
				return this.taskIndex;
			}
			set
			{
				this.taskIndex = value;
			}
		}

		public HttpLoadFileEventArgs()
		{
		}
	}
}