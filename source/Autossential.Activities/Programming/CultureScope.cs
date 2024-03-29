﻿using Autossential.Shared.Activities.Base;
using System.Activities;
using System.Globalization;
using System.Threading;

namespace Autossential.Activities
{
    public sealed class CultureScope : ScopeActivity
    {
        [RequiredArgument]
        public InArgument<string> CultureName { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            if (Body == null)
                return;

            context.Properties.Add(typeof(CultureScopeHandle).FullName, new CultureScopeHandle(CultureName.Get(context)));
            context.ScheduleAction(Body);
        }

        private class CultureScopeHandle : Handle, IExecutionProperty
        {
            private readonly CultureInfo _newCulture;
            private CultureInfo _originalCulture;
            public CultureScopeHandle(string cultureName)
            {
                _newCulture = CultureInfo.GetCultureInfo(cultureName);
            }

            public void CleanupWorkflowThread()
            {
                Thread.CurrentThread.CurrentCulture = _originalCulture;
            }

            public void SetupWorkflowThread()
            {
                _originalCulture = CultureInfo.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = _newCulture;
            }
        }
    }
}