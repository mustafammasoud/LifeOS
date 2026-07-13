// LifeOS/src/LifeOS.Desktop/Views/Pages/StatisticsView.axaml.cs
using Avalonia.Controls;
using Avalonia.Interactivity;
using LifeOS.Desktop.ViewModels.Pages;

namespace LifeOS.Desktop.Views.Pages;

public partial class StatisticsView : UserControl
{
    public StatisticsView()
    {
        InitializeComponent();
    }

    private void OnRefreshClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is StatisticsViewModel vm)
        {
            _ = vm.LoadAsync();   
        }
    }
}