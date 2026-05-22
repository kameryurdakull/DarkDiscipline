using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cysharp.Threading.Tasks;
using DarkDiscipline.Application.Events;
using DarkDiscipline.Application.Fitness;
using DarkDiscipline.Application.Nutrition;
using DarkDiscipline.Application.Persistence;
using DarkDiscipline.Domain.Fitness;
using DarkDiscipline.Domain.Nutrition;
using DarkDiscipline.Infrastructure.Persistence;
using DarkDiscipline.Runtime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkDiscipline.Presentation.Dashboard
{
    public sealed class DarkDisciplineDashboardController : MonoBehaviour
    {
        private const string CanvasName = "Dashboard Canvas";
        private const string EventSystemName = "EventSystem";
        private const string AppTitle = "Dark Discipline";
        private const string AppSubtitle = "Train. Eat. Hydrate.";
        private const string DefaultExerciseName = "Bench Press";
        private const string DefaultNutritionLabel = "Meal";
        private const string DefaultSetCount = "3";
        private const string DefaultRepetitions = "8";
        private const string DefaultWeightKg = "60";
        private const string DefaultBasalMetabolicRate = "2000";
        private const string DefaultCalorieAdjustment = "-300";
        private const string DefaultWaterTarget = "2.5";
        private const string DefaultCalories = "640";
        private const string DefaultProteinGrams = "48";
        private const string DefaultCarbohydrateGrams = "72";
        private const string DefaultFatGrams = "18";
        private const string DefaultHydrationLiters = "0.5";
        private const string EmptyExerciseList = "No exercises yet.";
        private const string EmptyMealList = "No meals today.";
        private const string EmptyWaterList = "No water logs today.";
        private const string ParseErrorText = "Check values.";
        private const string SavedText = "Saved.";
        private const string AddedText = "Added.";
        private const string RemovedText = "Removed.";
        private const string ResetText = "Today reset.";
        private const string LoggedText = "Workout logged.";
        private const string SummaryFormat = "Cal {0:0}/{1:0}  Protein {2:0}g\nCarb {3:0}g  Fat {4:0}g  Water {5:0.0}/{6:0.0}L";
        private const string ReportSummaryFormat = "{0}: Cal {1:0} | P {2:0}g | C {3:0}g | F {4:0}g | W {5:0.0}L";
        private const float DefaultRateOfPerceivedExertion = 7.5f;
        private const float ButtonPunchScale = 0.08f;
        private const float ButtonPunchDuration = 0.16f;

        [SerializeField] private DarkDisciplineSettings settings;

        private readonly Dictionary<DashboardTab, GameObject> _pagesByTab = new();
        private readonly Dictionary<DashboardTab, Image> _tabImagesByTab = new();
        private readonly List<ExercisePlan> _exercisePlans = new();
        private readonly List<NutritionEntry> _mealEntries = new();
        private readonly List<HydrationEntry> _hydrationEntries = new();

        private DarkDisciplineRuntimeApi _runtimeApi;
        private ReportPeriod _activeReportPeriod = ReportPeriod.Daily;
        private TMP_InputField _basalMetabolicRateInput;
        private TMP_InputField _calorieAdjustmentInput;
        private TMP_InputField _waterTargetInput;
        private TMP_InputField _exerciseNameInput;
        private TMP_InputField _setCountInput;
        private TMP_InputField _repetitionsInput;
        private TMP_InputField _weightInput;
        private TMP_InputField _nutritionLabelInput;
        private TMP_InputField _caloriesInput;
        private TMP_InputField _proteinInput;
        private TMP_InputField _carbohydrateInput;
        private TMP_InputField _fatInput;
        private TMP_InputField _hydrationInput;
        private TextMeshProUGUI _profileStatusText;
        private TextMeshProUGUI _exerciseStatusText;
        private TextMeshProUGUI _mealStatusText;
        private TextMeshProUGUI _waterStatusText;
        private TextMeshProUGUI _summaryText;
        private TextMeshProUGUI _reportStatusText;
        private RectTransform _exerciseTableContent;
        private RectTransform _mealListContent;
        private RectTransform _waterListContent;
        private LineChartGraphic _calorieChart;
        private LineChartGraphic _proteinChart;
        private LineChartGraphic _carbohydrateChart;
        private LineChartGraphic _fatChart;
        private LineChartGraphic _waterChart;

        private void Awake()
        {
            EnsureEventSystem();
            _runtimeApi = CreateRuntimeApi();
            BuildDashboard();
        }

        private void Start()
        {
            InitializeDashboardAsync().Forget();
        }

        private DarkDisciplineRuntimeApi CreateRuntimeApi()
        {
            var activeSettings = settings != null ? settings : ScriptableObject.CreateInstance<DarkDisciplineSettings>();
            var eventBus = new EventBus();
            IExercisePlanRepository exercisePlanRepository = new JsonExercisePlanRepository();
            IWorkoutRepository workoutRepository = new JsonWorkoutRepository();
            INutritionRepository nutritionRepository = new JsonNutritionRepository();
            IWellnessRepository wellnessRepository = new JsonWellnessRepository();
            var exercisePlanService = new ExercisePlanService(exercisePlanRepository);
            var progressionCalculator = new WorkoutProgressionCalculator(activeSettings);
            var workoutService = new WorkoutService(workoutRepository, progressionCalculator, eventBus);
            var nutritionService = new NutritionService(nutritionRepository, eventBus);
            var wellnessService = new WellnessService(wellnessRepository);
            var reportService = new ReportService(nutritionRepository, wellnessRepository);
            return new DarkDisciplineRuntimeApi(exercisePlanService, workoutService, nutritionService, wellnessService, reportService);
        }

        private void BuildDashboard()
        {
            var canvas = CreateCanvas();
            ClearChildren(canvas.transform);
            var root = CreateRect("Mobile Root", canvas.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            CreateHeader(root);
            var pageHost = CreateRect("Page Host", root, new Vector2(0f, 0.1f), new Vector2(1f, 0.86f), new Vector2(24f, 0f), new Vector2(-24f, 0f));
            CreatePages(pageHost);
            CreateTabBar(root);
            ShowTab(DashboardTab.Exercise);
        }

        private void CreateHeader(Transform parent)
        {
            var header = CreateRect("Header", parent, new Vector2(0f, 0.86f), Vector2.one, new Vector2(32f, 0f), new Vector2(-32f, -10f));
            var layout = AddVerticalLayout(header.gameObject, 8f, new RectOffset(0, 0, 20, 0));
            layout.childAlignment = TextAnchor.UpperLeft;
            CreateText(header, AppTitle, 42, FontStyles.Bold, new Color(0.92f, 0.95f, 0.98f), 56f);
            CreateText(header, AppSubtitle, 22, FontStyles.Normal, new Color(0.62f, 0.72f, 0.78f), 34f);
        }

        private void CreatePages(Transform parent)
        {
            _pagesByTab.Add(DashboardTab.Exercise, CreateExercisePage(parent));
            _pagesByTab.Add(DashboardTab.Meals, CreateMealsPage(parent));
            _pagesByTab.Add(DashboardTab.Water, CreateWaterPage(parent));
            _pagesByTab.Add(DashboardTab.Reports, CreateReportsPage(parent));
            _pagesByTab.Add(DashboardTab.Profile, CreateProfilePage(parent));
        }

        private GameObject CreateProfilePage(Transform parent)
        {
            var content = CreateScrollPage(parent, "Profile Page");
            CreateSectionTitle(content, "Profile");
            _basalMetabolicRateInput = CreateLabeledInput(content, "BMR", DefaultBasalMetabolicRate);
            _calorieAdjustmentInput = CreateLabeledInput(content, "Cal +/-", DefaultCalorieAdjustment);
            _waterTargetInput = CreateLabeledInput(content, "Water", DefaultWaterTarget);
            CreateButton(content, "Save", () => SaveProfileAsync().Forget());
            _profileStatusText = CreateText(content, string.Empty, 22, FontStyles.Normal, Color.white, 42f);
            _summaryText = CreateText(content, string.Empty, 24, FontStyles.Normal, new Color(0.82f, 0.9f, 0.94f), 96f);
            return content.transform.parent.gameObject;
        }

        private GameObject CreateExercisePage(Transform parent)
        {
            var content = CreateScrollPage(parent, "Exercise Page");
            CreateSectionTitle(content, "Exercise");
            _exerciseNameInput = CreateLabeledInput(content, "Name", DefaultExerciseName);
            CreateInlineInputs(content, ("Sets", DefaultSetCount, field => _setCountInput = field), ("Reps", DefaultRepetitions, field => _repetitionsInput = field), ("Kg", DefaultWeightKg, field => _weightInput = field));
            CreateButton(content, "Add / Update", () => SaveExerciseAsync().Forget());
            _exerciseStatusText = CreateText(content, string.Empty, 22, FontStyles.Normal, Color.white, 40f);
            _exerciseTableContent = CreateListContainer(content, "Exercise Table");
            return content.transform.parent.gameObject;
        }

        private GameObject CreateMealsPage(Transform parent)
        {
            var content = CreateScrollPage(parent, "Meals Page");
            CreateSectionTitle(content, "Meals");
            _nutritionLabelInput = CreateLabeledInput(content, "Meal", DefaultNutritionLabel);
            _caloriesInput = CreateLabeledInput(content, "Cal", DefaultCalories);
            CreateInlineInputs(content, ("Protein", DefaultProteinGrams, field => _proteinInput = field), ("Carb", DefaultCarbohydrateGrams, field => _carbohydrateInput = field), ("Fat", DefaultFatGrams, field => _fatInput = field));
            CreateButton(content, "Add Meal", () => LogNutritionAsync().Forget());
            CreateButton(content, "Daily Reset", () => ResetDailyAsync().Forget());
            _mealStatusText = CreateText(content, string.Empty, 22, FontStyles.Normal, Color.white, 40f);
            _mealListContent = CreateListContainer(content, "Meal List");
            return content.transform.parent.gameObject;
        }

        private GameObject CreateWaterPage(Transform parent)
        {
            var content = CreateScrollPage(parent, "Water Page");
            CreateSectionTitle(content, "Water");
            _hydrationInput = CreateLabeledInput(content, "Liters", DefaultHydrationLiters);
            CreateButton(content, "Add Water", () => LogHydrationAsync().Forget());
            CreateButton(content, "Daily Reset", () => ResetDailyAsync().Forget());
            _waterStatusText = CreateText(content, string.Empty, 22, FontStyles.Normal, Color.white, 40f);
            _waterListContent = CreateListContainer(content, "Water List");
            return content.transform.parent.gameObject;
        }

        private GameObject CreateReportsPage(Transform parent)
        {
            var content = CreateScrollPage(parent, "Reports Page");
            CreateSectionTitle(content, "Reports");
            var segmented = CreateHorizontalGroup(content, "Report Periods", 8f, 72f);
            CreateButton(segmented, "Daily", () => SetReportPeriod(ReportPeriod.Daily));
            CreateButton(segmented, "Weekly", () => SetReportPeriod(ReportPeriod.Weekly));
            CreateButton(segmented, "Monthly", () => SetReportPeriod(ReportPeriod.Monthly));
            CreateButton(content, "Daily Reset", () => ResetDailyAsync().Forget());
            _reportStatusText = CreateText(content, string.Empty, 21, FontStyles.Normal, Color.white, 60f);
            _calorieChart = CreateChart(content, "Calories", new Color(0.35f, 0.72f, 0.95f));
            _proteinChart = CreateChart(content, "Protein", new Color(0.52f, 0.84f, 0.52f));
            _carbohydrateChart = CreateChart(content, "Carbs", new Color(0.95f, 0.72f, 0.34f));
            _fatChart = CreateChart(content, "Fat", new Color(0.9f, 0.54f, 0.64f));
            _waterChart = CreateChart(content, "Water", new Color(0.42f, 0.62f, 1f));
            return content.transform.parent.gameObject;
        }

        private void CreateTabBar(Transform parent)
        {
            var tabBar = CreateRect("Tab Bar", parent, Vector2.zero, new Vector2(1f, 0.1f), new Vector2(18f, 14f), new Vector2(-18f, -14f));
            var layout = AddHorizontalLayout(tabBar.gameObject, 8f, new RectOffset(0, 0, 0, 0));
            layout.childForceExpandWidth = true;
            CreateTab(tabBar, DashboardTab.Exercise, "Exercise");
            CreateTab(tabBar, DashboardTab.Meals, "Meals");
            CreateTab(tabBar, DashboardTab.Water, "Water");
            CreateTab(tabBar, DashboardTab.Reports, "Reports");
            CreateTab(tabBar, DashboardTab.Profile, "Profile");
        }

        private void CreateTab(Transform parent, DashboardTab tab, string label)
        {
            var buttonObject = CreateButton(parent, label, () => ShowTab(tab));
            _tabImagesByTab.Add(tab, buttonObject.GetComponent<Image>());
        }

        private async UniTaskVoid InitializeDashboardAsync()
        {
            var profile = await _runtimeApi.GetWellnessProfileAsync();
            _basalMetabolicRateInput.text = profile.BasalMetabolicRateCalories.ToString("0", CultureInfo.InvariantCulture);
            _calorieAdjustmentInput.text = profile.CalorieAdjustment.ToString(CultureInfo.InvariantCulture);
            _waterTargetInput.text = profile.DailyWaterTargetLiters.ToString("0.0", CultureInfo.InvariantCulture);
            await RefreshDashboardAsync();
        }

        private void ShowTab(DashboardTab tab)
        {
            foreach (var pair in _pagesByTab)
            {
                pair.Value.SetActive(pair.Key == tab);
            }

            foreach (var pair in _tabImagesByTab)
            {
                pair.Value.color = pair.Key == tab ? new Color(0.24f, 0.56f, 0.66f) : new Color(0.13f, 0.17f, 0.2f);
            }

            RefreshDashboardAsync().Forget();
        }

        private void SetReportPeriod(ReportPeriod period)
        {
            _activeReportPeriod = period;
            RefreshDashboardAsync().Forget();
        }

        private async UniTaskVoid SaveProfileAsync()
        {
            if (TryReadFloat(_basalMetabolicRateInput, out var basalMetabolicRate) == false ||
                TryReadInt(_calorieAdjustmentInput, out var calorieAdjustment) == false ||
                TryReadFloat(_waterTargetInput, out var waterTarget) == false)
            {
                _profileStatusText.text = ParseErrorText;
                return;
            }

            await _runtimeApi.SaveWellnessProfileAsync(new WellnessProfileDraft(basalMetabolicRate, calorieAdjustment, waterTarget));
            _profileStatusText.text = SavedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid SaveExerciseAsync()
        {
            if (TryReadExerciseDraft(out var draft) == false)
            {
                _exerciseStatusText.text = ParseErrorText;
                return;
            }

            await _runtimeApi.SaveExercisePlanAsync(draft);
            _exerciseStatusText.text = SavedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid LogWorkoutAsync(ExercisePlan plan)
        {
            var sets = new List<ExerciseSetRecord>(plan.SetCount);

            for (var index = 0; index < plan.SetCount; index++)
            {
                sets.Add(new ExerciseSetRecord(plan.Repetitions, plan.WeightKg, DefaultRateOfPerceivedExertion));
            }

            var exercise = new WorkoutExerciseRecord(plan.Name, plan.Repetitions, sets);
            await _runtimeApi.LogWorkoutAsync(new WorkoutSessionDraft(new[] { exercise }));
            _exerciseStatusText.text = LoggedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid RemoveExerciseAsync(string planId)
        {
            await _runtimeApi.RemoveExercisePlanAsync(planId);
            _exerciseStatusText.text = RemovedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid LogNutritionAsync()
        {
            if (TryReadInt(_caloriesInput, out var calories) == false ||
                TryReadFloat(_proteinInput, out var protein) == false ||
                TryReadFloat(_carbohydrateInput, out var carbohydrate) == false ||
                TryReadFloat(_fatInput, out var fat) == false)
            {
                _mealStatusText.text = ParseErrorText;
                return;
            }

            await _runtimeApi.LogNutritionAsync(new NutritionEntryDraft(_nutritionLabelInput.text, calories, protein, carbohydrate, fat));
            _mealStatusText.text = AddedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid RemoveNutritionAsync(string entryId)
        {
            await _runtimeApi.RemoveNutritionEntryAsync(entryId);
            _mealStatusText.text = RemovedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid LogHydrationAsync()
        {
            if (TryReadFloat(_hydrationInput, out var liters) == false)
            {
                _waterStatusText.text = ParseErrorText;
                return;
            }

            await _runtimeApi.LogHydrationAsync(new HydrationEntryDraft(liters));
            _waterStatusText.text = AddedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid RemoveHydrationAsync(string entryId)
        {
            await _runtimeApi.RemoveHydrationEntryAsync(entryId);
            _waterStatusText.text = RemovedText;
            await RefreshDashboardAsync();
        }

        private async UniTaskVoid ResetDailyAsync()
        {
            await _runtimeApi.ResetNutritionForDateAsync(DateTime.Today);
            await _runtimeApi.ResetHydrationForDateAsync(DateTime.Today);
            _mealStatusText.text = ResetText;
            _waterStatusText.text = ResetText;
            _reportStatusText.text = ResetText;
            await RefreshDashboardAsync();
        }

        private async UniTask RefreshDashboardAsync()
        {
            _exercisePlans.Clear();
            _mealEntries.Clear();
            _hydrationEntries.Clear();
            _exercisePlans.AddRange(await _runtimeApi.GetExercisePlansAsync());
            _mealEntries.AddRange(await _runtimeApi.GetNutritionEntriesByDateAsync(DateTime.Today));
            _hydrationEntries.AddRange(await _runtimeApi.GetHydrationEntriesByDateAsync(DateTime.Today));

            await RefreshSummaryAsync();
            RefreshExerciseTable();
            RefreshMealList();
            RefreshWaterList();
            await RefreshReportsAsync();
        }

        private async UniTask RefreshSummaryAsync()
        {
            var profile = await _runtimeApi.GetWellnessProfileAsync();
            var nutrition = await _runtimeApi.GetNutritionSummaryAsync(DateTime.Today);
            var hydration = await _runtimeApi.GetHydrationSummaryAsync(DateTime.Today);
            _summaryText.text = string.Format(SummaryFormat, nutrition.Calories, profile.DailyCalorieTarget, nutrition.ProteinGrams, nutrition.CarbohydrateGrams, nutrition.FatGrams, hydration.ConsumedLiters, hydration.TargetLiters);
        }

        private void RefreshExerciseTable()
        {
            ClearChildren(_exerciseTableContent);
            CreateTableHeader(_exerciseTableContent, "Name", "Sets", "Reps", "Kg", string.Empty, string.Empty);

            if (_exercisePlans.Count == 0)
            {
                CreateText(_exerciseTableContent, EmptyExerciseList, 22, FontStyles.Normal, new Color(0.7f, 0.78f, 0.82f), 58f);
                return;
            }

            for (var index = 0; index < _exercisePlans.Count; index++)
            {
                var plan = _exercisePlans[index];
                var row = CreateTableRow(_exerciseTableContent, 78f);
                CreateCell(row, plan.Name, 2f);
                CreateCell(row, plan.SetCount.ToString(CultureInfo.InvariantCulture), 1f);
                CreateCell(row, plan.Repetitions.ToString(CultureInfo.InvariantCulture), 1f);
                CreateCell(row, plan.WeightKg.ToString("0.0", CultureInfo.InvariantCulture), 1f);
                CreateButton(row, "Log", () => LogWorkoutAsync(plan).Forget());
                CreateButton(row, "Remove", () => RemoveExerciseAsync(plan.Id).Forget());
            }
        }

        private void RefreshMealList()
        {
            ClearChildren(_mealListContent);

            if (_mealEntries.Count == 0)
            {
                CreateText(_mealListContent, EmptyMealList, 22, FontStyles.Normal, new Color(0.7f, 0.78f, 0.82f), 58f);
                return;
            }

            for (var index = 0; index < _mealEntries.Count; index++)
            {
                var entry = _mealEntries[index];
                var row = CreateTableRow(_mealListContent, 82f);
                CreateCell(row, $"{entry.Label}\n{entry.Calories} cal | P {entry.ProteinGrams:0} C {entry.CarbohydrateGrams:0} F {entry.FatGrams:0}", 3f);
                CreateButton(row, "Remove", () => RemoveNutritionAsync(entry.Id).Forget());
            }
        }

        private void RefreshWaterList()
        {
            ClearChildren(_waterListContent);

            if (_hydrationEntries.Count == 0)
            {
                CreateText(_waterListContent, EmptyWaterList, 22, FontStyles.Normal, new Color(0.7f, 0.78f, 0.82f), 58f);
                return;
            }

            for (var index = 0; index < _hydrationEntries.Count; index++)
            {
                var entry = _hydrationEntries[index];
                var row = CreateTableRow(_waterListContent, 74f);
                CreateCell(row, $"{entry.Liters:0.0} L", 3f);
                CreateButton(row, "Remove", () => RemoveHydrationAsync(entry.Id).Forget());
            }
        }

        private async UniTask RefreshReportsAsync()
        {
            var report = await _runtimeApi.GetNutritionReportAsync(_activeReportPeriod, DateTime.Today);
            _calorieChart.SetValues(report.Points.Select(point => point.Calories).ToArray());
            _proteinChart.SetValues(report.Points.Select(point => point.ProteinGrams).ToArray());
            _carbohydrateChart.SetValues(report.Points.Select(point => point.CarbohydrateGrams).ToArray());
            _fatChart.SetValues(report.Points.Select(point => point.FatGrams).ToArray());
            _waterChart.SetValues(report.Points.Select(point => point.WaterLiters).ToArray());

            var latest = report.Points.Count == 0 ? null : report.Points[report.Points.Count - 1];
            _reportStatusText.text = latest == null
                ? string.Empty
                : string.Format(ReportSummaryFormat, _activeReportPeriod, latest.Calories, latest.ProteinGrams, latest.CarbohydrateGrams, latest.FatGrams, latest.WaterLiters);
        }

        private bool TryReadExerciseDraft(out ExercisePlanDraft draft)
        {
            draft = null;

            if (string.IsNullOrWhiteSpace(_exerciseNameInput.text) ||
                TryReadInt(_setCountInput, out var setCount) == false ||
                TryReadInt(_repetitionsInput, out var repetitions) == false ||
                TryReadFloat(_weightInput, out var weightKg) == false)
            {
                return false;
            }

            draft = new ExercisePlanDraft(_exerciseNameInput.text, setCount, repetitions, weightKg);
            return true;
        }

        private static bool TryReadInt(TMP_InputField inputField, out int value)
        {
            return int.TryParse(inputField.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryReadFloat(TMP_InputField inputField, out float value)
        {
            var normalizedText = inputField.text.Replace(',', '.');
            return float.TryParse(normalizedText, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private static Canvas CreateCanvas()
        {
            var existingCanvas = FindFirstObjectByType<Canvas>();

            if (existingCanvas != null)
            {
                EnsureCanvasSupport(existingCanvas.gameObject);
                return existingCanvas;
            }

            var canvasObject = new GameObject(CanvasName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            EnsureCanvasSupport(canvasObject);
            return canvas;
        }

        private static Transform CreateScrollPage(Transform parent, string name)
        {
            var page = CreateRect(name, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            var scroll = new GameObject("Scroll", typeof(RectTransform), typeof(ScrollRect));
            scroll.transform.SetParent(page, false);
            Stretch(scroll.GetComponent<RectTransform>());
            var viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(scroll.transform, false);
            Stretch(viewport.GetComponent<RectTransform>());
            viewport.GetComponent<Image>().color = Color.white;
            viewport.GetComponent<Mask>().showMaskGraphic = false;
            var content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);
            var contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0f, 1f);
            contentRect.anchorMax = new Vector2(1f, 1f);
            contentRect.pivot = new Vector2(0.5f, 1f);
            contentRect.offsetMin = Vector2.zero;
            contentRect.offsetMax = Vector2.zero;
            var layout = AddVerticalLayout(content, 18f, new RectOffset(0, 0, 0, 24));
            layout.childForceExpandHeight = false;
            content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            var scrollRect = scroll.GetComponent<ScrollRect>();
            scrollRect.content = contentRect;
            scrollRect.viewport = viewport.GetComponent<RectTransform>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            return content.transform;
        }

        private static void CreateSectionTitle(Transform parent, string title)
        {
            CreateText(parent, title, 34, FontStyles.Bold, new Color(0.92f, 0.95f, 0.98f), 54f);
        }

        private static TMP_InputField CreateLabeledInput(Transform parent, string label, string value)
        {
            var group = CreatePanel(parent, label, 112f);
            CreateText(group, label, 20, FontStyles.Bold, new Color(0.68f, 0.78f, 0.84f), 30f);
            return CreateInput(group, value);
        }

        private static void CreateInlineInputs(Transform parent, params (string label, string value, Action<TMP_InputField> assign)[] inputs)
        {
            var row = CreateHorizontalGroup(parent, "Input Row", 10f, 112f);

            for (var index = 0; index < inputs.Length; index++)
            {
                var input = CreateLabeledInput(row, inputs[index].label, inputs[index].value);
                inputs[index].assign.Invoke(input);
            }
        }

        private static RectTransform CreateListContainer(Transform parent, string name)
        {
            var panel = CreatePanel(parent, name, 0f);
            var layoutElement = panel.GetComponent<LayoutElement>();
            layoutElement.minHeight = 220f;
            layoutElement.preferredHeight = -1f;
            layoutElement.flexibleHeight = 0f;
            return panel.GetComponent<RectTransform>();
        }

        private static void CreateTableHeader(Transform parent, params string[] labels)
        {
            var row = CreateTableRow(parent, 52f);

            for (var index = 0; index < labels.Length; index++)
            {
                CreateCell(row, labels[index], index == 0 ? 2f : 1f, FontStyles.Bold);
            }
        }

        private static Transform CreateTableRow(Transform parent, float height)
        {
            return CreateHorizontalGroup(parent, "Row", 8f, height);
        }

        private static void CreateCell(Transform parent, string text, float flexibleWidth, FontStyles style = FontStyles.Normal)
        {
            var label = CreateText(parent, text, 19, style, new Color(0.86f, 0.92f, 0.95f), 54f);
            var layoutElement = label.GetComponent<LayoutElement>();
            layoutElement.flexibleWidth = flexibleWidth;
            layoutElement.preferredWidth = 0f;
        }

        private static LineChartGraphic CreateChart(Transform parent, string label, Color color)
        {
            var panel = CreatePanel(parent, label, 212f);
            CreateText(panel, label, 22, FontStyles.Bold, new Color(0.86f, 0.92f, 0.95f), 34f);
            var chartObject = new GameObject(label + " Chart", typeof(RectTransform), typeof(CanvasRenderer), typeof(LineChartGraphic));
            chartObject.transform.SetParent(panel, false);
            var layoutElement = chartObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = 140f;
            layoutElement.preferredHeight = 140f;
            var chart = chartObject.GetComponent<LineChartGraphic>();
            chart.color = color;
            return chart;
        }

        private static Transform CreatePanel(Transform parent, string name, float height)
        {
            var panel = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup), typeof(LayoutElement));
            panel.transform.SetParent(parent, false);
            panel.GetComponent<Image>().color = new Color(0.11f, 0.14f, 0.17f, 0.98f);
            var layout = AddVerticalLayout(panel, 8f, new RectOffset(18, 18, 14, 14));
            layout.childForceExpandHeight = false;
            var layoutElement = panel.GetComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;
            return panel.transform;
        }

        private static TMP_InputField CreateInput(Transform parent, string value)
        {
            var inputObject = new GameObject("Input", typeof(RectTransform), typeof(Image), typeof(TMP_InputField), typeof(LayoutElement));
            inputObject.transform.SetParent(parent, false);
            inputObject.GetComponent<Image>().color = new Color(0.18f, 0.23f, 0.27f);
            var layoutElement = inputObject.GetComponent<LayoutElement>();
            layoutElement.minHeight = 54f;
            layoutElement.preferredHeight = 54f;
            var text = CreateInputText(inputObject.transform, value, Color.white);
            var placeholder = CreateInputText(inputObject.transform, string.Empty, new Color(0.5f, 0.58f, 0.64f));
            var input = inputObject.GetComponent<TMP_InputField>();
            input.textComponent = text;
            input.placeholder = placeholder;
            input.textViewport = inputObject.GetComponent<RectTransform>();
            input.text = value;
            return input;
        }

        private static TextMeshProUGUI CreateInputText(Transform parent, string text, Color color)
        {
            var textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(parent, false);
            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(16f, 8f);
            rectTransform.offsetMax = new Vector2(-16f, -8f);
            var label = textObject.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = 22;
            label.color = color;
            label.alignment = TextAlignmentOptions.MidlineLeft;
            return label;
        }

        private static TextMeshProUGUI CreateText(Transform parent, string text, int size, FontStyles style, Color color, float height)
        {
            var textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
            textObject.transform.SetParent(parent, false);
            var label = textObject.GetComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = size;
            label.fontStyle = style;
            label.color = color;
            label.alignment = TextAlignmentOptions.MidlineLeft;
            label.enableWordWrapping = true;
            var layoutElement = textObject.GetComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;
            return label;
        }

        private static GameObject CreateButton(Transform parent, string label, Action onClick)
        {
            var buttonObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            buttonObject.transform.SetParent(parent, false);
            buttonObject.GetComponent<Image>().color = new Color(0.2f, 0.45f, 0.55f);
            var layoutElement = buttonObject.GetComponent<LayoutElement>();
            layoutElement.minHeight = 58f;
            layoutElement.preferredHeight = 58f;
            layoutElement.flexibleWidth = 1f;
            var button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                buttonObject.transform.DOKill();
                buttonObject.transform.localScale = Vector3.one;
                buttonObject.transform.DOPunchScale(Vector3.one * ButtonPunchScale, ButtonPunchDuration, 6, 0.5f);
                onClick.Invoke();
            });
            var text = CreateText(buttonObject.transform, label, 20, FontStyles.Bold, Color.white, 58f);
            text.alignment = TextAlignmentOptions.Center;
            Stretch(text.rectTransform);
            return buttonObject;
        }

        private static Transform CreateHorizontalGroup(Transform parent, string name, float spacing, float height)
        {
            var group = new GameObject(name, typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
            group.transform.SetParent(parent, false);
            var layout = AddHorizontalLayout(group, spacing, new RectOffset(0, 0, 0, 0));
            layout.childForceExpandWidth = true;
            var layoutElement = group.GetComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;
            return group.transform;
        }

        private static VerticalLayoutGroup AddVerticalLayout(GameObject target, float spacing, RectOffset padding)
        {
            var layout = target.GetComponent<VerticalLayoutGroup>();
            layout = layout != null ? layout : target.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            return layout;
        }

        private static HorizontalLayoutGroup AddHorizontalLayout(GameObject target, float spacing, RectOffset padding)
        {
            var layout = target.GetComponent<HorizontalLayoutGroup>();
            layout = layout != null ? layout : target.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.padding = padding;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;
            return layout;
        }

        private static Transform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            var rectObject = new GameObject(name, typeof(RectTransform));
            rectObject.transform.SetParent(parent, false);
            var rectTransform = rectObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            return rectObject.transform;
        }

        private static void Stretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private static void EnsureCanvasSupport(GameObject canvasObject)
        {
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler = scaler != null ? scaler : canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.matchWidthOrHeight = 1f;

            if (canvasObject.GetComponent<GraphicRaycaster>() == null)
            {
                canvasObject.AddComponent<GraphicRaycaster>();
            }
        }

        private static void EnsureEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            new GameObject(EventSystemName, typeof(EventSystem), typeof(StandaloneInputModule));
        }

        private static void ClearChildren(Transform parent)
        {
            for (var index = parent.childCount - 1; index >= 0; index--)
            {
                var child = parent.GetChild(index).gameObject;
                child.SetActive(false);

                if (UnityEngine.Application.isPlaying)
                {
                    Destroy(child);
                    continue;
                }

                DestroyImmediate(child);
            }
        }
    }
}
