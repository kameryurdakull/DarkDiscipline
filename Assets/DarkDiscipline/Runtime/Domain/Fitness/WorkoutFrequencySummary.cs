using System;

namespace DarkDiscipline.Domain.Fitness
{
    public sealed class WorkoutFrequencySummary
    {
        public WorkoutFrequencySummary(string exerciseName, int sessionCount, float averageDaysBetweenSessions, DateTime? lastPerformedAtUtc)
        {
            ExerciseName = exerciseName;
            SessionCount = sessionCount;
            AverageDaysBetweenSessions = averageDaysBetweenSessions;
            LastPerformedAtUtc = lastPerformedAtUtc;
        }

        public string ExerciseName { get; }
        public int SessionCount { get; }
        public float AverageDaysBetweenSessions { get; }
        public DateTime? LastPerformedAtUtc { get; }
    }
}
