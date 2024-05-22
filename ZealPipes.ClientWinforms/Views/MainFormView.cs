using System.Diagnostics;
using ZealPipes.ClientWinforms.Interfaces;
using ZealPipes.ClientWinforms.Models;
using ZealPipes.ClientWinforms.Presenters;
using ZealPipes.ClientWinforms.UI.Factories;
using ZealPipes.ClientWinforms.UI.Interfaces;

namespace ZealPipes.ClientWinforms.Views;

public partial class MainForm : Form, IMainView
{
    private readonly MainPresenter _presenter;
    private IPlayerControl _playerControl;
    private Bitmap backgroundBuffer;

    public MainForm(MainPresenter presenter)
    {
        InitializeComponent();
        this.BackgroundImageLayout = ImageLayout.Stretch;
        _presenter = presenter;
        _presenter.SetView(this);

        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        UpdateStyles();

        // Use the factory to create the desired version of PlayerControl
        _playerControl = PlayerControlFactory.CreatePlayerControl("Default"); // or "CustomTheme"
        
        Controls.Add(_playerControl.GetControl());
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (backgroundBuffer != null)
        {
            e.Graphics.DrawImage(backgroundBuffer, 0, 0, this.Width, this.Height);
        }
    }

    public void SetBackgroundImage(Image image)
    {
        if (backgroundBuffer != null)
        {
            backgroundBuffer.Dispose();
        }
        backgroundBuffer = new Bitmap(image);
        this.Invalidate(); // Triggers a repaint
    }
    public void UpdateCharacterData(Character character)
    {
        try
        {
            this.Invoke(new Action(() =>
            {
                _playerControl.UpdatePlayerInfo(character);
            }));
        }
        catch (Exception ex) {
            Debug.WriteLine($"{ex.Message}: {ex.StackTrace}");
        }
    }

    public void DrawGauge(string gaugeData)
    {
        // Implement logic to draw gauge
    }

    public void UpdatePlayerData(Character character)
    {
        _playerControl.UpdatePlayerInfo(character);
    }

    public void UpdateLabelData(string labelData)
    {
        // Implement logic to update label data
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}

