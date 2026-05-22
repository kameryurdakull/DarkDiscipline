using System;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Fitness;

namespace DarkDiscipline.Application.Fitness
{
    public sealed class ExercisePlanService : IExercisePlanService
    {
        private readonly IExercisePlanRepository _repository;

        public ExercisePlanService(IExercisePlanRepository repository)
        {
            _repository = repository;
        }

        public async UniTask<ExercisePlan> SaveAsync(ExercisePlanDraft draft)
        {
            if (draft == null)
            {
                throw new ArgumentNullException(nameof(draft));
            }

            var plan = new ExercisePlan(Guid.NewGuid().ToString("N"), draft.Name, draft.SetCount, draft.Repetitions, draft.WeightKg);
            await _repository.SaveAsync(plan);
            return plan;
        }

        public UniTask RemoveAsync(string exerciseId)
        {
            return _repository.RemoveAsync(exerciseId);
        }

        public UniTask<System.Collections.Generic.IReadOnlyList<ExercisePlan>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }
    }
}
