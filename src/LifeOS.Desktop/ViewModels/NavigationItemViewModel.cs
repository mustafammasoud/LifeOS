using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Desktop.Navigation;
using Material.Icons;

namespace LifeOS.Desktop.ViewModels;

public sealed class NavigationItemViewModel : ObservableObject
{
    public PageKey Key { get; }
    public string Label { get; }
    public MaterialIconKind Icon { get; }


    public NavigationItemViewModel(
        PageKey key,
        string label,
        MaterialIconKind icon)
    {
        Key = key;
        Label = label;
        Icon = icon;
    }
}
