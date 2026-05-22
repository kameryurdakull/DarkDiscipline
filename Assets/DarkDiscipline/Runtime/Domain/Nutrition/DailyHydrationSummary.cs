using DarkDiscipline.Domain.Common;

namespace DarkDiscipline.Domain.Nutrition
{
    public sealed class DailyHydrationSummary
    {
        public DailyHydrationSummary(DateKey date, float consumedLiters, float targetLiters)
        {
            Date = date;
            ConsumedLiters = consumedLiters;
            TargetLiters = targetLiters;
        }

        public DateKey Date { get; }
        public float ConsumedLiters { get; }
        public float TargetLiters { get; }
        public float RemainingLiters => TargetLiters > ConsumedLiters ? TargetLiters - ConsumedLiters : 0f;
    }
}
