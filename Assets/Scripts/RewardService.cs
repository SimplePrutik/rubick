using Zenject;

public class RewardService
{
    private PlayerStatsController playerStatsController;
    
    [Inject]
    public void Construct(PlayerStatsController playerStatsController)
    {
        this.playerStatsController = playerStatsController;
    }
    
    public void GiveReward(int value)
    {
        playerStatsController.GoldCollected.Value++;
    }
}