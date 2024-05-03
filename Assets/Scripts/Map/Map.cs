using UnityEngine;
using Zenject;

namespace Map
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            playerController.transform.SetParent(transform);
            playerController.transform.localPosition = spawnPoint.localPosition;
        }
    }
}