using DarkDiscipline.Domain.Common;

namespace DarkDiscipline.Domain.Nutrition
{
    public sealed class NutritionReportPoint
    {
        public NutritionReportPoint(DateKey date, float calories, float proteinGrams, float carbohydrateGrams, float fatGrams, float waterLiters)
        {
            Date = date;
            Calories = calories;
            ProteinGrams = proteinGrams;
            CarbohydrateGrams = carbohydrateGrams;
            FatGrams = fatGrams;
            WaterLiters = waterLiters;
        }

        public DateKey Date { get; }
        public float Calories { get; }
        public float ProteinGrams { get; }
        public float CarbohydrateGrams { get; }
        public float FatGrams { get; }
        public float WaterLiters { get; }
    }
}
