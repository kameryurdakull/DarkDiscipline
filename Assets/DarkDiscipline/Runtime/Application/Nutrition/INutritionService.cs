using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public interface INutritionService
    {
        UniTask<NutritionEntry> LogEntryAsync(NutritionEntryDraft draft);
        UniTask<DailyNutritionSummary> GetDailySummaryAsync(DateTime date);
        UniTask<IReadOnlyList<NutritionEntry>> GetEntriesByDateAsync(DateTime date);
        UniTask RemoveEntryAsync(string entryId);
        UniTask ResetEntriesForDateAsync(DateTime date);
    }
}
