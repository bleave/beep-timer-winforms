# BeepTimerWinForms

A minimal **WinForms** tray-icon interval beep timer. Sits silent in your tray, beeps every N seconds, useful for everything from pomodoro-style work intervals to drilling a habit.

## Features

- System tray icon with right-click menu
- Configurable interval (default 30 seconds)
- Toggle between system beep and an external sound file
- "Snooze" / pause without losing the schedule

## Build / Run

```bash
dotnet build
dotnet run --project BeepTimerWinForms
```

Targets .NET 8 + Windows Forms (Windows only).

## Variants

This repo is the base version. Other variants exist locally that explore compact dark UI, NAudio, and WAV+MP3 playback — they may get pushed as separate repos later.

## Stack

C# / .NET 8 / Windows Forms.
