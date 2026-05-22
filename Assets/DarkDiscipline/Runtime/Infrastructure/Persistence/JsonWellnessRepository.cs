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
    public sealed class JsonWellnessRepository : IWellnessRepository
    {
        private const string FileName = "wellness-profile.json";
        private const float DefaultBasalMetabolicRateCalories = 2000f;
        private const int DefaultCalorieAdjustment = 0;
        private const float DefaultDailyWaterTargetLiters = 2.5f;

        private readonly string _filePath;

        public JsonWellnessRepository()
        {
            _filePath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);
        }

        public async UniTask SaveProfileAsync(WellnessProfile profile)
        {
            var store = await LoadStoreAsync();
            store.Profile = WellnessProfileDto.FromDomain(profile);
            await SaveStoreAsync(store);
        }

        public async UniTask<WellnessProfile> GetProfileAsync()
        {
            var store = await LoadStoreAsync();
            return store.Profile == null
                ? new WellnessProfile(DefaultBasalMetabolicRateCalories, DefaultCalorieAdjustment, DefaultDailyWaterTargetLiters)
                : store.Profile.ToDomain();
        }

        public async UniTask SaveHydrationAsync(HydrationEntry entry)
        {
            var store = await LoadStoreAsync();
            store.HydrationEntries.Add(HydrationEntryDto.FromDomain(entry));
            await SaveStoreAsync(store);
        }

        public async UniTask<IReadOnlyList<HydrationEntry>> GetHydrationByDateAsync(DateKey date)
        {
            var store = await LoadStoreAsync();
            return store.HydrationEntries
                .Select(entry => entry.ToDomain())
                .Where(entry => DateKey.FromDateTime(entry.ConsumedAtUtc) == date)
                .ToArray();
        }

        public async UniTask RemoveHydrationAsync(string entryId)
        {
            var store = await LoadStoreAsync();
            store.HydrationEntries.RemoveAll(entry => entry.Id == entryId);
            await SaveStoreAsync(store);
        }

        public async UniTask ResetHydrationByDateAsync(DateKey date)
        {
            var store = await LoadStoreAsync();
            store.HydrationEntries.RemoveAll(entry => DateKey.FromDateTime(entry.ToDomain().ConsumedAtUtc) == date);
            await SaveStoreAsync(store);
        }

        private async UniTask<WellnessStore> LoadStoreAsync()
        {
            if (File.Exists(_filePath) == false)
            {
                return new WellnessStore();
            }

            await UniTask.SwitchToThreadPool();
            var json = File.ReadAllText(_filePath);
            await UniTask.SwitchToMainThread();
            var store = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<WellnessStore>(json);
            return store ?? new WellnessStore();
        }

        private async UniTask SaveStoreAsync(WellnessStore store)
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
