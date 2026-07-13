using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Calendar;
using LifeOS.Desktop.Services;
using LifeOS.Domain.Calendar;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class CalendarViewModel : ObservableObject
{
    private readonly ICalendarService _calendarService;

    public ObservableCollection<CalendarEvent> Events { get; } = new();

 [ObservableProperty] private DateTime? _selectedDate = DateTime.Today;
 [ObservableProperty] private string _newTitle = string.Empty;
 [ObservableProperty] private TimeSpan? _newStartTime = DateTime.Now.TimeOfDay;
 [ObservableProperty] private TimeSpan? _newEndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1)); 


    partial void OnSelectedDateChanged(DateTime? value) => _ = LoadAsync();

    private async Task LoadAsync()
   {  
    var date = DateOnly.FromDateTime(SelectedDate ?? DateTime.Today);
    var events = await _calendarService.GetEventsForDateAsync(date);
    Events.Clear();
    foreach (var e in events)
        Events.Add(e);
   }

    [RelayCommand]
    private async Task AddEventAsync()
  {
    if (string.IsNullOrWhiteSpace(NewTitle) || NewStartTime is null || NewEndTime is null) return;

    var baseDate = (SelectedDate ?? DateTime.Today).Date;
    var start = baseDate.Add(NewStartTime.Value);
    var end = baseDate.Add(NewEndTime.Value);

    await _calendarService.AddEventAsync(NewTitle, start, end, null);
    NewTitle = string.Empty;
    await LoadAsync();
  } 

    private readonly IDialogService _dialogService;

public CalendarViewModel(ICalendarService calendarService, IDialogService dialogService)
{
    _calendarService = calendarService;
    _dialogService = dialogService;
    _ = LoadAsync();
}

[RelayCommand]
private async Task DeleteEventAsync(CalendarEvent ev)
{
    if (!await _dialogService.ConfirmAsync("Delete Event", $"Delete \"{ev.Title}\"?")) return;
    await _calendarService.DeleteEventAsync(ev.Id);
    await LoadAsync();
}
}
