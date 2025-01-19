using UniRx;
using Zenject;

public class PlayerStatsController
{
    public ReactiveProperty<int> GoldCollected = new ReactiveProperty<int>();

    [Inject]
    public void Construct()
    {
        GoldCollected.Value = 0;
    }
}