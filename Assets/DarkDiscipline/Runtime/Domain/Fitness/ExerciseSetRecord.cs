using System;

namespace DarkDiscipline.Domain.Fitness
{
    [Serializable]
    public sealed class ExerciseSetRecord
    {
        public ExerciseSetRecord(int repetitions, float weightKg, float rateOfPerceivedExertion)
        {
            if (repetitions < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repetitions));
            }

            if (weightKg < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(weightKg));
            }

            if (rateOfPerceivedExertion < 0f || rateOfPerceivedExertion > 10f)
            {
                throw new ArgumentOutOfRangeException(nameof(rateOfPerceivedExertion));
            }

            Repetitions = repetitions;
            WeightKg = weightKg;
            RateOfPerceivedExertion = rateOfPerceivedExertion;
        }

        public int Repetitions { get; }
        public float WeightKg { get; }
        public float RateOfPerceivedExertion { get; }
    }
}
