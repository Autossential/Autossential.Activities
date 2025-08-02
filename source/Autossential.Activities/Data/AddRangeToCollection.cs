using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class AddRangeToCollection : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument Collection { get; set; }

        [RequiredArgument]
        public InArgument Items { get; set; }

        public bool AutoInstantiate { get; set; } = true;

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            Type collectionType = null;
            if (Collection is not null)
            {
                var argType = Collection.ArgumentType;
                if (argType.IsGenericType && typeof(ICollection<>).MakeGenericType(argType.GenericTypeArguments[0]).IsAssignableFrom(argType))
                {
                    metadata.AddRuntimeArgument(Collection, argType, nameof(Collection), true);
                    collectionType = argType.GenericTypeArguments[0];
                }
                else
                {
                    metadata.AddValidationError(ResourcesFn.Validation_DerivedTypeErrorFormat(nameof(Collection), "ICollection<T>"));
                }
            }

            if (Items is not null)
            {
                var argType = Items.ArgumentType;
                Type itemsElementType = null;
                if (argType.IsGenericType)
                {
                    itemsElementType = argType.GenericTypeArguments[0];
                }
                else if (argType.IsArray)
                {
                    itemsElementType = argType.GetElementType();
                }

                // Use o tipo do elemento da coleção, se disponível
                var targetElementType = collectionType ?? itemsElementType;

                // Verifique se o tipo dos itens é atribuível ao tipo da coleção
                if (itemsElementType != null && targetElementType != null && targetElementType.IsAssignableFrom(itemsElementType))
                {
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(argType))
                    {
                        metadata.AddRuntimeArgument(Items, argType, nameof(Items), true);
                    }
                    else
                    {
                        metadata.AddValidationError(ResourcesFn.Validation_DerivedTypeErrorFormat(nameof(Items), $"IEnumerable<{targetElementType.Name}>"));
                    }
                }
                else
                {
                    metadata.AddValidationError(ResourcesFn.Validation_DerivedTypeErrorFormat(nameof(Items), $"IEnumerable<{targetElementType?.Name ?? "T"}>"));
                }
            }

        }

        protected override void Execute(CodeActivityContext context)
        {
            var data = Collection.Get(context);
            if (data == null && AutoInstantiate)
            {
                try
                {
                    data = Activator.CreateInstance(Collection.ArgumentType);
                }
                catch { }
            }

            if (data == null)
                throw new ArgumentNullException(ResourcesFn.Validation_InstanceIsNullFormat(nameof(Collection)));

            var items = Items.Get(context) ?? throw new ArgumentNullException(nameof(Items));

            var elementType = Collection.ArgumentType.GenericTypeArguments[0];
            var itemsEnumerable = ((System.Collections.IEnumerable)items).Cast<object>();

            var method = typeof(AddRangeToCollection)
                .GetMethod(nameof(AddRangeInternal), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .MakeGenericMethod(elementType);

            method.Invoke(this, [data, itemsEnumerable]);
            Collection.Set(context, data);
        }

        private static void AddRangeInternal<T>(ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}