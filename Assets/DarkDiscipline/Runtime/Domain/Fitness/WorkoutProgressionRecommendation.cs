namespace DarkDiscipline.Domain.Fitness
{
    public sealed class WorkoutProgressionRecommendation
    {
        public WorkoutProgressionRecommendation(string exerciseName, float recommendedWeightKg, int targetRepetitions, string reason)
        {
            ExerciseName = exerciseName;
            RecommendedWeightKg = recommendedWeightKg;
            TargetRepetitions = targetRepetitions;
            Reason = reason;
        }

        public string ExerciseName { get; }
        public float RecommendedWeightKg { get; }
        public int TargetRepetitions { get; }
        public string Reason { get; }
    }
}
