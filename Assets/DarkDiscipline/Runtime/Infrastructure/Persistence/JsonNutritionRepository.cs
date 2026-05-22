using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;
using UnityEngine;

namespace DarkDiscipline.Infrastructure.Persistence
{
    public sealed class JsonNutritionRepository : INutritionRepository
    {
        private const string FileName = "nutrition-entries.json";
        private readonly string _filePath;

        public JsonNutritionRepository()
        {
            _filePath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);
        }

        public async UniTask SaveAsync(NutritionEntry entry)
        {
            var store = await LoadStoreAsync();
            store.Entries.Add(NutritionEntryDto.FromDomain(entry));
            await SaveStoreAsync(store);
        }

        public async UniTask<IReadOnlyList<NutritionEntry>> GetByDateAsync(DateKey date)
        {
            var store = await LoadStoreAsync();
            return store.Entries
                .Select(entry => entry.ToDomain())
                .Where(entry => DateKey.FromDateTime(entry.ConsumedAtUtc) == date)
                .ToArray();
        }

        public async UniTask RemoveAsync(string entryId)
        {
            var store = await LoadStoreAsync();
            store.Entries.RemoveAll(entry => entry.Id == entryId);
            await SaveStoreAsync(store);
        }

        public async UniTask ResetByDateAsync(DateKey date)
        {
            var store = await LoadStoreAsync();
            store.Entries.RemoveAll(entry => DateKey.FromDateTime(entry.ToDomain().ConsumedAtUtc) == date);
            await SaveStoreAsync(store);
        }

        private async UniTask<NutritionEntryStore> LoadStoreAsync()
        {
            if (File.Exists(_filePath) == false)
            {
                return new NutritionEntryStore();
            }

            await UniTask.SwitchToThreadPool();
            var json = File.ReadAllText(_filePath);
            await UniTask.SwitchToMainThread();
            var store = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<NutritionEntryStore>(json);
            return store ?? new NutritionEntryStore();
        }

        private async UniTask SaveStoreAsync(NutritionEntryStore store)
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
