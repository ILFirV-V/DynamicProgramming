namespace DynamicProgramming.SecondLaboratory.Models;

public class InvestmentResult
{
    public int OptimalProfit { get; }
    public int[] OptimalInvestments { get; }

    public InvestmentResult(int optimalProfit, int[] optimalInvestments)
    {
        OptimalProfit = optimalProfit;
        OptimalInvestments = optimalInvestments;
    }
}