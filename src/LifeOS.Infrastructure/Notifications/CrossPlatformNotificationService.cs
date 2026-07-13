using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LifeOS.Infrastructure.Notifications;

public class CrossPlatformNotificationService : INotificationService
{
    public void Notify(string title, string message)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            NotifyLinux(title, message);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            NotifyMac(title, message);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            NotifyWindows(title, message);
        }
    }

    private static void NotifyLinux(string title, string message)
    {
        TryRun("notify-send", $"\"{title}\" \"{message}\"");
        TryRun("paplay", "/usr/share/sounds/freedesktop/stereo/complete.oga");
    }

    private static void NotifyMac(string title, string message)
    {
        var script = $"display notification \"{message}\" with title \"{title}\" sound name \"Glass\"";
        TryRun("osascript", $"-e '{script}'");
    }

    private static void NotifyWindows(string title, string message)
    {
        var script =
            "Add-Type -AssemblyName System.Windows.Forms; " +
            "Add-Type -AssemblyName System.Drawing; " +
            "$n = New-Object System.Windows.Forms.NotifyIcon; " +
            "$n.Icon = [System.Drawing.SystemIcons]::Information; " +
            "$n.Visible = $true; " +
            $"$n.ShowBalloonTip(3000, '{title}', '{message}', [System.Windows.Forms.ToolTipIcon]::Info); " +
            "Start-Sleep -Seconds 4; " +
            "$n.Dispose()";

        TryRun("powershell", $"-NoProfile -Command \"{script}\"");
    }

    private static void TryRun(string fileName, string arguments)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }
        catch
        {
            // Tool not available on this machine — fail silently rather than crash the app.
        }
    }
}
