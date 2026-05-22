using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DarkDiscipline.Presentation.Animation
{
    public interface IUiTweenAnimator
    {
        UniTask FadeAsync(CanvasGroup canvasGroup, float targetAlpha, float durationSeconds);
    }
}
