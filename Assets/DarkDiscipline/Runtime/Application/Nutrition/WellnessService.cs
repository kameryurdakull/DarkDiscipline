using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public sealed class WellnessService : IWellnessService
    {
        private readonly IWellnessRepository _repository;

        public WellnessService(IWellnessRepository repository)
        {
            _repository = repository;
        }

        public async UniTask<WellnessProfile> SaveProfileAsync(WellnessProfileDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentNullException(nameof(draft));
            }

            var profile = new WellnessProfile(draft.BasalMetabolicRateCalories, draft.CalorieAdjustment, draft.DailyWaterTargetLiters);
            await _repository.SaveProfileAsync(profile);
            return profile;
        }

        public UniTask<WellnessProfile> GetProfileAsync()
        {
            return _repository.GetProfileAsync();
        }

        public async UniTask<HydrationEntry> LogHydrationAsync(HydrationEntryDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentNullException(nameof(draft));
            }

            var entry = new HydrationEntry(Guid.NewGuid().ToString("N"), DateTime.UtcNow, draft.Liters);
            await _repository.SaveHydrationAsync(entry);
            return entry;
        }

        public async UniTask<DailyHydrationSummary> GetHydrationSummaryAsync(DateTime date)
        {
            var profile = await _repository.GetProfileAsync();
            var dateKey = DateKey.FromDateTime(date);
            var entries = await _repository.GetHydrationByDateAsync(dateKey);
            return new DailyHydrationSummary(dateKey, entries.Sum(entry => entry.Liters), profile.DailyWaterTargetLiters);
        }

        public UniTask<IReadOnlyList<HydrationEntry>> GetHydrationEntriesByDateAsync(DateTime date)
        {
            return _repository.GetHydrationByDateAsync(DateKey.FromDateTime(date));
        }

        public UniTask RemoveHydrationEntryAsync(string entryId)
        {
            return _repository.RemoveHydrationAsync(entryId);
        }

        public UniTask ResetHydrationForDateAsync(DateTime date)
        {
            return _repository.ResetHydrationByDateAsync(DateKey.FromDateTime(date));
        }
    }
}
