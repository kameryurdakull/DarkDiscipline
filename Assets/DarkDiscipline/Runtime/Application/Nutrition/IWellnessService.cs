using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public interface IWellnessService
    {
        UniTask<WellnessProfile> SaveProfileAsync(WellnessProfileDraft draft);
        UniTask<WellnessProfile> GetProfileAsync();
        UniTask<HydrationEntry> LogHydrationAsync(HydrationEntryDraft draft);
        UniTask<DailyHydrationSummary> GetHydrationSummaryAsync(DateTime date);
        UniTask<IReadOnlyList<HydrationEntry>> GetHydrationEntriesByDateAsync(DateTime date);
        UniTask RemoveHydrationEntryAsync(string entryId);
        UniTask ResetHydrationForDateAsync(DateTime date);
    }
}
