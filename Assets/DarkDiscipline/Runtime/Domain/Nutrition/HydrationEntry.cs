using System;

namespace DarkDiscipline.Domain.Nutrition
{
    [Serializable]
    public sealed class HydrationEntry
    {
        public HydrationEntry(string id, DateTime consumedAtUtc, float liters)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Entry id cannot be empty.", nameof(id));
            }

            if (liters <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(liters));
            }

            Id = id;
            ConsumedAtUtc = consumedAtUtc.Kind == DateTimeKind.Utc ? consumedAtUtc : consumedAtUtc.ToUniversalTime();
            Liters = liters;
        }

        public string Id { get; }
        public DateTime ConsumedAtUtc { get; }
        public float Liters { get; }
    }
}
