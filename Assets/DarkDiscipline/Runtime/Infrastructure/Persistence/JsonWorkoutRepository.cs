using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Fitness;
using UnityEngine;

namespace DarkDiscipline.Infrastructure.Persistence
{
    public sealed class JsonWorkoutRepository : IWorkoutRepository
    {
        private const string FileName = "workout-sessions.json";
        private readonly string _filePath;

        public JsonWorkoutRepository()
        {
            _filePath = Path.Combine(UnityEngine.Application.persistentDataPath, FileName);
        }

        public async UniTask SaveAsync(WorkoutSession session)
        {
            var store = await LoadStoreAsync();
            store.Sessions.Add(WorkoutSessionDto.FromDomain(session));
            await SaveStoreAsync(store);
        }

        public async UniTask<IReadOnlyList<WorkoutSession>> GetAllAsync()
        {
            var store = await LoadStoreAsync();
            return store.Sessions.Select(session => session.ToDomain()).ToArray();
        }

        private async UniTask<WorkoutSessionStore> LoadStoreAsync()
        {
            if (File.Exists(_filePath) == false)
            {
                return new WorkoutSessionStore();
            }

            await UniTask.SwitchToThreadPool();
            var json = File.ReadAllText(_filePath);
            await UniTask.SwitchToMainThread();
            var store = string.IsNullOrWhiteSpace(json) ? null : JsonUtility.FromJson<WorkoutSessionStore>(json);
            return store ?? new WorkoutSessionStore();
        }

        private async UniTask SaveStoreAsync(WorkoutSessionStore store)
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
