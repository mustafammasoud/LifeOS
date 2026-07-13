using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;

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
}