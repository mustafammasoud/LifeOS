    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Avalonia.Threading;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using LifeOS.Application.Study;
    using LifeOS.Domain.Study;
    using LifeOS.Infrastructure.Notifications;
    using LifeOS.Infrastructure.Settings;
    using System.Collections.Generic;
    using System.Linq;
    using LifeOS.Desktop.Services;

    namespace LifeOS.Desktop.ViewModels.Pages;

    public sealed partial class StudyViewModel : ObservableObject
    {
        public List<int> DurationOptions { get; } = new() { 5, 15, 25, 45, 60 };
        private readonly IStudyService _studyService;
        private readonly INotificationService _notificationService;
        private readonly ISettingsService _settingsService;
        private readonly DispatcherTimer _timer;
        private int _ticksSinceLastSave;

        public ObservableCollection<Subject> Subjects { get; } = new();
        public ObservableCollection<PomodoroSession> TodaySessions { get; } = new();

        [ObservableProperty]
        private Subject? _selectedSubject;

        [ObservableProperty]
        private string _newSubjectName = string.Empty;

        [ObservableProperty]
        private int _durationMinutes = 25;

        [ObservableProperty]
        private int _remainingSeconds = 25 * 60;

        [ObservableProperty]
        private bool _isRunning;

        [ObservableProperty]
        private string _sessionNotes = string.Empty;

        [ObservableProperty]
        private int _todayFocusMinutes;

        public string TimeDisplay => $"{RemainingSeconds / 60:D2}:{RemainingSeconds % 60:D2}";

        public StudyViewModel(IStudyService studyService, INotificationService notificationService, IDialogService dialogService, ISettingsService settingsService)
        {
            _studyService = studyService;
            _notificationService = notificationService;
            _settingsService = settingsService;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += OnTick;
            _dialogService = dialogService;

            RestoreTimerState();
            _ = LoadAsync();
        }

        private void RestoreTimerState()
        { 
            
        var settings = _settingsService.Current;

        DurationMinutes = settings.StudyDurationMinutes;
        IsRunning = settings.StudyIsRunning;

        if (IsRunning && settings.StudyTargetEndTimeUtc is { } targetEnd)
        {
            var secondsLeft = (int)(targetEnd - DateTime.UtcNow).TotalSeconds;

            if (secondsLeft <= 0)
            {
                RemainingSeconds = 0;
                IsRunning = false;
                _ = CompleteSessionAsync();
            }
            else
            {
                RemainingSeconds = secondsLeft;
                _timer.Start();
            }
        }
        else
        {
            RemainingSeconds = settings.StudyRemainingSeconds;
        }
        }

        private void SaveTimerState()
        {
        var settings = _settingsService.Current;

        settings.StudyDurationMinutes = DurationMinutes;
        settings.StudyIsRunning = IsRunning;
        settings.StudySelectedSubjectId = SelectedSubject?.Id;

        if (IsRunning)
        {
            settings.StudyTargetEndTimeUtc = DateTime.UtcNow.AddSeconds(RemainingSeconds);
            settings.StudyRemainingSeconds = RemainingSeconds; // احتياطي
        }
        else
        {
            settings.StudyRemainingSeconds = RemainingSeconds;
            settings.StudyTargetEndTimeUtc = null;
        }

        _settingsService.Save(settings);
        
        }

        private async Task LoadAsync()
        {
            Subjects.Clear();
            foreach (var s in await _studyService.GetSubjectsAsync())
                Subjects.Add(s);

            var savedSubjectId = _settingsService.Current.StudySelectedSubjectId;
            if (savedSubjectId is not null)
                SelectedSubject = Subjects.FirstOrDefault(s => s.Id == savedSubjectId);

            TodaySessions.Clear();
            foreach (var s in await _studyService.GetTodaySessionsAsync())
                TodaySessions.Add(s);

            TodayFocusMinutes = await _studyService.GetTodayFocusMinutesAsync();
        }

        partial void OnDurationMinutesChanged(int value)
        {
            if (!IsRunning)
            {
                RemainingSeconds = value * 60;
                SaveTimerState();
            }
        }

        partial void OnRemainingSecondsChanged(int value) => OnPropertyChanged(nameof(TimeDisplay));

        partial void OnSelectedSubjectChanged(Subject? value) => SaveTimerState();

        [RelayCommand]
        private void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            _timer.Start();
            SaveTimerState();
        }

        [RelayCommand]
        private void Pause()
        {
            IsRunning = false;
            _timer.Stop();
            SaveTimerState();
        }

        [RelayCommand]
        private void Reset()
        {
            IsRunning = false;
            _timer.Stop();
            RemainingSeconds = DurationMinutes * 60;
            SaveTimerState();
        }

        private async void OnTick(object? sender, EventArgs e)
        {
            RemainingSeconds--;

            _ticksSinceLastSave++;
            if (_ticksSinceLastSave >= 5)
            {
                SaveTimerState();
                _ticksSinceLastSave = 0;
            }

            if (RemainingSeconds <= 0)
            {
                _timer.Stop();
                IsRunning = false;
                await CompleteSessionAsync();
            }
        }

        private async Task CompleteSessionAsync()
        {
            _notificationService.Notify("LifeOS", " session finished!");

            await _studyService.LogSessionAsync(
                SelectedSubject?.Id,
                DurationMinutes,
                string.IsNullOrWhiteSpace(SessionNotes) ? null : SessionNotes);

            SessionNotes = string.Empty;
            RemainingSeconds = DurationMinutes * 60;
            SaveTimerState();
            await LoadAsync();
        }

        [RelayCommand]
        private async Task AddSubjectAsync()
        {
            if (string.IsNullOrWhiteSpace(NewSubjectName)) return;

            await _studyService.AddSubjectAsync(NewSubjectName);
            NewSubjectName = string.Empty;
            await LoadAsync();
        }

        private readonly IDialogService _dialogService;

        [RelayCommand]
        private async Task DeleteSubjectAsync(Subject subject)
        {
            if (!await _dialogService.ConfirmAsync("Delete Subject", $"Delete \"{subject.Name}\"?")) return;
            await _studyService.DeleteSubjectAsync(subject.Id);
            await LoadAsync();
        }
    }