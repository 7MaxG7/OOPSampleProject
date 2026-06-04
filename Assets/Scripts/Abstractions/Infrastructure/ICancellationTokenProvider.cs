using System.Threading;

namespace Abstractions.Infrastructure
{
    public interface ICancellationTokenProvider
    {
        void Init();
        CancellationTokenSource CreateLocalCts();
    }
}