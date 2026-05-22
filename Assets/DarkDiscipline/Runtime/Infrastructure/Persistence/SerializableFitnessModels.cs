using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Infrastructure.Persistence
{
    [Serializable]
    internal sealed class WorkoutSessionStore
    {
        public List<WorkoutSessionDto> Sessions = new();
    }

    [Serializable]
    internal sealed class WorkoutSessionDto
    {
        public string Id;
        public string PerformedAtUtc;
        public List<WorkoutExerciseDto> Exercises = new();

        public static WorkoutSessionDto FromDomain(WorkoutSession session)
        {
            return new WorkoutSessionDto
            {
                Id = session.Id,
                PerformedAtUtc = session.PerformedAtUtc.ToString("O"),
                Exercises = session.Exercises.Select(WorkoutExerciseDto.FromDomain).ToList()
            };
        }

        public WorkoutSession ToDomain()
        {
            return new WorkoutSession(Id, DateTime.Parse(PerformedAtUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime(), Exercises.Select(exercise => exercise.ToDomain()).ToArray());
        }
    }

    [Serializable]
    internal sealed class WorkoutExerciseDto
    {
        public string ExerciseName;
        public int TargetRepetitions;
        public List<ExerciseSetDto> Sets = new();

        public static WorkoutExerciseDto FromDomain(WorkoutExerciseRecord exercise)
        {
            return new WorkoutExerciseDto
            {
                ExerciseName = exercise.ExerciseName,
                TargetRepetitions = exercise.TargetRepetitions,
                Sets = exercise.Sets.Select(ExerciseSetDto.FromDomain).ToList()
            };
        }

        public WorkoutExerciseRecord ToDomain()
        {
            return new WorkoutExerciseRecord(ExerciseName, TargetRepetitions, Sets.Select(set => set.ToDomain()).ToArray());
        }
    }

    [Serializable]
    internal sealed class ExerciseSetDto
    {
        public int Repetitions;
        public float WeightKg;
        public float RateOfPerceivedExertion;

        public static ExerciseSetDto FromDomain(ExerciseSetRecord set)
        {
            return new ExerciseSetDto
            {
                Repetitions = set.Repetitions,
                WeightKg = set.WeightKg,
                RateOfPerceivedExertion = set.RateOfPerceivedExertion
            };
        }

        public ExerciseSetRecord ToDomain()
        {
            return new ExerciseSetRecord(Repetitions, WeightKg, RateOfPerceivedExertion);
        }
    }
}
