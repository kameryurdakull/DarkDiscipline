using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Persistence
{
    public interface IWellnessRepository
    {
        UniTask SaveProfileAsync(WellnessProfile profile);
        UniTask<WellnessProfile> GetProfileAsync();
        UniTask SaveHydrationAsync(HydrationEntry entry);
        UniTask<IReadOnlyList<HydrationEntry>> GetHydrationByDateAsync(DateKey date);
        UniTask RemoveHydrationAsync(string entryId);
        UniTask ResetHydrationByDateAsync(DateKey date);
    }
}
