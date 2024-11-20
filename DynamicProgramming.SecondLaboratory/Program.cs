// See https://aka.ms/new-console-template for more information

using DynamicProgramming.SecondLaboratory;
using DynamicProgramming.SecondLaboratory.Factories;



var firstFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 8},
    {2, 10},
    {3, 11},
    {4, 12},
    {5, 18},
    {6, 20},
    {7, 21}
};

var secondFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 6},
    {2, 9},
    {3, 11},
    {4, 13},
    {5, 15},
    {6, 17},
    {7, 18}
};

var thirdFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 3},
    {2, 4},
    {3, 7},
    {4, 11},
    {5, 18},
    {6, 20},
    {7, 21}
};

var fourthFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 4},
    {2, 6},
    {3, 8},
    {4, 13},
    {5, 16},
    {6, 18},
    {7, 19}
};
var fifthFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 7},
    {2, 8},
    {3, 11},
    {4, 11},
    {5, 11},
    {6, 13},
    {7, 14}
};

var sixthFactoryValues = new Dictionary<int, int>()
{
    {0, 0},
    {1, 5},
    {2, 9},
    {3, 12},
    {4, 13},
    {5, 13},
    {6, 15},
    {7, 16}
};

var step = 40;
var sum = 280;

var firstFactory = new FunctionFactory(step, firstFactoryValues);
var secondFactory = new FunctionFactory(step, secondFactoryValues);
var thirdFactory = new FunctionFactory(step, thirdFactoryValues);
var fourthFactory = new FunctionFactory(step, fourthFactoryValues);
var fifthFactory = new FunctionFactory(step, fifthFactoryValues);
var sixthFactory = new FunctionFactory(step, sixthFactoryValues);
var functions = new Func<int, int>[6]
{
    sixthFactory.Calculate, fifthFactory.Calculate, fourthFactory.Calculate,
    thirdFactory.Calculate,secondFactory.Calculate, firstFactory.Calculate
    // firstFactory.Calculate, secondFactory.Calculate, thirdFactory.Calculate,
    // fourthFactory.Calculate, fifthFactory.Calculate, sixthFactory.Calculate 
};
var result = CalculatingOptimalInvestments.CalculateOptimalInvestments(sum, step, functions);
Console.WriteLine("Результат:");
Console.WriteLine($"Оптимальная прибыль: {result.OptimalProfit}");

for (var i = result.OptimalInvestments.Length - 1; i >= 0 ; i--)
{
    var investmentAmount = result.OptimalInvestments[i] * step;
    if (investmentAmount > 0)
    {
        Console.WriteLine($"Фабрика {i + 1}: Вложить {investmentAmount}, прибыль: {functions[i](investmentAmount)}");
    }
    else
    {
        Console.WriteLine($"Фабрика {i + 1}: Не инвестировать");
    }
}


// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(80, step, functions).OptimalProfit);
// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(120, step, functions).OptimalProfit); 
// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(160, step, functions).OptimalProfit); 
// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(200, step, functions).OptimalProfit); 
// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(240, step, functions).OptimalProfit); 
// Console.WriteLine(CalculatingOptimalInvestments.CalculateOptimalInvestments(280, step, functions).OptimalProfit); 

