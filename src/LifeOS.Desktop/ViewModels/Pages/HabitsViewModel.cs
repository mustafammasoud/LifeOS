using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeOS.Application.Habits;
using LifeOS.Domain.Habits;
using System.Threading.Tasks;
using LifeOS.Desktop.Services;
using System.Collections.Generic;
using System;

namespace LifeOS.Desktop.ViewModels.Pages;

public sealed partial class HabitsViewModel : ObservableObject
{
    private readonly Dictionary<Guid, int> _dailyFloors = new();
   private DateOnly _floorsDate = DateOnly.FromDateTime(DateTime.Today);
    private readonly IHabitService _habitService;

    public HabitsViewModel(IHabitService habitService, IDialogService dialogService)
    {
        _habitService = habitService;
        _dialogService = dialogService;
        _ = LoadAsync();
    }
    public ObservableCollection<HabitProgress> Habits { get; } = new();

    [ObservableProperty]
    private string _newHabitName = string.Empty;
    private async Task LoadAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

    if (today != _floorsDate)
    {
        _dailyFloors.Clear();   
        _floorsDate = today;
    }

    var habits = await _habitService.GetTodayHabitsAsync();
    Habits.Clear();

    foreach (var h in habits)
    {
        if (!_dailyFloors.ContainsKey(h.Habit.Id))
            _dailyFloors[h.Habit.Id] = h.TodayCount;  

        Habits.Add(h);
    }
    }

    [RelayCommand]
    private async Task AddHabitAsync()
    {
        if (string.IsNullOrWhiteSpace(NewHabitName)) return;

        await _habitService.AddHabitAsync(NewHabitName, HabitType.Boolean);
        NewHabitName = string.Empty;
        await LoadAsync();
    }
    private readonly IDialogService _dialogService;  


[RelayCommand]
private async Task DeleteHabitAsync(HabitProgress progress)
{
    if (!await _dialogService.ConfirmAsync("Delete Habit", $"Delete \"{progress.Habit.Name}\"?")) return;
    await _habitService.DeleteHabitAsync(progress.Habit.Id);
    await LoadAsync();
}

    [RelayCommand]
  private async Task IncrementProgressAsync(HabitProgress progress)
  {
    if (progress.TodayCount >= progress.Habit.TargetCount)
        return;

    await _habitService.LogProgressAsync(progress.Habit.Id, 1);
    await LoadAsync();
    }
  
  [RelayCommand]
  private async Task DecrementProgressAsync(HabitProgress progress)
  {
    var floor = _dailyFloors.TryGetValue(progress.Habit.Id, out var f) ? f : 0;

    if (progress.TodayCount <= floor)
        return;   

    await _habitService.LogProgressAsync(progress.Habit.Id, -1);
    await LoadAsync();
  }
}

