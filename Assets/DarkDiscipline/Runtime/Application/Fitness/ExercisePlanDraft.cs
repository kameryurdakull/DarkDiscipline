namespace DarkDiscipline.Application.Fitness
{
    public sealed class ExercisePlanDraft
    {
        public ExercisePlanDraft(string name, int setCount, int repetitions, float weightKg)
        {
            Name = name;
            SetCount = setCount;
            Repetitions = repetitions;
            WeightKg = weightKg;
        }

        public string Name { get; }
        public int SetCount { get; }
        public int Repetitions { get; }
        public float WeightKg { get; }
    }
}
