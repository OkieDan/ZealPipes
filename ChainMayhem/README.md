# ChainMayhem

A lightweight EverQuest pet monitoring tool that provides voice alerts for charm breaks and low pet health.

## Features

- **Charm Break Detection**: Alerts when a pet charm breaks (pet HP drops from >10% to 0%)
- **Low Pet Health Warning**: Alerts when pet HP drops below 20%
- **Multi-Character Support**: Monitor multiple EQ characters simultaneously
- **Voice Alerts**: Uses text-to-speech for immediate notifications

## Requirements

- .NET 8.0 or higher
- EverQuest with Zeal installed and configured
- Windows with System.Speech support

## Configuration

Edit `appsettings.json` to match your Zeal configuration:

```json
{
  "ZealSettings": {
    "ProcessName": "eqgame",
    "PipeName": "zeal",
    "BufferSize": 32768
  }
}
```

## Usage

1. Launch EverQuest with Zeal
2. Run ChainMayhem
3. Select the character you want to monitor from the menu
4. ChainMayhem will alert you when:
   - A charm breaks on any group member
   - Any group member's pet HP drops below 20%

## How It Works

ChainMayhem uses ZealPipes to read real-time data from EverQuest and monitors:
- Group member pet health percentages
- Sudden pet HP changes indicating charm breaks or critical health