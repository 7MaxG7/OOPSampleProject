using System.Threading;

namespace Infrastructure
{
    public interface ICancellationTokenProvider
    {
        void Init();
        CancellationTokenSource CreateLocalCts();
    }
}