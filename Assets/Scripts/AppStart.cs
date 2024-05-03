using UI;
using Zenject;

public class AppStart : IInitializable
{
    private readonly ScreensService screensService;
    
    public AppStart(ScreensService screensService)
    {
        this.screensService = screensService;
    }
    
    public void Initialize()
    {
        screensService.ChangeScreen<GameScreen>();
    }
}