using System;

namespace DarkDiscipline.Domain.Nutrition
{
    [Serializable]
    public sealed class WellnessProfile
    {
        public WellnessProfile(float basalMetabolicRateCalories, int calorieAdjustment, float dailyWaterTargetLiters)
        {
            if (basalMetabolicRateCalories <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(basalMetabolicRateCalories));
            }

            if (dailyWaterTargetLiters <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(dailyWaterTargetLiters));
            }

            BasalMetabolicRateCalories = basalMetabolicRateCalories;
            CalorieAdjustment = calorieAdjustment;
            DailyWaterTargetLiters = dailyWaterTargetLiters;
        }

        public float BasalMetabolicRateCalories { get; }
        public int CalorieAdjustment { get; }
        public float DailyWaterTargetLiters { get; }
        public float DailyCalorieTarget => BasalMetabolicRateCalories + CalorieAdjustment;
    }
}
