using CommunityToolkit.Mvvm.ComponentModel;

namespace LifeOS.Desktop.ViewModels.Pages;

public partial class ComingSoonViewModel : ObservableObject
{

    [ObservableProperty]
    private string pageName;


    public ComingSoonViewModel(string pageName)
    {
        PageName = pageName;
    }

}
