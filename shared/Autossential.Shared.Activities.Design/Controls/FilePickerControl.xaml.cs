using Autossential.Shared.Utils;
using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Autossential.Shared.Activities.Design.Controls
{
    // Interaction logic for FilePickerControl.xaml
    public partial class FilePickerControl
    {
        public FilePickerControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ModelItemProperty = DependencyProperty.Register("ModelItem", typeof(ModelItem), typeof(FilePickerControl));

        public ModelItem ModelItem
        {
            get { return GetValue(ModelItemProperty) as ModelItem; }
            set { SetValue(ModelItemProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(FilePickerControl));

        public string PropertyName
        {
            get { return GetValue(PropertyNameProperty) as string; }
            set { SetValue(PropertyNameProperty, value); }
        }

        public string Filter { get; set; }
        public string Title { get; set; }
        public bool Multiselect { get; set; }
        public bool? CheckFileExists { get; set; }
        public bool? CheckPathExists { get; set; }
        public bool? ValidateNames { get; set; }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = Multiselect
            };

            if (string.IsNullOrEmpty(Filter))
                ofd.Filter = "All files (*.*)|*.*";
            else
                ofd.Filter = Filter;

            if (string.IsNullOrWhiteSpace(Title))
                Title = "Select file(s)";

            ofd.Title = Title;
            ofd.InitialDirectory = Directory.GetCurrentDirectory();

            if (CheckFileExists.HasValue)
                ofd.CheckFileExists = CheckFileExists.Value;

            if (CheckPathExists.HasValue)
                ofd.CheckPathExists = CheckPathExists.Value;

            if (ValidateNames.HasValue)
                ofd.ValidateNames = ValidateNames.Value;

            if (ofd.ShowDialog() == true)
            {
                var baseUri = new Uri(ofd.InitialDirectory);

                var files = ofd.FileNames;
                if (files.Length == 1)
                {
                    ModelItem.Properties[PropertyName].SetValue(InArgument<string>.FromValue(IOUtil.GetRelativePath(ofd.InitialDirectory, files[0])));
                    return;
                }

                var paths = string.Join(", ", files.Select(path => $"\"{IOUtil.GetRelativePath(ofd.InitialDirectory, path)}\""));
                var code = ExpressionServiceLanguage.CreateExpression<IEnumerable<string>>(ModelItem, $"{{{paths}}}", $"new [] {{{paths}}}");
                ModelItem.Properties[PropertyName].SetValue(new InArgument<IEnumerable<string>>(code));
            }
        }
    }
}