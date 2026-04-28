using Autossential.Activities.Properties;
using System.Activities;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.Extensions
{
    internal static class DesignExtensions
    {
        extension<T>(DesignProperty<T> property)
        {
            /// <summary>
            /// Adds a widget of the specified type to the current property.
            /// </summary>
            /// <param name="widgetType">The type of widget to add. Cannot be null or empty.</param>
            public void AddWidget(string widgetType)
            {
                property.Widget = new DefaultWidget
                {
                    Type = widgetType,
                };
            }
        }

#if WINDOWS
        extension(DesignProperty<InArgument<string>> property)
        {
            public void AddFolderDialogMenuAction()
            {
                property.AddMenuAction(new MenuAction()
                {
                    DisplayName = Resources.Common_ViewModel_BrowseForFolder,
                    IsVisible = true,
                    IsMain = true,
                    Handler = _ =>
                    {
                        // cria uma TaskCompletionSource para controlar o resultado
                        var tcs = new TaskCompletionSource<bool>();

                        var thread = new Thread(() =>
                        {
                            try
                            {
                                using (var dialog = new FolderBrowserDialog())
                                {
                                    dialog.ShowNewFolderButton = true;
                                    if (dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        property.Value = Path.GetRelativePath(Directory.GetCurrentDirectory(), dialog.SelectedPath);
                                    }
                                }
                                tcs.SetResult(true);
                            }
                            catch (Exception ex)
                            {
                                tcs.SetException(ex);
                            }
                        });

                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();

                        return tcs.Task;
                    }
                });
            }

            public void AddFileDialogMenuAction(bool checkFileExists, string filter)
            {
                property.AddMenuAction(new MenuAction
                {
                    DisplayName = Resources.Common_ViewModel_BrowseForFile,
                    IsVisible = true,
                    IsMain = true,
                    Handler = _ => Task.Run(() =>
                    {
                        var ofd = new Microsoft.Win32.OpenFileDialog
                        {
                            Filter = filter,
                            Multiselect = false,
                            CheckFileExists = checkFileExists
                        };

                        if (ofd.ShowDialog() == true)
                        {
                            property.Value = Path.GetRelativePath(Directory.GetCurrentDirectory(), ofd.FileName);
                        }
                    })
                });
            }
        }
#endif
    }
}