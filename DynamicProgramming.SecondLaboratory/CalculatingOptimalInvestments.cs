using DynamicProgramming.SecondLaboratory.Models;

namespace DynamicProgramming.SecondLaboratory;

public static class CalculatingOptimalInvestments
{
    /// <summary>
    /// Вычисляет оптимальную стратегию инвестирования.
    /// </summary>
    /// <param name="totalInvestmentAmount">Общая сумма для инвестирования.</param>
    /// <param name="investStep">Шаг инвестирования.</param>
    /// <param name="profitFunctions">Функции прибыли для каждого инвестиционного варианта.</param>
    /// <returns>Результат инвестирования (максимальная прибыль и оптимальное распределение инвестиций).</returns>
    public static InvestmentResult CalculateOptimalInvestments(int totalInvestmentAmount, int investStep, params Func<int, int>[] profitFunctions)
    {
        if (totalInvestmentAmount % investStep != 0)
        {
            throw new ArgumentException("Начальная сумма должна делиться на шаг без остатка");
        }
        if (totalInvestmentAmount <= 0 || investStep <= 0)
        {
            throw new ArgumentException("Шаг инвестирования и начальная сумма должны быть больше 0");
        }
        // Создаем словарь функций прибыли, где ключ - номер варианта инвестирования (индекс в массиве profitFunctions + 1)
        var profitFunctionMap = GetFactoryProfitFunctions(profitFunctions);
        // Количество шагов инвестирования (сколько раз можно инвестировать по шагу investStep)
        var investmentStepsCount = totalInvestmentAmount / investStep;
        // Количество вариантов инвестирования
        var factoriesCount = profitFunctionMap.Count;
        // Вычисляем максимальную прибыль и оптимальное распределение инвестиций с помощью динамического программирования
        var profitOptimization = CalculateProfitOptimization(investmentStepsCount, investStep, profitFunctionMap);
        // Максимальная достижимая прибыль
        var optimalProfit = profitOptimization.MaxProfit[factoriesCount, investmentStepsCount];
        // Оптимальное распределение инвестиций по вариантам
        var optimalInvestments = 
            CalculateFactoryOptimalInvestments(investmentStepsCount, factoriesCount, profitOptimization.InvestmentDistribution);

        return new InvestmentResult(optimalProfit, optimalInvestments);
    }
    
    /// <summary>
    /// Создает словарь функций прибыли, используя массив функций.
    /// </summary>
    /// <param name="profitFunctions">Массив функций прибыли.</param>
    /// <returns>Словарь, где ключ - номер варианта инвестирования, значение - функция прибыли.</returns>
    /// <exception cref="InvalidOperationException">Выбрасывается, если массив функций пуст.</exception>
    private static IDictionary<int, Func<int, int>> GetFactoryProfitFunctions(params Func<int, int>[] profitFunctions)
    {
        if (profitFunctions.Length == 0)
        {
            throw new InvalidOperationException("Функции прибыли не добавлены");
        }
        var factoryProfitFunctions = new Dictionary<int, Func<int, int>>();
        for (var i = 0; i < profitFunctions.Length; i++)
        {
            factoryProfitFunctions[i] = profitFunctions[i]; 
        }
        return factoryProfitFunctions;
    }
    
    
    /// <summary>
    /// Вычисляет максимальную прибыль и оптимальное распределение инвестиций с помощью динамического программирования.
    /// </summary>
    /// <param name="investmentStepsCount">Количество шагов инвестирования.</param>
    /// <param name="investStep">Шаг инвестирования.</param>
    /// <param name="profitFunctionMap">Словарь функций прибыли.</param>
    /// <returns>Результат оптимизации прибыли.</returns>
    private static ProfitOptimizationResult CalculateProfitOptimization(int investmentStepsCount, int investStep,
        IDictionary<int, Func<int, int>> profitFunctionMap)
    {
        // Количество вариантов инвестирования
        var factoriesCount = profitFunctionMap.Count;

        // Двумерный массив для хранения максимальной прибыли.
        // maxProfit[i, j] - максимальная прибыль при инвестировании в i вариантов инвестирования и j шагах.
        var maxProfit = new int[factoriesCount + 1, investmentStepsCount + 1];

        // Двумерный массив для хранения оптимального распределения инвестиций.
        // investmentDistribution[i, j] - количество шагов инвестирования в i-й вариант при j шагах инвестирования.
        var investmentDistribution = new int[factoriesCount + 1, investmentStepsCount + 1];
        // Динамическое программирование: перебираем варианты инвестирования и шаги инвестирования
        for (var factoryIndex = 0; factoryIndex < factoriesCount; factoryIndex++)
        {
            for (var investmentStepIndex = 0; investmentStepIndex <= investmentStepsCount; investmentStepIndex++)
            {
                // Перебираем все возможные распределения инвестиций в текущий вариант
                for (var investmentIncrementCount = 0; investmentIncrementCount <= investmentStepIndex; investmentIncrementCount++)
                {
                    // Сумма инвестиций в текущий вариант
                    var investmentAmount = investmentIncrementCount * investStep;
                    // Прибыль от инвестиций в текущий вариант
                    var currentProfit = profitFunctionMap[factoryIndex](investmentAmount);
                    // Максимальная прибыль от предыдущих вариантов инвестирования
                    var previousMaxProfit = maxProfit[factoryIndex, investmentStepIndex - investmentIncrementCount];

                    // Если текущее распределение инвестиций дает большую прибыль, обновляем значения
                    if (previousMaxProfit + currentProfit > maxProfit[factoryIndex + 1, investmentStepIndex])
                    {
                        maxProfit[factoryIndex + 1, investmentStepIndex] = previousMaxProfit + currentProfit;
                        investmentDistribution[factoryIndex + 1, investmentStepIndex] = investmentIncrementCount;
                    }
                }
            }
        }

        return new ProfitOptimizationResult()
        {
            MaxProfit = maxProfit,
            InvestmentDistribution = investmentDistribution
        };
    }
    
    /// <summary>
    /// Вычисляет оптимальное количество инвестиций для каждого варианта инвестирования.
    /// </summary>
    /// <param name="investmentStepsCount">Количество шагов инвестирования.</param>
    /// <param name="factoriesCount">Количество вариантов инвестирования.</param>
    /// <param name="investmentDistribution">Оптимальное распределение инвестиций.</param>
    /// <returns>Массив оптимальных инвестиций для каждого варианта.</returns>
    private static int[] CalculateFactoryOptimalInvestments(int investmentStepsCount, int factoriesCount,
        int[,] investmentDistribution)
    {
        // Массив для хранения оптимальных инвестиций для каждого варианта
        var optimalInvestments = new int[factoriesCount]; 
        // Начинаем с последнего варианта инвестирования и идем назад
        var remainingInvestmentStepIndex = investmentStepsCount;
        for (var factoryIndex = factoriesCount - 1; factoryIndex >= 0; factoryIndex--)
        {
            // Оптимальное количество шагов инвестирования в текущий вариант
            var investmentCount = investmentDistribution[factoryIndex + 1, remainingInvestmentStepIndex]; 
            optimalInvestments[factoryIndex] = investmentCount;
            // Уменьшаем количество оставшихся шагов инвестирования
            remainingInvestmentStepIndex -= investmentCount;
        }

        return optimalInvestments;
    }
}
