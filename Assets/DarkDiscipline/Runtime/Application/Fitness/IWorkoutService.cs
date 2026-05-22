using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public interface IWorkoutService
    {
        UniTask<WorkoutSession> LogWorkoutAsync(WorkoutSessionDraft draft);
        UniTask<WorkoutProgressionRecommendation> GetNextRecommendationAsync(string exerciseName);
        UniTask<WorkoutFrequencySummary> GetFrequencySummaryAsync(string exerciseName, int dayRange);
    }
}
