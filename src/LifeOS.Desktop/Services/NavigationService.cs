using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeOS.Desktop.Navigation;

namespace LifeOS.Desktop.Services;

public interface INavigationService
{
    event Action<PageKey>? NavigateRequested;

    void Navigate(PageKey page);
}

public class NavigationService : INavigationService
{
    public event Action<PageKey>? NavigateRequested;

    public void Navigate(PageKey page)
    {
        NavigateRequested?.Invoke(page);
    }
}