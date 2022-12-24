using Autossential.Activities.Design.PropertyEditors;
using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

namespace Autossential.Shared.Activities.Design
{
    public class ActivitiesTableBuilder : AttributeTableBuilder
    {
        private readonly HashSet<Type> _activityTypes = new HashSet<Type>();
        private ResourceManager _resourceManager;

        public ActivitiesTableBuilder(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public ActivitiesTableBuilder Add<TActivity, TActivityDesigner>(params Attribute[] attributes)
            => Add(typeof(TActivity), typeof(TActivityDesigner), attributes);

        public ActivitiesTableBuilder Add(Type activityType, Type activityTypeDesigner, params Attribute[] attributes)
        {
            _activityTypes.Add(activityType);

            Attribute[] attrs;

            if (activityType.IsGenericType)
            {
                attrs = new Attribute[attributes.Length + 2];
                attrs[1] = new DefaultTypeArgumentAttribute(typeof(object));
            }
            else
            {
                attrs = new Attribute[attributes.Length + 1];
            }

            attrs[0] = new DesignerAttribute(activityTypeDesigner);

            Array.Copy(attributes, 0, attrs, attrs.Length - attributes.Length, attributes.Length);
            AddCustomAttributes(activityType, attrs);
            return this;
        }

        public ActivitiesTableBuilder AddToMember(Type ownerType, string memberName, params Attribute[] attributes)
        {
            AddCustomAttributes(ownerType, memberName, attributes);
            return this;
        }

        public ActivitiesTableBuilder AddToMembers(Type ownerType, Attribute attribute, params string[] memberNames)
        {
            foreach (var memberName in memberNames)
                AddToMember(ownerType, memberName, attribute);

            return this;
        }

        public ActivitiesTableBuilder AddToMember<TOwner>(Expression<Func<TOwner, object>> member, params Attribute[] attributes)
        {
            AddToMember(typeof(TOwner), GetMemberName(member), attributes);
            return this;
        }

        public ActivitiesTableBuilder AddToMembers<TOwner>(Attribute attribute, params Expression<Func<TOwner, object>>[] members)
        {
            foreach (var member in members)
                AddToMember(member, attribute);

            return this;
        }

        public ActivitiesTableBuilder Obsolete<TActivity>()
        {
            AddCustomAttributes(typeof(TActivity), new BrowsableAttribute(false));
            AddCustomAttributes(typeof(TActivity), new ObsoleteAttribute());
            return this;
        }

        private static string GetMemberName<TActivity>(Expression<Func<TActivity, object>> property)
        {
            var me = property.Body as MemberExpression ?? (property.Body as UnaryExpression)?.Operand as MemberExpression;
            return me.Member.Name;
        }
        public void Commit()
        {
            ApplyCommonAttributes();
            MetadataStore.AddAttributeTable(CreateTable());
        }

        private bool TryGetFromResource(string key, out string value)
        {
            value = _resourceManager.GetString(key);
            return value != null;
        }

        private static string GetDisplayNameKey(string activityName, string memberName = null) => activityName + (string.IsNullOrEmpty(memberName) ? "" : "_" + memberName) + "_DisplayName";
        private static string GetDescriptionKey(string activityName, string memberName = null) => activityName + (string.IsNullOrEmpty(memberName) ? "" : "_" + memberName) + "_Description";

        private void ApplyDefaultEditorAttribute(Type activityType, PropertyInfo property)
        {
            var baseType = property.PropertyType.BaseType;
            if (baseType != typeof(InArgument) && baseType != typeof(OutArgument) && baseType != typeof(InOutArgument) && !property.PropertyType.IsGenericType)
                return;

            var argType = property.PropertyType.GetGenericArgumentType();
            if (argType == typeof(bool))
                AddCustomAttributes(activityType, property, new EditorAttribute(typeof(BooleanPropertyEditor), typeof(DialogPropertyValueEditor)));
        }

        private bool AddCommonAttributes(Type activityType, PropertyInfo property)
        {
            var name = property.Name;
            if (name == "ContinueOnError" || name == "Timeout" || name == "DisplayName")
            {
                if (name != "DisplayName")
                    AddCustomAttributes(property.PropertyType, new DescriptionAttribute(_resourceManager.GetString("Common_" + name)));

                return true;
            }

            return false;
        }

        private void ApplyCommonAttributes()
        {
            foreach (var activityType in _activityTypes)
            {
                var activityName = activityType.IsGenericType ? activityType.Name.Substring(0, activityType.Name.IndexOf('`')) : activityType.Name;
                if (TryGetFromResource(GetDisplayNameKey(activityName), out string value))
                    AddCustomAttributes(activityType, new DisplayNameAttribute(value));

                if (TryGetFromResource(GetDescriptionKey(activityName), out value))
                    AddCustomAttributes(activityType, new DescriptionAttribute(value));

                foreach (var property in activityType.GetProperties())
                {
                    ApplyDefaultEditorAttribute(activityType, property);

                    if (AddCommonAttributes(activityType, property))
                        continue;

                    var propName = property.Name;
                    var propType = property.PropertyType;


                    // DISPLAY NAME ATTRIBUTES

                    if (TryGetFromResource(GetDisplayNameKey(activityName, propName), out value))
                    {
                        AddCustomAttributes(activityType, property, new DisplayNameAttribute(value));
                    }
                    else if (propName.StartsWith("Input") && propName.Length > 5)
                    {
                        AddCustomAttributes(activityType, property, new DisplayNameAttribute(propName.Substring(5)));
                    }
                    else if (propName.StartsWith("Output") && propName.Length > 6)
                    {
                        AddCustomAttributes(activityType, property, new DisplayNameAttribute(propName.Substring(6)));
                    }


                    // DESCRIPTION ATTRIBUTES

                    if (TryGetFromResource(GetDescriptionKey(activityName, propName), out value))
                        AddCustomAttributes(activityType, property, new DescriptionAttribute(value));


                    // CATEGORY ATTRIBUTES

                    if (typeof(InArgument).IsAssignableFrom(propType))
                    {
                        AddCustomAttributes(activityType, property, new CategoryAttribute(_resourceManager.GetString("Input_Category")));
                        continue;
                    }

                    if (typeof(OutArgument).IsAssignableFrom(propType))
                    {
                        AddCustomAttributes(activityType, property, new CategoryAttribute(_resourceManager.GetString("Output_Category")));
                        continue;
                    }

                    if (typeof(InOutArgument).IsAssignableFrom(propType))
                    {
                        AddCustomAttributes(activityType, property, new CategoryAttribute(_resourceManager.GetString("InputOutput_Category")));
                        continue;
                    }

                    AddCustomAttributes(activityType, property, new CategoryAttribute(_resourceManager.GetString("Options_Category")));
                }
            }
        }
    }

