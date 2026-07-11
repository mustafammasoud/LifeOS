using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace LifeOS.Desktop.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{

    [ObservableProperty]
    private string greeting;


    [ObservableProperty]
    private string todayLabel;


    public DashboardViewModel()
    {
        Greeting = BuildGreeting();
        TodayLabel = DateTime.Now.ToString("dddd, MMMM d");
    }


    private string BuildGreeting()
    {
        var hour = DateTime.Now.Hour;

        return hour switch
        {
            <12 => "Good Morning",
            <18 => "Good Afternoon",
            _ => "Good Evening"
        };
    }

}

