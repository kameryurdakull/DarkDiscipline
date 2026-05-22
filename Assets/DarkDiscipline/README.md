# Dark Discipline Runtime

This module provides the first production-oriented foundation for tracking workouts and nutrition.

## Setup

1. Open `Tools/Dark Discipline/Setup Wizard`.
2. Click `Create Runtime Settings`.
3. Add `DarkDisciplineLifetimeScope` to a scene object.
4. Assign the created `DarkDisciplineSettings` asset to the lifetime scope.

## Runtime API

Resolve `DarkDisciplineRuntimeApi` from VContainer and use:

```csharp
await runtimeApi.LogWorkoutAsync(workoutDraft);
await runtimeApi.GetNextWorkoutRecommendationAsync("Bench Press");
await runtimeApi.GetWorkoutFrequencySummaryAsync("Bench Press", 30);
await runtimeApi.LogNutritionAsync(nutritionDraft);
await runtimeApi.GetNutritionSummaryAsync(DateTime.Today);
await runtimeApi.SaveWellnessProfileAsync(profileDraft);
await runtimeApi.LogHydrationAsync(hydrationDraft);
await runtimeApi.GetHydrationSummaryAsync(DateTime.Today);
```

## Architecture

- Domain models stay independent from Unity scene objects.
- Application services expose UniTask-based async APIs.
- Persistence is behind repository interfaces.
- Events are published through the local event bus.
- UI animation goes through a DOTween adapter.

## Dashboard Scene

`Assets/Scenes/DarkDisciplineDashboard.unity` includes a runtime dashboard with manual entry panels for:

- Basal metabolic rate, calorie deficit/surplus, and daily water target.
- Exercise name, set count, repetitions, and weight.
- Daily calories, protein, carbohydrates, and fat.
- Water intake and remaining daily hydration need.