    //public sealed class ActivitiesAttributesBuilder
    //{
    //    private readonly AttributeTableBuilder _tableBuilder = new AttributeTableBuilder();
    //    private CategoryAttribute _inCategory;
    //    private CategoryAttribute _outCategory;
    //    private CategoryAttribute _inOutCategory;
    //    private CategoryAttribute _optionsCategory;
    //    private readonly ResourceManager _resourcesManager;

    //    // CUSTOM EDITOR ATTRIBUTES
    //    private EditorAttribute _booleanPropertyEditorAttribute = new EditorAttribute(typeof(BooleanPropertyEditor), typeof(DialogPropertyValueEditor));

    //    private ActivitiesAttributesBuilder(ResourceManager manager)
    //    {
    //        _resourcesManager = manager;
    //    }

    //    public static void Build(ResourceManager manager, Action<ActivitiesAttributesBuilder> builder)
    //    {
    //        var activitesAttributesBuilder = new ActivitiesAttributesBuilder(manager);
    //        builder(activitesAttributesBuilder);
    //        activitesAttributesBuilder.ValidateAndCreate();
    //    }

    //    public ActivitiesAttributesBuilder Register(Type activityType, Type activityDesignerType, Attribute[] activityAttributes)
    //    {
    //        _tableBuilder.AddCustomAttributes(activityType, activityAttributes);
    //        _tableBuilder.AddCustomAttributes(activityType, new DesignerAttribute(activityDesignerType));
    //        ApplyCommonAttributes(activityType);
    //        return this;
    //    }

    //    public ActivitiesAttributesBuilder Register<TActivity, TActivityDesigner>(Attribute[] activityAttributes, params Action<MembersAttributesBuilder<TActivity>>[] membersAttributesBuilder)
    //        where TActivity : class
    //        where TActivityDesigner : class
    //    {
    //        var activityType = typeof(TActivity);
    //        _tableBuilder.AddCustomAttributes(activityType, activityAttributes);
    //        _tableBuilder.AddCustomAttributes(activityType, new DesignerAttribute(typeof(TActivityDesigner)));
    //        if (membersAttributesBuilder.Length > 0)
    //        {
    //            var amb = new MembersAttributesBuilder<TActivity>(_tableBuilder);
    //            foreach (var reg in membersAttributesBuilder)
    //                reg(amb);
    //        }
    //        ApplyCommonAttributes(activityType);
    //        return this;
    //    }

