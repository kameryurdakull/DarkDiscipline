using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Common;
using DarkDiscipline.Domain.Nutrition;

namespace DarkDiscipline.Application.Nutrition
{
    public sealed class ReportService : IReportService
    {
        private readonly INutritionRepository _nutritionRepository;
        private readonly IWellnessRepository _wellnessRepository;

        public ReportService(INutritionRepository nutritionRepository, IWellnessRepository wellnessRepository)
        {
            _nutritionRepository = nutritionRepository;
            _wellnessRepository = wellnessRepository;
        }

        public async UniTask<NutritionReport> BuildNutritionReportAsync(ReportPeriod period, DateTime endDate)
        {
            var dayCount = (int)period;
            var points = new List<NutritionReportPoint>(dayCount);
            var normalizedEndDate = endDate.Date;

            for (var index = dayCount - 1; index >= 0; index--)
            {
                var date = normalizedEndDate.AddDays(-index);
                var dateKey = DateKey.FromDateTime(date);
                var nutritionEntries = await _nutritionRepository.GetByDateAsync(dateKey);
                var hydrationEntries = await _wellnessRepository.GetHydrationByDateAsync(dateKey);
                points.Add(new NutritionReportPoint(
                    dateKey,
                    nutritionEntries.Sum(entry => entry.Calories),
                    nutritionEntries.Sum(entry => entry.ProteinGrams),
                    nutritionEntries.Sum(entry => entry.CarbohydrateGrams),
                    nutritionEntries.Sum(entry => entry.FatGrams),
                    hydrationEntries.Sum(entry => entry.Liters)));
            }

            return new NutritionReport(period, points);
        }
    }
}
