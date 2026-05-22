using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace DarkDiscipline.Presentation.Animation
{
    public sealed class DotweenUiTweenAnimator : IUiTweenAnimator
    {
        public UniTask FadeAsync(CanvasGroup canvasGroup, float targetAlpha, float durationSeconds)
        {
            var completionSource = new UniTaskCompletionSource();
            canvasGroup
                .DOFade(targetAlpha, durationSeconds)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => completionSource.TrySetResult());

            return completionSource.Task;
        }
    }
}
