using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autossential.Shared
{
    internal static class ActivityMetadataExtensions
    {
        public static void AddRuntimeArgument(
          this ActivityMetadata metadata,
          Argument messageArgument,
          string runtimeArgumentName,
          Type runtimeArgumentType,
          ArgumentDirection runtimeArgumentDirection,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(runtimeArgumentName, runtimeArgumentType, runtimeArgumentDirection, isRequired, overloadGroupNames);
            metadata.Bind(messageArgument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument(
          this CodeActivityMetadata metadata,
          Argument messageArgument,
          string runtimeArgumentName,
          Type runtimeArgumentType,
          ArgumentDirection runtimeArgumentDirection,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(runtimeArgumentName, runtimeArgumentType, runtimeArgumentDirection, isRequired, overloadGroupNames);
            metadata.Bind(messageArgument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument(
          this NativeActivityMetadata metadata,
          Argument messageArgument,
          string runtimeArgumentName,
          Type runtimeArgumentType,
          ArgumentDirection runtimeArgumentDirection,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(runtimeArgumentName, runtimeArgumentType, runtimeArgumentDirection, isRequired, overloadGroupNames);
            metadata.Bind(messageArgument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.In, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.Out, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          InOutArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this ActivityMetadata metadata,
          InOutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.InOut, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.In, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.Out, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          InOutArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this CodeActivityMetadata metadata,
          InOutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.InOut, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this NativeActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this NativeActivityMetadata metadata,
          InArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.In, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this NativeActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired)
        {
            metadata.AddRuntimeArgument<T>(argument, argumentName, isRequired, (List<string>)null);
        }

        public static void AddRuntimeArgument<T>(
          this NativeActivityMetadata metadata,
          OutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.Out, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }

        public static void AddRuntimeArgument<T>(
          this NativeActivityMetadata metadata,
          InOutArgument<T> argument,
          string argumentName,
          bool isRequired,
          List<string> overloadGroupNames)
        {
            RuntimeArgument runtimeArgument = new RuntimeArgument(argumentName, typeof(T), ArgumentDirection.InOut, isRequired, overloadGroupNames);
            metadata.Bind((Argument)argument, runtimeArgument);
            metadata.AddArgument(runtimeArgument);
        }
    }
}
