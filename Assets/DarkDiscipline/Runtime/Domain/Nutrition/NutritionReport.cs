using System.Collections.Generic;

namespace DarkDiscipline.Domain.Nutrition
{
    public sealed class NutritionReport
    {
        public NutritionReport(ReportPeriod period, IReadOnlyList<NutritionReportPoint> points)
        {
            Period = period;
            Points = points;
        }

        public ReportPeriod Period { get; }
        public IReadOnlyList<NutritionReportPoint> Points { get; }
    }
}
