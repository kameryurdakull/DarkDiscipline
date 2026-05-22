using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Fitness;
using UnityEngine;

namespace DarkDiscipline.Infrastructure.Persistence
{
    public sealed class JsonExercisePlanRepository : IExercisePlanRepository
    {
        private const string FileName = "exercise-plans.json";
        private readonly string _filePath;

        public JsonExercisePlanRepository()
        {
            _filePath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);
        }

        public async UniTask SaveAsync(ExercisePlan plan)
        {
            var store = await LoadStoreAsync();
            store.Plans.RemoveAll(existing => existing.Id == plan.Id || string.Equals(existing.Name, plan.Name, System.StringComparison.OrdinalIgnoreCase));
            store.Plans.Add(ExercisePlanDto.FromDomain(plan));
            await SaveStoreAsync(store);
        }

        public async UniTask RemoveAsync(string exerciseId)
        {
            var store = await LoadStoreAsync();
            store.Plans.RemoveAll(plan => plan.Id == exerciseId);
            await SaveStoreAsync(store);
        }

        public async UniTask<IReadOnlyList<ExercisePlan>> GetAllAsync()
        {
            var store = await LoadStoreAsync();
            return store.Plans.Select(plan => plan.ToDomain()).OrderBy(plan => plan.Name).ToArray();
        }

        private async UniTask<ExercisePlanStore> LoadStoreAsync()
        {
            if (File.Exists(_filePath) == false)
            {
                return new ExercisePlanStore();
            }

            await UniTask.SwitchToThreadPool();
            var json = File.ReadAllText(_filePath);
            await UniTask.SwitchToMainThread();
            var store = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<ExercisePlanStore>(json);
            return store ?? new ExercisePlanStore();
        }

        private async UniTask SaveStoreAsync(ExercisePlanStore store)
        {
            var json = JsonUtility.ToJson(store, true);
            var directoryPath = Path.GetDirectoryName(_filePath);

            await UniTask.SwitchToThreadPool();

            if (string.IsNullOrWhiteSpace(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(_filePath, json);
            await UniTask.SwitchToMainThread();
        }
    }
}
