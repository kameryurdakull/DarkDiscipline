using DarkDiscipline.Domain.Common;

namespace DarkDiscipline.Domain.Nutrition
{
    public sealed class DailyNutritionSummary
    {
        public DailyNutritionSummary(DateKey date, int calories, float proteinGrams, float carbohydrateGrams, float fatGrams)
        {
            Date = date;
            Calories = calories;
            ProteinGrams = proteinGrams;
            CarbohydrateGrams = carbohydrateGrams;
            FatGrams = fatGrams;
        }

        public DateKey Date { get; }
        public int Calories { get; }
        public float ProteinGrams { get; }
        public float CarbohydrateGrams { get; }
        public float FatGrams { get; }
    }
}
