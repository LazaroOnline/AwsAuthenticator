using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AwsCredentialManager.ViewModels;

namespace AwsCredentialManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            var awsCredentialManagerForm = this.FindControl<AwsCredentialManagerForm>("AwsCredentialManagerForm");
            var viewModel = (AwsCredentialManagerViewModel)awsCredentialManagerForm.DataContext;
            
            viewModel.SaveConfig();
        }
    }
}
