using System;
using System.IO;
using KnapsackProblem.ProblemSet;
using KnapsackProblem.Solver;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            IProblemSet problemSet = new VelogamesProblemSet();
            //IProblemSet problemSet = new KellererBookProblemSet();

            Console.WriteLine(Directory.GetCurrentDirectory());

            ISolver solver = new Solver3DRecursiveByWeight(problemSet.Knapsack, problemSet.DataSet);

            solver.Solve();

            //solver.LogDataValues();

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
