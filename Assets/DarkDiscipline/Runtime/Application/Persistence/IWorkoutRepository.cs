using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Persistence
{
    public interface IWorkoutRepository
    {
        UniTask SaveAsync(WorkoutSession session);
        UniTask<IReadOnlyList<WorkoutSession>> GetAllAsync();
    }
}
