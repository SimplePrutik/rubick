using UnityEngine;

namespace Map
{
    public class CellObject : MonoBehaviour
    {
        [SerializeField] private GameObject top;
        [SerializeField] private GameObject bottom;
        [SerializeField] private GameObject left;
        [SerializeField] private GameObject right;
        [SerializeField] private GameObject front;
        [SerializeField] private GameObject back;

        public void SetWalls(MazeGenerator.Cell cellSettings)
        {
            top.SetActive(!cellSettings.Top);
            bottom.SetActive(!cellSettings.Bottom);
            left.SetActive(!cellSettings.Left);
            right.SetActive(!cellSettings.Right);
            front.SetActive(!cellSettings.Front);
            back.SetActive(!cellSettings.Back);
        }

        public void SetSize(Vector3 size)
        {
            transform.localScale = size;
        }
    }
}