using Lepton_Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Lepton_Dictionary.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ExtensionTab : Page
    {
        public ObservableCollection<DictionaryExtension> Items = null; // The UI binds against the extensions
        public ExtensionTab()
        {
            this.InitializeComponent();
            Items = AppService.ExtensionManager.Extensions; // The AppService object holds global state such as the collection of extensions
            this.DataContext = Items;
        }
        /// <summary>
        /// Enable an extension
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DictionaryExtension ext = cb.DataContext as DictionaryExtension;
            if (!ext.Enabled)
            {
                await ext.Enable();
            }
        }

        /// <summary>
        /// Disable an extension
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            DictionaryExtension ext = cb.DataContext as DictionaryExtension;
            if (ext.Enabled)
            {
                ext.Disable();
            }
        }

        /// <summary>
        /// Remove an extension from the system if the user OKs it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // remove the package
            Button btn = sender as Button;
            DictionaryExtension ext = btn.DataContext as DictionaryExtension;
            AppService.ExtensionManager.RemoveExtension(ext);
        }

    }
}
