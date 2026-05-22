using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public readonly struct NutritionEntryLoggedEvent
    {
        public NutritionEntryLoggedEvent(NutritionEntry entry)
        {
            Entry = entry;
        }

        public NutritionEntry Entry { get; }
    }
}
