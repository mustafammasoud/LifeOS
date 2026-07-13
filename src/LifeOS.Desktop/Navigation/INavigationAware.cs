using System.Threading.Tasks;

namespace LifeOS.Desktop.Navigation;

public interface INavigationAware
{
    Task OnNavigatedToAsync();
}