using Microsoft.Maui.Controls.Shapes;

namespace MauiApp4;

public partial class MainPage
{
    CancellationTokenSource animateTimerCancellationTokenSource = new();
    const uint CoverAnimationDuration = 5000;

    public MainPage()
    {
        InitializeComponent();
        
        progress.SizeChanged += HandleProgressSizeChanged;
    }
    
    void HandleProgressSizeChanged(object? sender, EventArgs e)
    {
        var minSize = Math.Min(progress.Height, progress.Width) - 50;

        imgBorder.HeightRequest = imgBorder.WidthRequest = minSize;

        var roundedRec = (RoundRectangle)imgBorder.StrokeShape!;

        roundedRec.CornerRadius = new(minSize / 2);
    }

    bool isPlaying;
    private void Button_OnClicked(object? sender, EventArgs e)
    {
        isPlaying = !isPlaying;
        RotateCover(isPlaying);
    }
    
    async void RotateCover(bool isPlaying)
    {
        if (isPlaying)
        {
            await StartCoverAnimation(animateTimerCancellationTokenSource.Token);
        }
        else
        {
            animateTimerCancellationTokenSource.Cancel();
            animateTimerCancellationTokenSource.Dispose();
            animateTimerCancellationTokenSource = new();
        }
    }

    async Task StartCoverAnimation(CancellationToken tokenSource)
    {
        try
        {
            while (!tokenSource.IsCancellationRequested)
            {
                await albumArt.RelRotateTo(360, CoverAnimationDuration, Easing.Linear);
            }

            albumArt.CancelAnimations();

        }
        catch (TaskCanceledException ex)
        {
            albumArt.CancelAnimations();
        }
    }

}