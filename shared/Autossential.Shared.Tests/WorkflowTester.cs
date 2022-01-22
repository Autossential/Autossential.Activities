using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Autossential.Shared.Tests
{
    public class WorkflowTester
    {
        private static void Compile(Activity activity)
        {
            string activityName = activity.GetType().ToString();
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());
            var settings = new TextExpressionCompilerSettings
            {
                Activity = activity,
                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = false,
#if NET5_0
                Compiler = new CSharpAotCompiler()
#endif
            };

            var results = new TextExpressionCompiler(settings).Compile();
            if (results.HasErrors)
            {
                foreach (var m in results.CompilerMessages)
                    System.Diagnostics.Debug.WriteLine(m.Message);

                throw new Exception("Compilation failed.");
            }

            var compiledExpressionRoot = Activator.CreateInstance(results.ResultType, new object[] { activity }) as ICompiledExpressionRoot;
            CompiledExpressionInvoker.SetCompiledExpressionRoot(activity, compiledExpressionRoot);
        }


        public static T Invoke<T>(Activity<T> activity, IDictionary<string, object> args = null)
        {
            if (args == null)
                return WorkflowInvoker.Invoke(activity);

            return WorkflowInvoker.Invoke(activity, args);
        }

        public static IDictionary<string, object> Invoke(Activity activity, IDictionary<string, object> args = null)
        {
            if (args == null)
                return WorkflowInvoker.Invoke(activity);

            return WorkflowInvoker.Invoke(activity, args);
        }

        public static T CompileAndInvoke<T>(Activity<T> activity, IDictionary<string, object> args = null)
        {
            Compile(activity);
            return Invoke(activity, args);
        }

        public static ActivityResult<T> Run<T>(T activity, IDictionary<string, object> args = null) where T : Activity
        {
            return new ActivityResult<T>(Invoke(activity, args));
        }

        public static ActivityResult<T> CompileAndRun<T>(T activity, IDictionary<string, object> args = null) where T : Activity
        {
            Compile(activity);
            return new ActivityResult<T>(Invoke(activity, args));
        }
    }

    public class ActivityResult<T> where T : Activity
    {
        private readonly IDictionary<string, object> _results;
        internal ActivityResult(IDictionary<string, object> results)
        {
            _results = results;
        }
        public object Get(Expression<Func<T, object>> member)
            => _results[(member.Body as MemberExpression).Member.Name];
    }
}