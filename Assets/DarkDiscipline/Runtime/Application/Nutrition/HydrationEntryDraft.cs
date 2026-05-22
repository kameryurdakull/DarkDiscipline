namespace DarkDiscipline.Application.Nutrition
{
    public sealed class HydrationEntryDraft
    {
        public HydrationEntryDraft(float liters)
        {
            Liters = liters;
        }

        public float Liters { get; }
    }
}
