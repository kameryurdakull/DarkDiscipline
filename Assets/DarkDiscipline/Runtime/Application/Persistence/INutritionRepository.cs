using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Persistence
{
    public interface INutritionRepository
    {
        UniTask SaveAsync(NutritionEntry entry);
        UniTask<IReadOnlyList<NutritionEntry>> GetByDateAsync(DateKey date);
        UniTask RemoveAsync(string entryId);
        UniTask ResetByDateAsync(DateKey date);
    }
}
