namespace DarkDiscipline.Application.Nutrition
{
    public sealed class WellnessProfileDraft
    {
        public WellnessProfileDraft(float basalMetabolicRateCalories, int calorieAdjustment, float dailyWaterTargetLiters)
        {
            BasalMetabolicRateCalories = basalMetabolicRateCalories;
            CalorieAdjustment = calorieAdjustment;
            DailyWaterTargetLiters = dailyWaterTargetLiters;
        }

        public float BasalMetabolicRateCalories { get; }
        public int CalorieAdjustment { get; }
        public float DailyWaterTargetLiters { get; }
    }
}
