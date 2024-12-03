using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.VisualBasic.FileIO;
using SortingApp.Models;
using SortingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SortingApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        //List<OperationOption> operations;
        public ObservableCollection<OperationOption> Operations { get; } = new ObservableCollection<OperationOption>();

        public ObservableCollection<Step> Steps { get; } = new ObservableCollection<Step>();
        public ObservableCollection<Step> OrderedSteps { get; } = new ObservableCollection<Step>();

        public MainWindow()
        {
            this.InitializeComponent();

            var screenWidth = DisplayArea.Primary.WorkArea.Width;
            var screenHeight = DisplayArea.Primary.WorkArea.Height;
            var windowWidth = 1600;
            var windowHeight = 800;
            //AppWindow.Resize(new Windows.Graphics.SizeInt32(1600, 800));
            AppWindow.MoveAndResize(new Windows.Graphics.RectInt32((int)(screenWidth - windowWidth) / 2,
                (int)(screenHeight - windowHeight) / 2, windowWidth, windowHeight));

            Operations =
            [
                new OperationOption { Label = "10KVI段进线101开关由运行转冷备用",Type = OperationType.IN_POWER_OFF  },
                new OperationOption { Label = "10KVI段进线101开关由冷备用转运行",Type = OperationType.IN_POWER_ON  },
                new OperationOption { Label = "10KVI段出线102开关由运行转检修",Type = OperationType.OUT_POWER_OFF  },
                new OperationOption { Label = "10KVI段出线102开关由检修转运行",Type = OperationType.OUT_POWER_ON  },
            ];
        }

        private void RDBOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var option = e.AddedItems[0] as OperationOption;
            if (option != null)
            {
                /*ContentDialog noWifiDialog = new ContentDialog()
                {
                    Title = "Prompt",
                    Content = option.Label,
                    CloseButtonText = "Ok"
                };

                noWifiDialog.XamlRoot = this.Content.XamlRoot;

                await noWifiDialog.ShowAsync();*/
                Steps.Clear();
                OrderedSteps.Clear();
                switch (option.Type)
                {
                    case OperationType.IN_POWER_OFF:
                        StepFileReader.ParseSteps(Steps, "1011.csv", 12);
                        break;
                    case OperationType.IN_POWER_ON:
                        StepFileReader.ParseSteps(Steps, "1012.csv", 14);
                        break;
                    case OperationType.OUT_POWER_OFF:
                        StepFileReader.ParseSteps(Steps, "1021.csv", 13);
                        break;
                    case OperationType.OUT_POWER_ON:
                        StepFileReader.ParseSteps(Steps, "1022.csv", 14);
                        break;
                    default:
                        break;
                }

            }
        }

        private async void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            bool success = true;
            foreach (var step in OrderedSteps)
            {
                success = (step.ID == step.Index);
                if (!success)
                {
                    break;
                }
            }


            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = "提示",
                Content = success ? "排序正确!" : "排序失败!",
                CloseButtonText = "确定"
            };

            noWifiDialog.XamlRoot = this.Content.XamlRoot;

            await noWifiDialog.ShowAsync();
        }

        private void OrigView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var step = OrigView.SelectedItem as Step;
            if (step != null)
            {
                this.Steps.Remove(step);

                int index = this.OrderedSteps.Count;
                step.Index = ++index;
                this.OrderedSteps.Add(step);
            }
        }
    }
}
