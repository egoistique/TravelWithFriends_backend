
namespace Travel.Services.Statistics;

public class StatisticsModel
{
    public List<ChartData> TeamExpensesData { get; set; }
    public List<ChartData> CategoryExpensesData { get; set; }
}

public class ChartData
{
    public string Name { get; set; }
    public decimal Value { get; set; }
}
