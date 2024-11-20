namespace DynamicProgramming.SecondLaboratory.Models;

public record ProfitOptimizationResult
{
    public int[,] MaxProfit { get; init; }
    public int[,] InvestmentDistribution { get; init; }
}