using System;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Fitness;
using DarkDiscipline.Application.Nutrition;
using DarkDiscipline.Domain.Fitness;
using DarkDiscipline.Domain.Nutrition;
using System.Collections.Generic;

namespace DarkDiscipline.Runtime
{
    public sealed class DarkDisciplineRuntimeApi
    {
        private readonly IExercisePlanService _exercisePlanService;
        private readonly IWorkoutService _workoutService;
        private readonly INutritionService _nutritionService;
        private readonly IWellnessService _wellnessService;
        private readonly IReportService _reportService;

        public DarkDisciplineRuntimeApi(
            IExercisePlanService exercisePlanService,
            IWorkoutService workoutService,
            INutritionService nutritionService,
            IWellnessService wellnessService,
            IReportService reportService)
        {
            _exercisePlanService = exercisePlanService;
            _workoutService = workoutService;
            _nutritionService = nutritionService;
            _wellnessService = wellnessService;
            _reportService = reportService;
        }

        public UniTask<ExercisePlan> SaveExercisePlanAsync(ExercisePlanDraft draft)
        {
            return _exercisePlanService.SaveAsync(draft);
        }

        public UniTask RemoveExercisePlanAsync(string exerciseId)
        {
            return _exercisePlanService.RemoveAsync(exerciseId);
        }

        public UniTask<IReadOnlyList<ExercisePlan>> GetExercisePlansAsync()
        {
            return _exercisePlanService.GetAllAsync();
        }

        public UniTask<WorkoutSession> LogWorkoutAsync(WorkoutSessionDraft draft)
        {
            return _workoutService.LogWorkoutAsync(draft);
        }

        public UniTask<WorkoutProgressionRecommendation> GetNextWorkoutRecommendationAsync(string exerciseName)
        {
            return _workoutService.GetNextRecommendationAsync(exerciseName);
        }

        public UniTask<WorkoutFrequencySummary> GetWorkoutFrequencySummaryAsync(string exerciseName, int dayRange)
        {
            return _workoutService.GetFrequencySummaryAsync(exerciseName, dayRange);
        }

        public UniTask<NutritionEntry> LogNutritionAsync(NutritionEntryDraft draft)
        {
            return _nutritionService.LogEntryAsync(draft);
        }

        public UniTask<DailyNutritionSummary> GetNutritionSummaryAsync(DateTime date)
        {
            return _nutritionService.GetDailySummaryAsync(date);
        }

        public UniTask<IReadOnlyList<NutritionEntry>> GetNutritionEntriesByDateAsync(DateTime date)
        {
            return _nutritionService.GetEntriesByDateAsync(date);
        }

        public UniTask RemoveNutritionEntryAsync(string entryId)
        {
            return _nutritionService.RemoveEntryAsync(entryId);
        }

        public UniTask ResetNutritionForDateAsync(DateTime date)
        {
            return _nutritionService.ResetEntriesForDateAsync(date);
        }

        public UniTask<WellnessProfile> SaveWellnessProfileAsync(WellnessProfileDraft draft)
        {
            return _wellnessService.SaveProfileAsync(draft);
        }

        public UniTask<WellnessProfile> GetWellnessProfileAsync()
        {
            return _wellnessService.GetProfileAsync();
        }

        public UniTask<HydrationEntry> LogHydrationAsync(HydrationEntryDraft draft)
        {
            return _wellnessService.LogHydrationAsync(draft);
        }

        public UniTask<DailyHydrationSummary> GetHydrationSummaryAsync(DateTime date)
        {
            return _wellnessService.GetHydrationSummaryAsync(date);
        }

        public UniTask<IReadOnlyList<HydrationEntry>> GetHydrationEntriesByDateAsync(DateTime date)
        {
            return _wellnessService.GetHydrationEntriesByDateAsync(date);
        }

        public UniTask RemoveHydrationEntryAsync(string entryId)
        {
            return _wellnessService.RemoveHydrationEntryAsync(entryId);
        }

        public UniTask ResetHydrationForDateAsync(DateTime date)
        {
            return _wellnessService.ResetHydrationForDateAsync(date);
        }

        public UniTask<NutritionReport> GetNutritionReportAsync(ReportPeriod period, DateTime endDate)
        {
            return _reportService.BuildNutritionReportAsync(period, endDate);
        }
    }
}
