<div align="center">

# 🌿 LifeOS

**Your entire life, organized in one place.**

Tasks • Study Sessions • Habits • Goals • Events • Notes • Statistics

[![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-blue)]()
[![License: MIT](https://img.shields.io/badge/license-MIT-green)](LICENSE)

[**⬇️ Download LifeOS**](../../releases/latest) &nbsp;|&nbsp;  [Installation](#-installation) 

</div>

---

##  Get Started

No coding. No terminal knowledge required. Just download, install, and run.

👉 **[Go to the latest release](../../releases/latest)** and grab the file for your operating system.

---

##  Features

**📊 Dashboard**
See everything that matters at a glance — tasks, habits, goals, and stats in one screen.

**✅ Tasks**
Create, prioritize, and track your daily and weekly to-dos. Tasks automatically roll over into a new day at midnight, and your weekly completion rate is calculated from historical data — so nothing gets lost.

**📚 Study**
Built-in Pomodoro timer to run focused study sessions per subject, with a desktop notification when a session finishes.

**🔁 Habits**
Track daily habits with a simple increment/decrement counter, capped at your daily target, with visual streaks that show your consistency over time.

**🎯 Goals**
Set long or short-term goals — tracked manually or via milestones — and watch your true weighted progress percentage update in real time.

**📅 Events**
Never miss what's coming up — "Meeting in 3 days, 5:00 PM – 6:00 PM."

**📝 Notes**
Markdown-powered notes with live preview, linkable to your study subjects.

**📈 Statistics**
A fully connected, live-updating view of your progress — weekly tasks, habit streaks, goal completion, and a chart tracking your task completion over the last 7 days.

**⚙️ Settings**
Set your name, toggle notifications on/off, and choose where your database lives.

---

## 🔒 Your Data, Your Device

LifeOS stores everything locally in a SQLite database on your machine — nothing is sent to any server. You're always in full control of your data, and you can change where it's stored anytime from **Settings**.

---

## 💻 Installation

Head to the **[Releases page](../../releases/latest)** and download the file for your system.

### 🪟 Windows

1. Download `LifeOS-Setup.exe`
2. Run it and follow the installer (choose whether to create a desktop shortcut)
3. Launch **LifeOS** from the Start Menu or your desktop

> If Windows shows a "protected your PC" SmartScreen prompt (common for new, unsigned apps), click **More info → Run anyway**.

### 🐧 Linux

1. Download `LifeOS-linux-x64.zip`
2. Unzip it
3. Open a terminal in the extracted folder and run:
   ```bash
   chmod +x install.sh
   ./install.sh
   ```
4. LifeOS will appear in your application menu and on your Desktop

### 🍎 macOS

1. Download `LifeOS-macOS.dmg`
2. Open the `.dmg` file
3. Drag **LifeOS** into your **Applications** folder
4. Launch it from Launchpad or Applications

> On first launch, if macOS blocks the app (since it isn't notarized), go to **System Settings → Privacy & Security** and click **Open Anyway**.

---

## Built With

Built as a cross-platform desktop app using **.NET 10** and **Avalonia UI**, following **Clean Architecture** principles under the hood . 

---

## 👩‍💻 Building From Source

If you'd rather build LifeOS yourself instead of using the prebuilt installers:

```bash
git clone https://github.com/mustafammasoud/LifeOS.git 
cd LifeOS
dotnet run --project src/LifeOS.Desktop
```

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

---

## 📄 License

This project is licensed under the **MIT License** .

In short: you're free to use, copy, modify, and distribute this software, including for commercial purposes, as long as the original copyright notice is included.
