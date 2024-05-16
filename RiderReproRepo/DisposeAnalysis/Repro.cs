using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace RiderReproRepo.DisposeAnalysis;

public sealed class Repro : IDisposable
{
    private readonly CompositeDisposable disposer = new();
    
    public Repro()
    {
        DisposableByCtor = 
            // positive, as 3rd-party is not decorated with [HandlesResourceDisposal]
            new Disposable()
                .DisposeWith(disposer);
    }

    public Disposable DisposableByCtor { get; }

    public void Dispose() => disposer.Dispose();
}

public sealed class StaticInvokeRepro : IDisposable
{
    private readonly CompositeDisposable disposer = new();
    
    public StaticInvokeRepro()
    {
        DisposableByCtor = 
            DisposableMixins.DisposeWith(
                // positive, as 3rd-party is not decorated with [HandlesResourceDisposal]
                new Disposable(),
                disposer);
    }

    public Disposable DisposableByCtor { get; }

    public void Dispose() => disposer.Dispose();
}

[MustDisposeResource]
public sealed class Disposable : IDisposable
{
    public void Dispose()
    {
    }
}