using System.Threading;
using Abstractions.Infrastructure;
using Zenject;

namespace Infrastructure
{
	public sealed class CancellationTokenProvider : ICancellationTokenProvider, ICleanable
	{
		private CancellationTokenSource _cts;

		[Inject]
		public CancellationTokenProvider(ICleaner cleaner)
		{
			cleaner.AddCleanable(this);
		}
		
		public void Init()
		{
			_cts = new CancellationTokenSource();
		}

		public CancellationTokenSource CreateLocalCts()
			=> CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);

		public void CleanUp()
		{
			_cts.Cancel();
			_cts.Dispose();
		}
	}
}