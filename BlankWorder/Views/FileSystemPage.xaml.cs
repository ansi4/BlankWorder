﻿using System;
using System.Collections.Generic;
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
using BlankWorder.ViewModels;
using System.Windows.Input;
using Prism.Commands;
using Windows.Storage.Pickers;
using Windows.Storage;
using BlankWorder.Models;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BlankWorder.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileSystemPage : Page
    {

        // public DependencyProperty Selected = DependencyProperty.RegisterAttached;

        public FileSystemViewModel ViewModel => DataContext as FileSystemViewModel;

        public FileSystemPage()
        {            
            this.InitializeComponent();
        }
    }
}
