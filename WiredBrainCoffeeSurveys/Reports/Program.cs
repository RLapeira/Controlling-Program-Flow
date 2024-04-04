using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quitApp = false;

            do
            {
                Console.WriteLine("Please specify a report to run (rewards, comments, tasks, quit):");
                var selectedReport = Console.ReadLine();

                Console.WriteLine("Please specify which quarter of data: (q1, q2)");
                var selectedData = Console.ReadLine();

                var surveyResults = SurveyDataService.GetSurveyDataByFileName(selectedFile);

                switch (selectedReport)
                {
                    case "rewards":
                        RewardsReportService.GenerateWinnerEmails(surveyResults);
                        break;
                    case "comments":
                        CommentsReportService.GenerateCommentsReport(surveyResults);
                        break;
                    case "tasks":
                        TasksReportService.GenerateTasksReport(surveyResults);
                        break;
                    case "quit":
                        quitApp = true;
                        break;
                    default:
                        Console.WriteLine("Sorry, that's not a valid option.");
                        break;
                }
                Console.WriteLine();
            }
            while (!quitApp);
        }
    }
}
