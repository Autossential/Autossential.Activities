using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class AddRangeToCollectionTests
    {
        // ─── Helpers ─────────────────────────────────────────────────────────────

        private static void Invoke<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            WorkflowInvoker.Invoke(new AddRangeToCollection<T>(), new Dictionary<string, object>
            {
                { "Collection", collection },
                { "Items", items }
            });
        }

        // ─── Happy path ───────────────────────────────────────────────────────────
        [Fact]
        public void Invoke_StringItems_AddsAllToCollection()
        {
            var collection = new List<string> { "existing" };
            Invoke(collection, ["a", "b", "c"]);

            Assert.Equal(new[] { "existing", "a", "b", "c" }, collection);
        }

        [Fact]
        public void Invoke_IntItems_AddsAllToCollection()
        {
            var collection = new List<int>();
            Invoke(collection, [1, 2, 3]);

            Assert.Equal(new[] { 1, 2, 3 }, collection);
        }

        [Fact]
        public void Invoke_EmptyItems_CollectionUnchanged()
        {
            var collection = new List<string> { "only" };
            Invoke(collection, []);

            Assert.Single(collection);
            Assert.Equal("only", collection.First());
        }

        [Fact]
        public void Invoke_ItemsWithNulls_AddsNullsToCollection()
        {
            var collection = new List<string>();
            Invoke(collection, ["a", null!, "b"]);

            Assert.Equal(3, collection.Count);
            Assert.Null(collection[1]);
        }

        [Fact]
        public void Invoke_NonListCollection_StillAddsItems()
        {
            // Ensures the activity works with any ICollection<T>, not just List<T>
            ICollection<int> collection = new HashSet<int> { 1 };
            Invoke(collection, [2, 3]);

            Assert.Equal(3, collection.Count);
            Assert.Contains(2, collection);
            Assert.Contains(3, collection);
        }

        [Fact]
        public void Invoke_ItemsAsLazyEnumerable_EnumeratesAndAddsAll()
        {
            static IEnumerable<int> Lazy()
            {
                yield return 10;
                yield return 20;
            }

            var collection = new List<int>();
            Invoke(collection, Lazy());

            Assert.Equal(new[] { 10, 20 }, collection);
        }

        // ─── Null arguments ───────────────────────────────────────────────────────

        [Fact]
        public void Invoke_NullCollection_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new AddRangeToCollection<string>(), new Dictionary<string, object>
            {
                { "Collection", null! },
                { "Items", new string[] { "a" } }
            }));
        }

        [Fact]
        public void Invoke_NullItems_ThrowsInvalidOperationException()
        {
            var list = new List<string>();
            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new AddRangeToCollection<string>(), new Dictionary<string, object>
            {
                { "Collection", list },
                { "Items", null! }
            }));
        }
    }
}