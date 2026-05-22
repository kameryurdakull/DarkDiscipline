using System.Collections.Generic;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public interface IWorkoutProgressionCalculator
    {
        WorkoutProgressionRecommendation Calculate(string exerciseName, IReadOnlyList<WorkoutSession> history);
    }
}
