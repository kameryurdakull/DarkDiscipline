using UnityEngine;

namespace DarkDiscipline.Runtime
{
    [CreateAssetMenu(menuName = "Dark Discipline/Settings", fileName = nameof(DarkDisciplineSettings))]
    public sealed class DarkDisciplineSettings : ScriptableObject
    {
        [Header("Workout")]
        [SerializeField] private float defaultStartingWeightKg = 20f;
        [SerializeField] private int defaultTargetRepetitions = 8;
        [SerializeField] private float weightProgressionStepKg = 2.5f;

        [Header("Nutrition")]
        [SerializeField] private int dailyCalorieTarget = 2400;
        [SerializeField] private float dailyProteinTargetGrams = 160f;
        [SerializeField] private float dailyCarbohydrateTargetGrams = 260f;
        [SerializeField] private float dailyFatTargetGrams = 70f;

        public float DefaultStartingWeightKg => defaultStartingWeightKg;
        public int DefaultTargetRepetitions => defaultTargetRepetitions;
        public float WeightProgressionStepKg => weightProgressionStepKg;
        public int DailyCalorieTarget => dailyCalorieTarget;
        public float DailyProteinTargetGrams => dailyProteinTargetGrams;
        public float DailyCarbohydrateTargetGrams => dailyCarbohydrateTargetGrams;
        public float DailyFatTargetGrams => dailyFatTargetGrams;
    }
}
