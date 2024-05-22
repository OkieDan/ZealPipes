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
        private System.Windows.Forms.Timer screenshotTimer;
        private IntPtr targetWindowHandle;
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

        public void SetView(IMainView view)
        {
            _view = view;
            InitializeScreenshotTimer();
        }

        private void InitializeScreenshotTimer()
        {
            // Replace with the actual title of the 3rd party window
            string targetWindowTitle = "Client1";

            // Find the target window
            targetWindowHandle = WinApi.FindWindow(null, targetWindowTitle);
            if (targetWindowHandle == IntPtr.Zero)
            {
                //_view.ShowMessage("Target window not found.");
                return;
            }

            // Initialize the timer
            screenshotTimer = new System.Windows.Forms.Timer();
            screenshotTimer.Interval = 100; // 100 ms = 10 times per second
            screenshotTimer.Tick += ScreenshotTimer_Tick;
            screenshotTimer.Start();
        }


        private void ZealMessageService_OnCharacterUpdated(object? sender, ZealCharacter.ZealCharacterUpdatedEventArgs e)
        {
            _iCharacterUpdate++;
            //var player = new Player(e.Character);
            //_view.UpdateCharacterData($"Character.Name '{e.Character.Name}' on Character.ProcessId {e.ProcessId} has an update.");
            
            _view.UpdateCharacterData(new Character(e.Character));
            if (_iCharacterUpdate % 100 == 1)
                Debug.WriteLine($"100 character updates!");
            //Debug.WriteLine($"Character updates: {_iCharacterUpdate}");

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
            Bitmap screenshot = WinApi.CaptureWindow(targetWindowHandle);
            if (screenshot != null)
            {
                // Pass the screenshot to the view
                _view.SetBackgroundImage(screenshot);
            }
        }
    }
}