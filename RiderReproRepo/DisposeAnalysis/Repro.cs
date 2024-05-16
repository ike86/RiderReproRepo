using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace RiderReproRepo.DisposeAnalysis;

public sealed class Repro : IDisposable
{
    private readonly CompositeDisposable disposer = new();
    
    public Repro()
    {
        DisposableByCtor = new Disposable().DisposeWith(disposer);
        DisposableByFactory = new DisposableFactory()
            .Create()
            .DisposeWith(disposer);
    }

    public Disposable DisposableByCtor { get; }

    public Disposable DisposableByFactory { get; }

    public void Dispose() => disposer.Dispose();
}

public sealed class StaticInvokeRepro : IDisposable
{
    private readonly CompositeDisposable disposer = new();
    
    public StaticInvokeRepro()
    {
        DisposableByCtor = DisposableMixins.DisposeWith(new Disposable(), disposer);
        DisposableByFactory = DisposableMixins.DisposeWith(new DisposableFactory().Create(), disposer);
    }

    public Disposable DisposableByCtor { get; }

    public Disposable DisposableByFactory { get; }

    public void Dispose() => disposer.Dispose();
}

[MustDisposeResource]
public sealed class Disposable : IDisposable
{
    public void Dispose()
    {
    }
}

public class DisposableFactory
{
    [MustDisposeResource]
    public Disposable Create() => new Disposable();
}