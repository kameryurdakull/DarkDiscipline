using DarkDiscipline.Application.Events;
using DarkDiscipline.Application.Fitness;
using DarkDiscipline.Application.Nutrition;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Infrastructure.Persistence;
using DarkDiscipline.Presentation.Animation;
using DarkDiscipline.Runtime;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DarkDiscipline.Infrastructure.DependencyInjection
{
    public sealed class DarkDisciplineLifetimeScope : LifetimeScope
    {
        private const string SettingsResourcePath = "DarkDisciplineSettings";
        [SerializeField] private DarkDisciplineSettings settings;

        protected override void Configure(IContainerBuilder builder)
        {
            var activeSettings = settings != null ? settings : Resources.Load<DarkDisciplineSettings>(SettingsResourcePath);
            activeSettings = activeSettings != null ? activeSettings : ScriptableObject.CreateInstance<DarkDisciplineSettings>();
            builder.RegisterInstance(activeSettings);
            builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
            builder.Register<IExercisePlanRepository, JsonExercisePlanRepository>(Lifetime.Singleton);
            builder.Register<IWorkoutRepository, JsonWorkoutRepository>(Lifetime.Singleton);
            builder.Register<INutritionRepository, JsonNutritionRepository>(Lifetime.Singleton);
            builder.Register<IWellnessRepository, JsonWellnessRepository>(Lifetime.Singleton);
            builder.Register<IExercisePlanService, ExercisePlanService>(Lifetime.Singleton);
            builder.Register<IWorkoutProgressionCalculator, WorkoutProgressionCalculator>(Lifetime.Singleton);
            builder.Register<IWorkoutService, WorkoutService>(Lifetime.Singleton);
            builder.Register<INutritionService, NutritionService>(Lifetime.Singleton);
            builder.Register<IWellnessService, WellnessService>(Lifetime.Singleton);
            builder.Register<IReportService, ReportService>(Lifetime.Singleton);
            builder.Register<IUiTweenAnimator, DotweenUiTweenAnimator>(Lifetime.Singleton);
            builder.Register<DarkDisciplineRuntimeApi>(Lifetime.Singleton);
        }
    }
}
