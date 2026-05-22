using System;

namespace DarkDiscipline.Domain.Fitness
{
    [Serializable]
    public sealed class ExercisePlan
    {
        public ExercisePlan(string id, string name, int setCount, int repetitions, float weightKg)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Exercise id cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Exercise name cannot be empty.", nameof(name));
            }

            if (setCount <= 0 || repetitions <= 0 || weightKg < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(setCount));
            }

            Id = id;
            Name = name.Trim();
            SetCount = setCount;
            Repetitions = repetitions;
            WeightKg = weightKg;
        }

        public string Id { get; }
        public string Name { get; }
        public int SetCount { get; }
        public int Repetitions { get; }
        public float WeightKg { get; }
    }
}
