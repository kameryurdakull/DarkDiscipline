using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Events;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public sealed class NutritionService : INutritionService
    {
        private readonly INutritionRepository _repository;
        private readonly IEventBus _eventBus;

        public NutritionService(INutritionRepository repository, IEventBus eventBus)
        {
            _repository = repository;
            _eventBus = eventBus;
        }

        public async UniTask<NutritionEntry> LogEntryAsync(NutritionEntryDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentNullException(nameof(draft));
            }

            var entry = new NutritionEntry(Guid.NewGuid().ToString("N"), DateTime.UtcNow, draft.Label, draft.Calories, draft.ProteinGrams, draft.CarbohydrateGrams, draft.FatGrams);
            await _repository.SaveAsync(entry);
            _eventBus.Publish(new NutritionEntryLoggedEvent(entry));
            return entry;
        }

        public async UniTask<DailyNutritionSummary> GetDailySummaryAsync(DateTime date)
        {
            var dateKey = DateKey.FromDateTime(date);
            var entries = await _repository.GetByDateAsync(dateKey);
            return new DailyNutritionSummary(
                dateKey,
                entries.Sum(entry => entry.Calories),
                entries.Sum(entry => entry.ProteinGrams),
                entries.Sum(entry => entry.CarbohydrateGrams),
                entries.Sum(entry => entry.FatGrams));
        }

        public UniTask<IReadOnlyList<NutritionEntry>> GetEntriesByDateAsync(DateTime date)
        {
            return _repository.GetByDateAsync(DateKey.FromDateTime(date));
        }

        public UniTask RemoveEntryAsync(string entryId)
        {
            return _repository.RemoveAsync(entryId);
        }

        public UniTask ResetEntriesForDateAsync(DateTime date)
        {
            return _repository.ResetByDateAsync(DateKey.FromDateTime(date));
        }
    }
}
