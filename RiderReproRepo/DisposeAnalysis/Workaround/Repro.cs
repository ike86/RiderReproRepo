using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace RiderReproRepo.DisposeAnalysis.Workaround;

public sealed class Repro : IDisposable
{
    private readonly CompositeDisposable disposer = new();

    public Repro()
    {
        DisposableByCtor = 
            // false-positive, as code is decorated with [HandlesResourceDisposal]
            new Disposable()
                .DisposeWith(disposer);
    }

    public Disposable DisposableByCtor { get; }

    public void Dispose() => disposer.Dispose();
}

public sealed class AllGood : IDisposable
{
    private readonly CompositeDisposable disposer = new();

    public AllGood()
    {
        DisposableByCtor = 
            DisposableMixins.DisposeWith(
                // negative, as expected
                new Disposable(),
                disposer);
    }

    public Disposable DisposableByCtor { get; }

    public void Dispose() => disposer.Dispose();
}

public static class DisposableMixins
{
    public static T DisposeWith<T>(
        [HandlesResourceDisposal] this T item,
        CompositeDisposable compositeDisposable)
        where T : IDisposable
    {
        return System.Reactive.Disposables.DisposableMixins.DisposeWith(item, compositeDisposable);
    }
}