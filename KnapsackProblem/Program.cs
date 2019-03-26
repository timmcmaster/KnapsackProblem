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
            Console.WriteLine("Start...");

            //IProblemSet problemSet = new VelogamesProblemSet();
            IProblemSet problemSet = new KellererBookProblemSet();

            Console.WriteLine(Directory.GetCurrentDirectory());

            ISolver solver = new Solver2DNonRecursiveByProfit(problemSet.Knapsack, problemSet.DataSet);

            solver.Solve();

            //solver.LogDataValues();

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
