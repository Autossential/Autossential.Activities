using Autossential.Activities;
using System.Activities;

public class AddRangeToCollectionTests
{
    private static void Invoke<T>(ICollection<T> collection, IEnumerable<T> items)
    {
        WorkflowInvoker.Invoke(new AddRangeToCollection<T>(), new Dictionary<string, object>
        {
            { "Collection", collection },
            { "Items", items }
        });
    }


    [Test]
    public async Task AddsAllToCollection_WhenStringItems()
    {
        var collection = new List<string> { "existing" };
        Invoke(collection, ["a", "b", "c"]);

        await Assert.That(collection).IsEquivalentTo(new[] { "existing", "a", "b", "c" });
    }

    [Test]
    public async Task AddsAllToCollection_WhenIntItems()
    {
        var collection = new List<int>();
        Invoke(collection, [1, 2, 3]);

        await Assert.That(collection).IsEquivalentTo(new[] { 1, 2, 3 });
    }

    [Test]
    public async Task CollectionUnchanged_WhenEmptyItems()
    {
        var collection = new List<string> { "only" };
        Invoke(collection, []);

        await Assert.That(collection).Count().IsEqualTo(1);
        await Assert.That(collection.First()).IsEqualTo("only");
    }

    [Test]
    public async Task AddsNullsToCollection_WhenItemsWithNulls()
    {
        var collection = new List<string>();
        Invoke(collection, ["a", null!, "b"]);

        await Assert.That(collection).Count().IsEqualTo(3);
        await Assert.That(collection[1]).IsNull();
    }

    [Test]
    public async Task StillAddsItems_WhenNonListCollection()
    {
        // Ensures the activity works with any ICollection<T>, not just List<T>
        ICollection<int> collection = new HashSet<int> { 1 };
        Invoke(collection, [2, 3]);

        await Assert.That(collection).Count().IsEqualTo(3);
        await Assert.That(collection).Contains(2);
        await Assert.That(collection).Contains(3);
    }

    [Test]
    public async Task EnumeratesAndAddsAll_WhenItemsAsLazyEnumerable()
    {
        static IEnumerable<int> Lazy()
        {
            yield return 10;
            yield return 20;
        }

        var collection = new List<int>();
        Invoke(collection, Lazy());

        await Assert.That(collection).IsEquivalentTo([10, 20]);
    }

    [Test]
    public async Task ThrowsInvalidOperationException_WhenNullCollection()
    {
        await Assert.That(() => WorkflowInvoker.Invoke(new AddRangeToCollection<string>(), new Dictionary<string, object>
        {
            { "Collection", null! },
            { "Items", new string[] { "a" } }
        })).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task ThrowsInvalidOperationException_WhenNullItems()
    {
        var list = new List<string>();
        await Assert.That(() => WorkflowInvoker.Invoke(new AddRangeToCollection<string>(), new Dictionary<string, object>
        {
            { "Collection", list },
            { "Items", null! }
        })).Throws<InvalidOperationException>();
    }
}