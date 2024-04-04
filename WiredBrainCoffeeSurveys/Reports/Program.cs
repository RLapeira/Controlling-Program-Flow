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

                var surveyResults = JsonConverter.DeserializeObject<SurveyResults>
                    (File.ReadAllText($"data/{selectedData}.json"));

                switch (selectedReport)
                {
                    case "rewards":
                        GenerateWinnerEmails(surveyResults);
                        break;
                    case "comments":
                        GenerateTasksReport(surveyResults);
                        break;
                    case "tasks":
                        GenerateCommentsReport(surveyResults);
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

        private static void GenerateWinnerEmails(SurveyResults results)
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            while (selectedEmails.Count < 2 && counter < results.Responses.Count)
            {
                var currentItem = results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.EmailAddress);
                    Console.WriteLine(currentItem.EmailAddress);
                }

                counter++;
            }

            File.WriteAllLines("WinnersReport.csv", selectedEmails);
        }

        private static void GenerateCommentsReport(SurveyResults results)
        {
            for (var i = 0; i < results.Responses.Count; i++)
            {
                var currentResponse = results.Responses[i];

                if (currentResponse.WouldRecommend < 7.0)
                {
                    Console.WriteLine(currentResponse.Comments);
                }
            }

            foreach (var response in results.Responses)
            {
                if (response.AreaToImprove == results.AreaToImprove)
                {
                    if (response.AreaToImprove == results.AreaToImprove)
                    {
                        Console.WriteLine(response.Comments);
                    }
                }
            }
        }

        public static void GenerateTasksReport(SurveyResults results)
        {
            var tasks = new List<string>();

            // Calculated Values
            double responseRate = results.NumberResponded / results.NumberSurveyed;
            double overallScore = (results.ServiceScore + results.CoffeeScore
                + results.FoodScore + results.PriceScore) / 4;

            if (results.CoffeeScore > results.FoodScore)
            {
                tasks.Add("Investigate coffee recipes and ingredients.");
            }

            if (overallScore > 8.0)
            {
                tasks.Add("Work with leadership to reward staff");
            }
            else
            {
                tasks.Add("Work with employees for improvement ideas.");
            }

            tasks.Add(responseRate switch
            {
                var rate when rate < .33 => "Research options to improve response rate.",
                var rate when rate > .33 && rate < .66 => "Reward participants with free coffee coupon.",
                var rate when rate > .66 => "Reward participants with discount coffee coupon."
            });

            tasks.Add(results.AreaToImprove switch
            {
                "RewardsProgram" => "Revisit the rewards deals.",
                "Cleanliness" => "Contact the cleaning vendor.",
                "MobileApp" => "Contact consulting firm about app.",
                _ => "Investigate individual comments for ideas."
            });

            Console.WriteLine(Environment.NewLine + "Tasks Output:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            File.WriteAllLines("TasksReport.csv", tasks);
        }
    }
}
