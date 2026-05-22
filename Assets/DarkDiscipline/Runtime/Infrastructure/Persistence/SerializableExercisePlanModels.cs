using System;
using System.Collections.Generic;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Infrastructure.Persistence
{
    [Serializable]
    internal sealed class ExercisePlanStore
    {
        public List<ExercisePlanDto> Plans = new();
    }

    [Serializable]
    internal sealed class ExercisePlanDto
    {
        public string Id;
        public string Name;
        public int SetCount;
        public int Repetitions;
        public float WeightKg;

        public static ExercisePlanDto FromDomain(ExercisePlan plan)
        {
            return new ExercisePlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                SetCount = plan.SetCount,
                Repetitions = plan.Repetitions,
                WeightKg = plan.WeightKg
            };
        }

        public ExercisePlan ToDomain()
        {
            return new ExercisePlan(Id, Name, SetCount, Repetitions, WeightKg);
        }
    }
}
