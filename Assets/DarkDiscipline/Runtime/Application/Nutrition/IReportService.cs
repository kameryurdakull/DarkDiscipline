using System;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public interface IReportService
    {
        UniTask<NutritionReport> BuildNutritionReportAsync(ReportPeriod period, DateTime endDate);
    }
}
