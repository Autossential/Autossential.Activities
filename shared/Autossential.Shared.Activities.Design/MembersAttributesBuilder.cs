using System;
using System.Activities.Presentation.Metadata;
using System.Linq.Expressions;

namespace Autossential.Shared.Activities.Design
{
    public abstract class MembersAttributesBuilder
    {
        protected readonly AttributeTableBuilder _tableBuilder;
        protected readonly Type _activityType;

        protected MembersAttributesBuilder(AttributeTableBuilder tableBuilder, Type activityType)
        {
            _tableBuilder = tableBuilder;
            _activityType = activityType;
        }
        public void Register<TActivity>(Attribute attribute, params Expression<Func<TActivity, object>>[] members)
        {
            foreach (var member in members)
                _tableBuilder.AddCustomAttributes(_activityType, GetMemberName(member), attribute);
        }

        public void Register<TActivity>(Expression<Func<TActivity, object>> member, params Attribute[] attributes)
        {
            _tableBuilder.AddCustomAttributes(_activityType, GetMemberName(member), attributes);
        }

        protected static string GetMemberName<TActivity>(Expression<Func<TActivity, object>> property)
        {
            var me = property.Body as MemberExpression ?? (property.Body as UnaryExpression)?.Operand as MemberExpression;
            return me.Member.Name;
        }
    }

    public sealed class MembersAttributesBuilder<TActivity> : MembersAttributesBuilder where TActivity : class
    {

        public MembersAttributesBuilder(AttributeTableBuilder tableBuilder) : base(tableBuilder, typeof(TActivity))
        {
        }

        public void Register(Attribute attribute, params Expression<Func<TActivity, object>>[] members) => Register<TActivity>(attribute, members);

        public void Register(Expression<Func<TActivity, object>> member, params Attribute[] attributes) => Register<TActivity>(member, attributes);
    }
}