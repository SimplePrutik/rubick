using System;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;

namespace UI
{
    public class ScreensService
    { 
        private readonly IFactory<Type, BaseScreen> screensFactory;
        private Dictionary<Type, BaseScreen> screenCache = new Dictionary<Type, BaseScreen>();
        public BaseScreen CurrentScreen { get; private set; }
        
        public ScreensService(IFactory<Type, BaseScreen> screensFactory)
        {
            this.screensFactory = screensFactory;
        }

        public void ChangeScreen<T>() where T : BaseScreen
        {
            var screenType = typeof(T);
            if (screenCache.ContainsKey(screenType))
                CurrentScreen = screenCache[screenType] as T;
            CurrentScreen = screensFactory.Create(typeof(T));
            screenCache[screenType] = CurrentScreen;
            CurrentScreen.Show();
        }

        public Tween Fade(
            float fadeTime,
            Action callbackAfterFadeIn = null,
            Action callbackBeforeFadeOut = null,
            Action callbackAfterFadeOut = null)
        {
            return DOTween.Sequence()
                .Append(CurrentScreen.Fader.DOFade(1f, fadeTime * 0.3f))
                .AppendCallback(() => callbackAfterFadeIn?.Invoke())
                .AppendInterval(fadeTime * 0.4f)
                .AppendCallback(() => callbackBeforeFadeOut?.Invoke())
                .Append(CurrentScreen.Fader.DOFade(0f, fadeTime * 0.3f))
                .AppendCallback(() => callbackAfterFadeOut?.Invoke());
        }
    }
}