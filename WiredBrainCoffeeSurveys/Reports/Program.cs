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

            if (responseRate < .33)
            {
                tasks.Add("Research options to improve response rate.");
            }
            else if (responseRate > .33 && responseRate < .66)
            {
                tasks.Add("Reward participants with free coffee coupon.");
            }
            else
            {
                tasks.Add("Reward participants with discount coffee coupon.");
            }

            switch (results.AreaToImprove)
            {
                case "RewardsProgram":
                    tasks.Add("Revisit the rewards deals.");
                    break;

                case "Cleanliness":
                    tasks.Add("Contact the cleaning vendor.");
                    break;

                case "MobileApp":
                    tasks.Add("Contact consulting firm about app.");
                    break;

                default:
                    tasks.Add("Investigate individual comments for ideas.");
                    break;
            }
        }
    }
}
