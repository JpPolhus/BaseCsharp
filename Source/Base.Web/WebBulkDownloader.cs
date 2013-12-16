using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Base.Web
{
	public interface IWebBulkDownloader
	{
		void Download(int retryCount, params string[] addresses);
		void Download(int retryCount, IEnumerable<string> addresses);

		Task<byte[]> this[string address] { get; }
		bool TryGetDownloadTask(string address, out Task<byte[]> task);
	}

	// TODO: when project is upgraded to .NET4.5 it should be som async methods on WebClient
	public class WebBulkDownloader : IWebBulkDownloader
	{
		private readonly IDictionary<string, Task<byte[]>> _downloadTasks = new Dictionary<string, Task<byte[]>>();

		public void Download(int retryCount, params string[] addresses)
		{
			Download(retryCount, (IEnumerable<string>)addresses);
		}

		public void Download(int retryCount, IEnumerable<string> addresses)
		{
			foreach (var address in addresses)
			{
				string v = address;
				var downloadTask = Task<byte[]>.Factory.StartNew(() => Download(v));
				for (int i = 0; i < retryCount; i++)
				{
					downloadTask = downloadTask.ContinueWith(x => RetryDownload(x, v));
				}

				_downloadTasks[v] = downloadTask;
			}
		}

		public Task<byte[]> this[string address]
		{
			get { return _downloadTasks[address]; }
		}

		public bool TryGetDownloadTask(string address, out Task<byte[]> task)
		{
			return _downloadTasks.TryGetValue(address, out task);
		}

		private byte[] Download(string address)
		{
			var uri = new Uri(address);

			using (var client = new WebClient())
			{
				return client.DownloadData(uri);
			}
		}

		private byte[] RetryDownload(Task<byte[]> task, string address)
		{
			if (task.IsFaulted)
			{
				var exception = task.Exception; // Mark as handled
				return Download(address);
			}

			return task.Result;
		}
	}
}
