using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Events;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public sealed class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _repository;
        private readonly IWorkoutProgressionCalculator _progressionCalculator;
        private readonly IEventBus _eventBus;

        public WorkoutService(IWorkoutRepository repository, IWorkoutProgressionCalculator progressionCalculator, IEventBus eventBus)
        {
            _repository = repository;
            _progressionCalculator = progressionCalculator;
            _eventBus = eventBus;
        }

        public async UniTask<WorkoutSession> LogWorkoutAsync(WorkoutSessionDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentNullException(nameof(draft));
            }

            var session = new WorkoutSession(Guid.NewGuid().ToString("N"), DateTime.UtcNow, draft.Exercises);
            await _repository.SaveAsync(session);
            _eventBus.Publish(new WorkoutSessionLoggedEvent(session));
            return session;
        }

        public async UniTask<WorkoutProgressionRecommendation> GetNextRecommendationAsync(string exerciseName)
        {
            var history = await _repository.GetAllAsync();
            return _progressionCalculator.Calculate(exerciseName, history);
        }

        public async UniTask<WorkoutFrequencySummary> GetFrequencySummaryAsync(string exerciseName, int dayRange)
        {
            if (string.IsNullOrWhiteSpace(exerciseName))
            {
                throw new ArgumentException("Exercise name cannot be empty.", nameof(exerciseName));
            }

            if (dayRange <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dayRange));
            }

            var lowerBoundUtc = DateTime.UtcNow.AddDays(-dayRange);
            var sessionDates = (await _repository.GetAllAsync())
                .Where(session => session.PerformedAtUtc >= lowerBoundUtc)
                .Where(session => session.Exercises.Any(exercise => string.Equals(exercise.ExerciseName, exerciseName, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(session => session.PerformedAtUtc)
                .Select(session => session.PerformedAtUtc)
                .ToArray();

            if (sessionDates.Length <= 1)
            {
                var lastPerformedAtUtc = sessionDates.Length == 0 ? (DateTime?)null : sessionDates[0];
                return new WorkoutFrequencySummary(exerciseName.Trim(), sessionDates.Length, 0f, lastPerformedAtUtc);
            }

            var averageDays = sessionDates
                .Skip(1)
                .Select((date, index) => (date - sessionDates[index]).TotalDays)
                .Average();

            return new WorkoutFrequencySummary(exerciseName.Trim(), sessionDates.Length, (float)averageDays, sessionDates[sessionDates.Length - 1]);
        }
    }
}
