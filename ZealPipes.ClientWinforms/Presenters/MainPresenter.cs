using System.Diagnostics;
using ZealPipes.ClientWinforms.Interfaces;
using ZealPipes.ClientWinforms.Models;
using ZealPipes.Common.Models;
using ZealPipes.Services;
using System.Windows.Forms;
using ZealPipes.ClientWinforms;

namespace ZealPipes.ClientWinforms.Presenters
{
    public class MainPresenter
    {
        private readonly ZealMessageService _zealMessageService;
        private IMainView _view;
        private int _iCharacterUpdate = 0;
        private System.Windows.Forms.Timer _screenshotTimer;
        private IntPtr _targetWindowHandle;
        private List<string> _characterNames = new List<string>();
        private string _selectedCharacterName = "Client1";

        public MainPresenter(ZealMessageService zealMessageService)
        {
            _zealMessageService = zealMessageService;

            // Subscribe to events
            _zealMessageService.OnCharacterUpdated += ZealMessageService_OnCharacterUpdated;
            _zealMessageService.OnGaugeMessageReceived += ZealMessageService_OnGaugeMessageReceived;
            _zealMessageService.OnPlayerMessageReceived += ZealMessageService_OnPlayerMessageReceived;
            _zealMessageService.OnLabelMessageReceived += ZealMessageService_OnLabelMessageReceived;
            _zealMessageService.StartProcessing();
        }
        public void SetSelectedCharacter(string selectedCharacterName)
        {
            if (_selectedCharacterName == selectedCharacterName)
            {
                return;
            }
            else
            {
                if (_screenshotTimer != null)
                {
                    _screenshotTimer.Stop();
                    _screenshotTimer.Tick -= ScreenshotTimer_Tick;
                }
                _selectedCharacterName = selectedCharacterName;
                InitializeScreenshotTimer();
            }
        }

        public void SetView(IMainView view)
        {
            _view = view;
            InitializeScreenshotTimer();
        }

        private void InitializeScreenshotTimer()
        {
            // Replace with the actual title of the 3rd party window
            string targetWindowTitle = _selectedCharacterName;

            // Find the target window
            _targetWindowHandle = WinApi.FindWindow(null, targetWindowTitle);
            if (_targetWindowHandle == IntPtr.Zero)
            {
                //_view.ShowMessage("Target window not found.");
                return;
            }

            // Initialize the timer
            _screenshotTimer = new System.Windows.Forms.Timer();
            _screenshotTimer.Interval = 100; // 100 ms = 10 times per second
            _screenshotTimer.Tick += ScreenshotTimer_Tick;
            _screenshotTimer.Start();
        }


        private void ZealMessageService_OnCharacterUpdated(object? sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
        {
            _iCharacterUpdate++;
            //var player = new Player(e.Character);
            //_view.UpdateCharacterData($"Character.Name '{e.Character.Name}' on Character.ProcessId {e.ProcessId} has an update.");
            if (!_characterNames.Exists(x => x == e.Character.Name))
            {
                _characterNames.Add(e.Character.Name);
                _view.UpdateCharacterDropdown(_characterNames);
            }

            if (e.Character.Name == _selectedCharacterName)
            {
                _view.UpdateCharacterData(new Character(e.Character));
                if (_iCharacterUpdate % 100 == 1)
                    Debug.WriteLine($"100 character updates!");
                Debug.WriteLine($"Character updates: {_iCharacterUpdate}");
            }
        }

        private void ZealMessageService_OnGaugeMessageReceived(object? sender, ZealMessageService.GaugeMessageReceivedEventArgs e)
        {
            _view.DrawGauge($"Gauge data: {e.Message.Data}");
        }

        private void ZealMessageService_OnPlayerMessageReceived(object? sender, ZealMessageService.PlayerMessageReceivedEventArgs e)
        {
            
            //_view.UpdatePlayerData($"Player data: {e.Message.Data}");
        }

        private void ZealMessageService_OnLabelMessageReceived(object? sender, ZealMessageService.LabelMessageReceivedEventArgs e)
        {
            _view.UpdateLabelData($"Label data: {e.Message.Data}");
        }


        private void ScreenshotTimer_Tick(object? sender, EventArgs e)
        {
            // Capture the screenshot
            Bitmap screenshot = WinApi.CaptureWindow(_targetWindowHandle);
            if (screenshot != null)
            {
                // Pass the screenshot to the view
                _view.SetBackgroundImage(screenshot);
            }
        }
    }
}