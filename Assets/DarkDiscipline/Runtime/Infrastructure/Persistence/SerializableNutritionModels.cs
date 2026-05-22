using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Infrastructure.Persistence
{
    [Serializable]
    internal sealed class NutritionEntryStore
    {
        public List<NutritionEntryDto> Entries = new();
    }

    [Serializable]
    internal sealed class NutritionEntryDto
    {
        public string Id;
        public string ConsumedAtUtc;
        public string Label;
        public int Calories;
        public float ProteinGrams;
        public float CarbohydrateGrams;
        public float FatGrams;

        public static NutritionEntryDto FromDomain(NutritionEntry entry)
        {
            return new NutritionEntryDto
            {
                Id = entry.Id,
                ConsumedAtUtc = entry.ConsumedAtUtc.ToString("O"),
                Label = entry.Label,
                Calories = entry.Calories,
                ProteinGrams = entry.ProteinGrams,
                CarbohydrateGrams = entry.CarbohydrateGrams,
                FatGrams = entry.FatGrams
            };
        }

        public NutritionEntry ToDomain()
        {
            return new NutritionEntry(Id, DateTime.Parse(ConsumedAtUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime(), Label, Calories, ProteinGrams, CarbohydrateGrams, FatGrams);
        }
    }
}
