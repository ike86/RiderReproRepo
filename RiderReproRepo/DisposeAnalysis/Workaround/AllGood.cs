using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace RiderReproRepo.DisposeAnalysis.Workaround;

public class Repro
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

public class AllGood
{
    private readonly CompositeDisposable disposer = new();

    public AllGood()
    {
        DisposableByCtor = DisposableMixins.DisposeWith(new Disposable(), disposer);
        DisposableByFactory = DisposableMixins.DisposeWith(new DisposableFactory().Create(), disposer);
    }

    public Disposable DisposableByCtor { get; }

    public Disposable DisposableByFactory { get; }

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