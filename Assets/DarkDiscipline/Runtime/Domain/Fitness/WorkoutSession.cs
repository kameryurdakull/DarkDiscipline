using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkDiscipline.Domain.Fitness
{
    [Serializable]
    public sealed class WorkoutSession
    {
        public WorkoutSession(string id, DateTime performedAtUtc, IReadOnlyList<WorkoutExerciseRecord> exercises)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Session id cannot be empty.", nameof(id));
            }

            if (exercises == null || exercises.Count == 0)
            {
                throw new ArgumentException("At least one exercise is required.", nameof(exercises));
            }

            Id = id;
            PerformedAtUtc = performedAtUtc.Kind == DateTimeKind.Utc ? performedAtUtc : performedAtUtc.ToUniversalTime();
            Exercises = exercises.ToArray();
        }

        public string Id { get; }
        public DateTime PerformedAtUtc { get; }
        public IReadOnlyList<WorkoutExerciseRecord> Exercises { get; }
    }
}
