using System;
using DG.Tweening;
using Zenject;

namespace UI
{
    public class ScreensService
    { 
        private readonly IFactory<Type, BaseScreen> screensFactory;

        private BaseScreen currentScreen;
        
        public ScreensService(IFactory<Type, BaseScreen> screensFactory)
        {
            this.screensFactory = screensFactory;
        }

        public void ChangeScreen<T>() where T : BaseScreen
        {
            currentScreen = screensFactory.Create(typeof(T));
            currentScreen.Show();
        }

        public Tween Fade(
            float fadeTime,
            Action callbackAfterFadeIn = null,
            Action callbackBeforeFadeOut = null,
            Action callbackAfterFadeOut = null)
        {
            return DOTween.Sequence()
                .Append(currentScreen.Fader.DOFade(1f, fadeTime * 0.3f))
                .AppendCallback(() => callbackAfterFadeIn?.Invoke())
                .AppendInterval(fadeTime * 0.4f)
                .AppendCallback(() => callbackBeforeFadeOut?.Invoke())
                .Append(currentScreen.Fader.DOFade(0f, fadeTime * 0.3f))
                .AppendCallback(() => callbackAfterFadeOut?.Invoke());
        }
    }
}