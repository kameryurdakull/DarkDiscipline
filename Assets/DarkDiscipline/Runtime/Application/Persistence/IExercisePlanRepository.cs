using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Persistence
{
    public interface IExercisePlanRepository
    {
        UniTask SaveAsync(ExercisePlan plan);
        UniTask RemoveAsync(string exerciseId);
        UniTask<IReadOnlyList<ExercisePlan>> GetAllAsync();
    }
}
