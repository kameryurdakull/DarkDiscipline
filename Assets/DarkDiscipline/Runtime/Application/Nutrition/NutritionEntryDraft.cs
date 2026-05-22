namespace DarkDiscipline.Application.Nutrition
{
    public sealed class NutritionEntryDraft
    {
        public NutritionEntryDraft(string label, int calories, float proteinGrams, float carbohydrateGrams, float fatGrams)
        {
            Label = label;
            Calories = calories;
            ProteinGrams = proteinGrams;
            CarbohydrateGrams = carbohydrateGrams;
            FatGrams = fatGrams;
        }

        public string Label { get; }
        public int Calories { get; }
        public float ProteinGrams { get; }
        public float CarbohydrateGrams { get; }
        public float FatGrams { get; }
    }
}
