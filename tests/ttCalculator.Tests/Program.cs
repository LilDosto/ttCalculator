using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetMatrix; 

namespace MathApp.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DEBUG SOLVER STARTED");
            try
            {
                List<string> eqs = new List<string> { "x+y-3", "x-y-1" };
                Dictionary<string, decimal> init = new Dictionary<string, decimal> { { "x", 0 }, { "y", 0 } };
                
                Console.WriteLine("Calling SolveEquationSystem...");
                var res = NumericalMethods.MethodMultiVar_SolveEquationSystem(eqs, init, 2, 0.001m);
                
                Console.WriteLine("Result count: " + (res == null ? "NULL" : res.Count.ToString()));
                if(res != null)
                {
                    foreach(var kv in res) Console.WriteLine(kv.Key + "=" + kv.Value);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION CAUGHT: " + ex.ToString());
            }
        }
    }
}
