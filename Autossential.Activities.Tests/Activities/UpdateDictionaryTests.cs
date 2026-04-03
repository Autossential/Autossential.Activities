using System.Activities;
using Autossential.Activities.Core;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class UpdateDictionaryTests
    {
        // ─── Helper ──────────────────────────────────────────────────────────────

        private static void Invoke<TKey, TValue>(Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> entries)
        {
            WorkflowInvoker.Invoke(new UpdateDictionary<TKey, TValue>(), new Dictionary<string, object>
            {
                { "Dictionary", dictionary },
                { "Entries", entries }
            });
        }

        // ─── Happy path ───────────────────────────────────────────────────────────

        [Fact]
        public void Invoke_NewKeys_AddsEntriesToDictionary()
        {
            var dict = new Dictionary<string, int> { ["a"] = 1 };
            Invoke(dict, new Dictionary<string, int> { ["b"] = 2, ["c"] = 3 });

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["b"]);
            Assert.Equal(3, dict["c"]);
        }

        [Fact]
        public void Invoke_ExistingKeys_OverwritesValues()
        {
            var dict = new Dictionary<string, string> { ["x"] = "old" };
            Invoke(dict, new Dictionary<string, string> { ["x"] = "new" });

            Assert.Single(dict);
            Assert.Equal("new", dict["x"]);
        }

        [Fact]
        public void Invoke_MixedKeys_AddsAndOverwritesCorrectly()
        {
            var dict = new Dictionary<string, int> { ["keep"] = 1, ["overwrite"] = 2 };
            Invoke(dict, new Dictionary<string, int> { ["overwrite"] = 99, ["new"] = 3 });

            Assert.Equal(1, dict["keep"]);
            Assert.Equal(99, dict["overwrite"]);
            Assert.Equal(3, dict["new"]);
        }

        [Fact]
        public void Invoke_NullEntries_LeaveDictionaryUnchanged()
        {
            var dict = new Dictionary<string, int> { ["a"] = 1 };
            Invoke(dict, null);

            Assert.Single(dict);
            Assert.Equal(1, dict["a"]);
        }

        [Fact]
        public void Invoke_EmptyEntries_LeaveDictionaryUnchanged()
        {
            var dict = new Dictionary<string, int> { ["a"] = 1 };
            Invoke(dict, new Dictionary<string, int>());

            Assert.Single(dict);
        }

        [Fact]
        public void Invoke_IntKeys_WorksCorrectly()
        {
            var dict = new Dictionary<int, string> { [1] = "one" };
            Invoke(dict, new Dictionary<int, string> { [2] = "two", [1] = "ONE" });

            Assert.Equal("ONE", dict[1]);
            Assert.Equal("two", dict[2]);
        }

        // ─── Null dictionary ──────────────────────────────────────────────────────

        [Fact]
        public void Invoke_NullDictionary_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Invoke<string, int>(null, new Dictionary<string, int> { ["a"] = 1 }));
        }
    }
}