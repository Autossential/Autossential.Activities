using Autossential.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Activities.Statements;

namespace Autossential.Tests
{
    [TestClass]
    public class CultureScopeTests
    {
        [TestMethod]
        [DataRow(1.75, "pt-BR", "R$ 1,75")]
        [DataRow(1.75, "en-US", "$1.75")]
        [DataRow(1.75, "nb-NO", "kr 1,75")]
        public void Currency(double value, string culture, string expected)
        {
            var dyn = new DynamicActivity<string>();
            dyn.Implementation = () => new CultureScope
            {
                CultureName = culture,
                Body = new ActivityAction
                {
                    Handler = new Assign<string>
                    {
                        To = new OutArgument<string>(env => dyn.Result.Get(env)),
                        Value = new InArgument<string>(_ => value.ToString("c"))
                    }
                }
            };

            var result = WorkflowInvoker.Invoke(dyn);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("pt-BR", "28/12/2021 00:00:00")]
        [DataRow("en-US", "12/28/2021 12:00:00 AM")]
        [DataRow("nb-NO", "28.12.2021 00:00:00")]
        public void DateTime(string culture, string expected)
        {
            var date = new DateTime(2021, 12, 28);
            var dyn = new DynamicActivity<string>();
            dyn.Implementation = () => new CultureScope
            {
                CultureName = culture,
                Body = new ActivityAction
                {
                    Handler = new Assign<string>
                    {
                        To = new OutArgument<string>(env => dyn.Result.Get(env)),
                        Value = new InArgument<string>(_ => date.ToString())
                    }
                }
            };

            var result = WorkflowInvoker.Invoke(dyn);
            Assert.AreEqual(expected, result);
        }
    }
}
