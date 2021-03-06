﻿using PeopleViewer.Presentation;
using System;
using System.Windows;

namespace PeopleViewer
{
    public partial class MainWindow : Window
    {
        PeopleViewModel ViewModel { get; }

        public MainWindow(PeopleViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException("'viewModel' parameter cannot be null");
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshPeople();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearPeople();
        }
    }
}
