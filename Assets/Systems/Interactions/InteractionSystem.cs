using UnityEngine;
using DG.Tweening;

public class InteractionSystem
{
    public static void AnimateButtonPressing(InteractiveButton button)
    {
        if (Time.timeScale == 0)
        {
            button.transform.DOScale(0.9f, 0.1f).SetUpdate(true).OnComplete(() =>
            {
                button.transform.DOScale(1f, 0.1f).SetUpdate(true).OnComplete(() =>
                {
                    button.onInteractionFinished.Invoke();
                    InteractiveButton.isAnimating = false;
                });
            });

            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(button.transform.DOScale(0.9f, 0.1f));
        sequence.Append(button.transform.DOScale(1f, 0.1f));
        sequence.AppendCallback(() =>
        {
            button.onInteractionFinished.Invoke();
        });
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(() =>
        {
            InteractiveButton.isAnimating = false;
        });
    }

    public static void AnimateButtonReleasing(InteractiveButton button)
    {
        button.transform.DOScale(1f, 0.1f).OnComplete(() => 
        {
            button.onInteractionFinished.Invoke();
        });
    }
}
