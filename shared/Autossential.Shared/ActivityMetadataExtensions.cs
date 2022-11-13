using System;
using System.Activities;
using System.Activities.Validation;

namespace Autossential.Shared
{
    internal static class ActivityMetadataExtensions
    {
        private static void AddRuntimeArgument(this
            CodeActivityMetadata metadata,
            Argument argument,
            Type argumentType,
            ArgumentDirection direction,
            string argumentName,
            bool isRequired)
        {
            var arg = new RuntimeArgument(argumentName, argumentType, direction, isRequired);
            metadata.Bind(argument, arg);
            metadata.AddArgument(arg);
        }

        public static void AddRuntimeArgument<T>(this CodeActivityMetadata metadata, InArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.In, argumentName, isRequired);

        public static void AddRuntimeArgument(this CodeActivityMetadata metadata, InArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.In, argumentName, isRequired);

        public static void AddRuntimeArgument<T>(this CodeActivityMetadata metadata, InOutArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.InOut, argumentName, isRequired);

        public static void AddRuntimeArgument(this CodeActivityMetadata metadata, InOutArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.InOut, argumentName, isRequired);

        public static void AddRuntimeArgument<T>(this CodeActivityMetadata metadata, OutArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.Out, argumentName, isRequired);

        public static void AddRuntimeArgument(this CodeActivityMetadata metadata, OutArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.Out, argumentName, isRequired);

        public static void AddValidationWarning(this CodeActivityMetadata metadata, string message)
        {
            metadata.AddValidationError(new ValidationError(message, true));
        }

        private static void AddRuntimeArgument(
            NativeActivityMetadata metadata,
            Argument argument,
            Type argumentType,
            ArgumentDirection direction,
            string argumentName,
            bool isRequired)
        {
            var arg = new RuntimeArgument(argumentName, argumentType, direction, isRequired);
            metadata.Bind(argument, arg);
            metadata.AddArgument(arg);
        }

        public static void AddRuntimeArgument<T>(this NativeActivityMetadata metadata, InArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.In, argumentName, isRequired);

        public static void AddRuntimeArgument(this NativeActivityMetadata metadata, InArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.In, argumentName, isRequired);

        public static void AddRuntimeArgument<T>(this NativeActivityMetadata metadata, InOutArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.InOut, argumentName, isRequired);

        public static void AddRuntimeArgument(this NativeActivityMetadata metadata, InOutArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.InOut, argumentName, isRequired);

        public static void AddRuntimeArgument<T>(this NativeActivityMetadata metadata, OutArgument<T> argument, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, typeof(T), ArgumentDirection.Out, argumentName, isRequired);

        public static void AddRuntimeArgument(this NativeActivityMetadata metadata, OutArgument argument, Type argumentType, string argumentName, bool isRequired) => AddRuntimeArgument(metadata, argument, argumentType, ArgumentDirection.Out, argumentName, isRequired);
    }
}