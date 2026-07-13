using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Notes;
using LifeOS.Desktop.Services;
using LifeOS.Domain.Notes;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class NotesViewModel : ObservableObject
{
    private readonly INoteService _noteService;

    public ObservableCollection<Note> Notes { get; } = new();

    [ObservableProperty] private Note? _selectedNote;
    [ObservableProperty] private string _editTitle = string.Empty;
    [ObservableProperty] private string _editContent = string.Empty;
    [ObservableProperty] private bool _isPreviewMode;


    private async Task LoadAsync()
    {
        Notes.Clear();
        foreach (var n in await _noteService.GetAllNotesAsync())
            Notes.Add(n);
    }

    partial void OnSelectedNoteChanged(Note? value)
    {
        EditTitle = value?.Title ?? string.Empty;
        EditContent = value?.ContentMarkdown ?? string.Empty;
    }

    [RelayCommand]
    private async Task NewNoteAsync()
    {
        var note = await _noteService.CreateNoteAsync("Untitled", null);
        await LoadAsync();
        SelectedNote = Notes.FirstOrDefault(n => n.Id == note.Id);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedNote is null) return;

        await _noteService.UpdateNoteAsync(SelectedNote.Id, EditTitle, EditContent, SelectedNote.SubjectId);
        await LoadAsync();
    }

   private readonly IDialogService _dialogService;

public NotesViewModel(INoteService noteService, IDialogService dialogService)
{
    _noteService = noteService;
    _dialogService = dialogService;
    _ = LoadAsync();
}

[RelayCommand]
private async Task DeleteAsync()
{
    if (SelectedNote is null) return;
    if (!await _dialogService.ConfirmAsync("Delete Note", $"Delete \"{SelectedNote.Title}\"?")) return;

    await _noteService.DeleteNoteAsync(SelectedNote.Id);
    SelectedNote = null;
    await LoadAsync();
}
}