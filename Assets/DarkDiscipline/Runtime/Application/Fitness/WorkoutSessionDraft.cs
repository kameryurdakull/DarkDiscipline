using System.Collections.Generic;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public sealed class WorkoutSessionDraft
    {
        public WorkoutSessionDraft(IReadOnlyList<WorkoutExerciseRecord> exercises)
        {
            Exercises = exercises;
        }

        public IReadOnlyList<WorkoutExerciseRecord> Exercises { get; }
    }
}
