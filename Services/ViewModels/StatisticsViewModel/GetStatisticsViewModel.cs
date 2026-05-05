namespace Services.ViewModels.StatisticsViewModel;

public class GetStatisticsViewModel
{
    public int TotalJobs { get; set; }
    public int CompletedJobs { get; set; }
    public int InProgressJobs { get; set; }
    public int FailedJobs { get; set; }
    public int TotalMobilizedWorkers { get; set; }
    public string AverageJobDuration { get; set; }
}