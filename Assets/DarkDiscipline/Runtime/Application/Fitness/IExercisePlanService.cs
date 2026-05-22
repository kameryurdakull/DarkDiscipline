using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public interface IExercisePlanService
    {
        UniTask<ExercisePlan> SaveAsync(ExercisePlanDraft draft);
        UniTask RemoveAsync(string exerciseId);
        UniTask<IReadOnlyList<ExercisePlan>> GetAllAsync();
    }
}
