using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractiveButton : Button
{
    public UnityEvent onInteractionFinished;
    public Sound soundEffect = Sound.Click;
    [HideInInspector] public static bool isAnimating;

    protected override void Start()
    {
        base.Start();

        onClick.AddListener(AnimateButtonPressing);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        onClick.RemoveListener(AnimateButtonPressing);
    }

    private void AnimateButtonPressing()
    {
        if (isAnimating) return;

        isAnimating = true;
        DependencyManager.audioManager.PlaySound(soundEffect);
        InteractionSystem.AnimateButtonPressing(this);
    }
}