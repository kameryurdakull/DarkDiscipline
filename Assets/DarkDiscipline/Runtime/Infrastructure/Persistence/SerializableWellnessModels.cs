using System;
using System.Collections.Generic;
using System.Globalization;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Infrastructure.Persistence
{
    [Serializable]
    internal sealed class WellnessStore
    {
        public WellnessProfileDto Profile;
        public List<HydrationEntryDto> HydrationEntries = new();
    }

    [Serializable]
    internal sealed class WellnessProfileDto
    {
        public float BasalMetabolicRateCalories;
        public int CalorieAdjustment;
        public float DailyWaterTargetLiters;

        public static WellnessProfileDto FromDomain(WellnessProfile profile)
        {
            return new WellnessProfileDto
            {
                BasalMetabolicRateCalories = profile.BasalMetabolicRateCalories,
                CalorieAdjustment = profile.CalorieAdjustment,
                DailyWaterTargetLiters = profile.DailyWaterTargetLiters
            };
        }

        public WellnessProfile ToDomain()
        {
            return new WellnessProfile(BasalMetabolicRateCalories, CalorieAdjustment, DailyWaterTargetLiters);
        }
    }

    [Serializable]
    internal sealed class HydrationEntryDto
    {
        public string Id;
        public string ConsumedAtUtc;
        public float Liters;

        public static HydrationEntryDto FromDomain(HydrationEntry entry)
        {
            return new HydrationEntryDto
            {
                Id = entry.Id,
                ConsumedAtUtc = entry.ConsumedAtUtc.ToString("O"),
                Liters = entry.Liters
            };
        }

        public HydrationEntry ToDomain()
        {
            return new HydrationEntry(Id, DateTime.Parse(ConsumedAtUtc, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToUniversalTime(), Liters);
        }
    }
}
