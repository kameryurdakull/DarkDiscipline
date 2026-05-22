using System;

namespace DarkDiscipline.Domain.Nutrition
{
    [Serializable]
    public sealed class NutritionEntry
    {
        public NutritionEntry(string id, DateTime consumedAtUtc, string label, int calories, float proteinGrams, float carbohydrateGrams, float fatGrams)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Entry id cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("Label cannot be empty.", nameof(label));
            }

            if (calories < 0 || proteinGrams < 0f || carbohydrateGrams < 0f || fatGrams < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(calories), "Nutrition values cannot be negative.");
            }

            Id = id;
            ConsumedAtUtc = consumedAtUtc.Kind == DateTimeKind.Utc ? consumedAtUtc : consumedAtUtc.ToUniversalTime();
            Label = label.Trim();
            Calories = calories;
            ProteinGrams = proteinGrams;
            CarbohydrateGrams = carbohydrateGrams;
            FatGrams = fatGrams;
        }

        public string Id { get; }
        public DateTime ConsumedAtUtc { get; }
        public string Label { get; }
        public int Calories { get; }
        public float ProteinGrams { get; }
        public float CarbohydrateGrams { get; }
        public float FatGrams { get; }
    }
}