    //    public ActivitiesAttributesBuilder Obsolete<TActivity>(string obsoleteMessage = null)
    //    {
    //        _tableBuilder.AddCustomAttributes(typeof(TActivity), new Attribute[]
    //        {
    //            new BrowsableAttribute(false),
    //            new ObsoleteAttribute(obsoleteMessage)
    //        });
    //        return this;
    //    }

    //    public ActivitiesAttributesBuilder RegisterToMember(Attribute attribute, string memberName, IEnumerable<Type> activityTypes)
    //    {
    //        foreach (var activityType in activityTypes)
    //            _tableBuilder.AddCustomAttributes(activityType, memberName, attribute);

    //        return this;
    //    }

    //    private void ValidateAndCreate()
    //    {
    //        _tableBuilder.ValidateTable();
    //        MetadataStore.AddAttributeTable(_tableBuilder.CreateTable());
    //    }

    //    public ActivitiesAttributesBuilder SetDefaultCategories(string inCategory, string outCategory, string inOutCategory, string optionsCategory)
    //    {
    //        _inCategory = new CategoryAttribute(inCategory);
    //        _outCategory = new CategoryAttribute(outCategory);
    //        _inOutCategory = new CategoryAttribute(inOutCategory);
    //        _optionsCategory = new CategoryAttribute(optionsCategory);
    //        return this;
    //    }

    //    private bool TryGetFromResource(string key, out string value)
    //    {
    //        value = _resourcesManager.GetString(key);
    //        return value != null;
    //    }

    //    private readonly string[] _propertiesToIgnore = new[] { "ContinueOnError", "Timeout", "DisplayName" };




    //    private void ApplyCommonAttributes(Type activityType)
    //    {
    //        var name = activityType.IsGenericType ? activityType.Name.Substring(0, activityType.Name.IndexOf('`')) : activityType.Name;
    //        if (TryGetFromResource(name + "_DisplayName", out string value))
    //            _tableBuilder.AddCustomAttributes(activityType, new DisplayNameAttribute(value));

    //        if (TryGetFromResource(name + "_Description", out value))
    //            _tableBuilder.AddCustomAttributes(activityType, new DescriptionAttribute(value));

    //        foreach (var prop in activityType.GetProperties())
    //        {
    //            ApplyDefaultEditorAttribute(activityType, prop);

    //            if (Array.IndexOf(_propertiesToIgnore, prop.Name) > -1)
    //            {
    //                if (prop.Name == "ContinueOnError" || prop.Name == "Timeout")
    //                    _tableBuilder.AddCustomAttributes(prop.PropertyType, new DescriptionAttribute(_resourcesManager.GetString("Common_" + prop.Name)));

    //                continue;
    //            }

    //            // DISPLAY NAME ATTRIBUTES

    //            if (TryGetFromResource($"{name}_{prop.Name}_DisplayName", out value))
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, new DisplayNameAttribute(value));
    //            }
    //            else if (prop.Name.StartsWith("Input") && prop.Name.Length > 5)
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, new DisplayNameAttribute(prop.Name.Substring(5)));
    //            }
    //            else if (prop.Name.StartsWith("Output") && prop.Name.Length > 6)
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, new DisplayNameAttribute(prop.Name.Substring(6)));
    //            }

    //            // DESCRIPTION ATTRIBUTES

    //            if (TryGetFromResource($"{name}_{prop.Name}_Description", out value))
    //                _tableBuilder.AddCustomAttributes(activityType, prop, new DescriptionAttribute(value));

    //            // CATEGORY ATTRIBUTES

    //            if (typeof(InArgument).IsAssignableFrom(prop.PropertyType))
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, _inCategory);
    //                continue;
    //            }

    //            if (typeof(OutArgument).IsAssignableFrom(prop.PropertyType))
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, _outCategory);
    //                continue;
    //            }

    //            if (typeof(InOutArgument).IsAssignableFrom(prop.PropertyType))
    //            {
    //                _tableBuilder.AddCustomAttributes(activityType, prop, _inOutCategory);
    //                continue;
    //            }

    //            _tableBuilder.AddCustomAttributes(activityType, prop, _optionsCategory);
    //        }
    //    }
    //}
}