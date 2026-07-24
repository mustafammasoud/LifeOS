using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeOS.Desktop.Services;

public interface IDialogService
{
    Task<bool> ConfirmAsync(string title, string message);
    Task<List<WeeklyPlanningItem>?> ShowWeeklyPlanningAsync(List<WeeklyPlanningItem> candidates);
}