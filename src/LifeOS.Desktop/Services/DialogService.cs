using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using LifeOS.Desktop.Views.Dialogs;

namespace LifeOS.Desktop.Services;

public class DialogService : IDialogService
{
    public async Task<bool> ConfirmAsync(string title, string message)
    {
        var dialog = new FAContentDialog
        {
            Title = title,
            Content = message,
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel",
            DefaultButton = FAContentDialogButton.Close
        };

        var result = await dialog.ShowAsync();
        return result == FAContentDialogResult.Primary;
    }

    public async Task<List<WeeklyPlanningItem>?> ShowWeeklyPlanningAsync(List<WeeklyPlanningItem> candidates)
    {
        var content = new WeeklyPlanningDialog { DataContext = candidates };

        var dialog = new FAContentDialog
        {
            Title = "Plan Your Week",
            Content = content,
            PrimaryButtonText = "Add Selected",
            CloseButtonText = "Skip",
            DefaultButton = FAContentDialogButton.Primary
        };

        var result = await dialog.ShowAsync();
        if (result != FAContentDialogResult.Primary)
            return null;

        return candidates.Where(c => c.IsSelected).ToList();
    }
}