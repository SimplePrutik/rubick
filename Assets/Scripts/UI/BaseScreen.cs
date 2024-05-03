using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField] private Image fader;

        public Image Fader => fader;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}