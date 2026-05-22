using System;
using System.Linq;
using DarkDiscipline.Domain.Fitness;
using DarkDiscipline.Runtime;

namespace DarkDiscipline.Application.Fitness
{
    public sealed class WorkoutProgressionCalculator : IWorkoutProgressionCalculator
    {
        private const string NoHistoryReason = "No previous workout found. Start with a controlled baseline.";
        private const string IncreaseReason = "All sets reached the target repetitions. Increase load.";
        private const string MaintainReason = "Target repetitions were not consistently reached. Keep the load steady.";

        private readonly DarkDisciplineSettings _settings;

        public WorkoutProgressionCalculator(DarkDisciplineSettings settings)
        {
            _settings = settings;
        }

        public WorkoutProgressionRecommendation Calculate(string exerciseName, System.Collections.Generic.IReadOnlyList<WorkoutSession> history)
        {
            if (string.IsNullOrWhiteSpace(exerciseName))
            {
                throw new ArgumentException("Exercise name cannot be empty.", nameof(exerciseName));
            }

            var latestExercise = history
                .OrderByDescending(session => session.PerformedAtUtc)
                .SelectMany(session => session.Exercises)
                .FirstOrDefault(exercise => string.Equals(exercise.ExerciseName, exerciseName, StringComparison.OrdinalIgnoreCase));

            if (latestExercise == null)
            {
                return new WorkoutProgressionRecommendation(exerciseName.Trim(), _settings.DefaultStartingWeightKg, _settings.DefaultTargetRepetitions, NoHistoryReason);
            }

            var nextWeight = latestExercise.HitTargetRepetitions
                ? latestExercise.AverageWeightKg + _settings.WeightProgressionStepKg
                : latestExercise.AverageWeightKg;

            var reason = latestExercise.HitTargetRepetitions ? IncreaseReason : MaintainReason;
            return new WorkoutProgressionRecommendation(latestExercise.ExerciseName, nextWeight, latestExercise.TargetRepetitions, reason);
        }
    }
}
