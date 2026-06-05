using System.Threading;
using Infrastructure.ControllersHolder;
using Zenject;

namespace Infrastructure.Wrappers
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