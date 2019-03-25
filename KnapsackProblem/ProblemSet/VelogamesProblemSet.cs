using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace KnapsackProblem.ProblemSet
{
    public class VelogamesProblemSet : IProblemSet
    {
        public Knapsack Knapsack { get; set; }
        public List<Item> DataSet { get; set; }

        public VelogamesProblemSet()
        {
            Knapsack = new Knapsack(102, 9, false);

            List<Item> allRiders = LoadVelogamesDataFromFile("P:\\Temp\\CatalunyaVelogamesData.csv");
            List<Item> rankedRiders = LoadPCSDataFromFile("P:\\Temp\\CatalunyaRidersByGC.csv");
            MatchRiders(allRiders, rankedRiders);

            LogFile.WriteLine("Rider, Weight, Value");
            foreach (var rider in allRiders)
            {
                LogFile.WriteLine("{0}, {1}, {2}", rider.Name, rider.Weight, rider.Value);
            }

            DataSet = allRiders;
        }

        public static List<Item> LoadVelogamesDataFromFile(string fileName)
        {
            List<Item> allRiders = new List<Item>();
            try
            {
                // Open the text file using a stream reader.
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    int lines = 0;

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        // skip header
                        if (lines > 0)
                        {
                            string riderName = fields[0].ToUpper();
                            string riderTeam = fields[1];
                            int riderEarnedPoints = int.Parse(fields[2]);
                            int riderCost = int.Parse(fields[3]);

                            Item rider = new Item(riderName, riderCost, 0);

                            allRiders.Add(rider);
                        }

                        lines++;
                    }
                    LogFile.WriteLine("{0} lines read (including header)", lines);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return allRiders;
        }

        public static List<Item> LoadPCSDataFromFile(string fileName)
        {
            List<Item> rankedRiders = new List<Item>();

            try
            {
                // Open the text file using a stream reader.
                using (TextFieldParser parser = new TextFieldParser(fileName))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    int lines = 0;

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        // skip header
                        if (lines > 0)
                        {
                            string raceGCPosition = fields[0];
                            string riderName = fields[1].ToUpper();
                            string riderTeam = fields[2];
                            int riderPoints = int.Parse(fields[3]);
                            string overallGCPosition = fields[4];

                            Item rider = new Item(riderName, 0, riderPoints);
                            rankedRiders.Add(rider);
                        }

                        lines++;
                    }
                    LogFile.WriteLine("{0} lines read (including header)", lines);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return rankedRiders;
        }

        private static void MatchRiders(List<Item> allRiders, List<Item> rankedRiders)
        {
            foreach (Item rider in allRiders)
            {
                var rankedRider = rankedRiders.Find(i => i.Name == rider.Name);

                if (rankedRider != default(Item))
                {
                    rider.Value = rankedRider.Value;
                }
            }
        }
    }
}