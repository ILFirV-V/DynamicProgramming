namespace DynamicProgramming.SecondLaboratory.Factories;

public class FunctionFactory
{
    private readonly Dictionary<int, int> functionValues;
    private readonly int step;

    public FunctionFactory(int step, Dictionary<int, int> functionValues)
    {
        this.step = step;
        this.functionValues = functionValues;
    }

    public int Calculate(int sum)
    {
        if (sum % step != 0)
        {
            throw new ArgumentException("Sum must be divisible by step");
        }

        var index = sum / step;
        return functionValues.GetValueOrDefault(index, 0);
    }
}