using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkDiscipline.Domain.Fitness
{
    [Serializable]
    public sealed class WorkoutExerciseRecord
    {
        public WorkoutExerciseRecord(string exerciseName, int targetRepetitions, IReadOnlyList<ExerciseSetRecord> sets)
        {
            if (string.IsNullOrWhiteSpace(exerciseName))
            {
                throw new ArgumentException("Exercise name cannot be empty.", nameof(exerciseName));
            }

            if (targetRepetitions <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetRepetitions));
            }

            if (sets == null || sets.Count == 0)
            {
                throw new ArgumentException("At least one set is required.", nameof(sets));
            }

            ExerciseName = exerciseName.Trim();
            TargetRepetitions = targetRepetitions;
            Sets = sets.ToArray();
        }

        public string ExerciseName { get; }
        public int TargetRepetitions { get; }
        public IReadOnlyList<ExerciseSetRecord> Sets { get; }

        public float AverageWeightKg => Sets.Count == 0 ? 0f : Sets.Average(set => set.WeightKg);
        public int TotalRepetitions => Sets.Sum(set => set.Repetitions);
        public bool HitTargetRepetitions => Sets.All(set => set.Repetitions >= TargetRepetitions);
    }
}
