using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnapsackProblem.ProblemSet;
using KnapsackProblem.Solver;
using Microsoft.VisualBasic.FileIO;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Start...");

            //IProblemSet problemSet = new VelogamesProblemSet();
            IProblemSet problemSet = new WikipediaProblemSet();

            Console.WriteLine(Directory.GetCurrentDirectory());

            ISolver solver = new RecursiveSolver2D(problemSet.Knapsack, problemSet.DataSet);

            solver.Solve();

            //solver.LogDataValues();

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
