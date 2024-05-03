using System;
using DG.Tweening;
using UnityEngine;

public static class CameraTransitionAnimations
{
    public static Tween TpvToFpvTransition(
        Transform tpvCamera,
        Transform fpvCamera,
        Action fade,
        float transitionTime)
    {
        var finalPos = fpvCamera.position;
        var finalRotation = fpvCamera.rotation.eulerAngles;
        return DOTween.Sequence()
            .Join(tpvCamera.DOMove(finalPos, transitionTime))
            .Join(tpvCamera.DORotate(finalRotation, transitionTime * 0.3f)
                           .OnComplete(fade.Invoke));
    }
}