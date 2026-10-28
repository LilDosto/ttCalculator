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
            Console.WriteLine("========================================");
            Console.WriteLine("ttCalculator v2.3 Algorithm Verification");
            Console.WriteLine("========================================");

            int passed = 0;
            int total = 0;

            void AssertTrue(bool condition, string testName)
            {
                total++;
                if (condition)
                {
                    passed++;
                    Console.WriteLine($"[PASS] {testName}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[FAIL] {testName}");
                    Console.ResetColor();
                }
            }

            // Test 1: Linear system solver
            try
            {
                List<string> eqs = new List<string> { "x+y-3", "x-y-1" };
                Dictionary<string, decimal> init = new Dictionary<string, decimal> { { "x", 0 }, { "y", 0 } };
                var res = NumericalMethods.MethodMultiVar_SolveEquationSystem(eqs, init, 2, 0.001m);
                AssertTrue(res != null && Math.Abs((double)res["x"] - 2.0) < 1e-4 && Math.Abs((double)res["y"] - 1.0) < 1e-4, "Equation System Solver (2x2)");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "Equation System Solver: " + ex.Message);
            }

            // Test 2: Continued Fractions (ToFraction)
            try
            {
                var f1 = NumericalMethods.ToFraction(1m / 6m);
                AssertTrue(f1.HasValue && f1.Value.Num == 1 && f1.Value.Denom == 6, "ToFraction(1/6) == 1/6");

                var f2 = NumericalMethods.ToFraction(1m / 120m);
                AssertTrue(f2.HasValue && f2.Value.Num == 1 && f2.Value.Denom == 120, "ToFraction(1/120) == 1/120");

                var f3 = NumericalMethods.ToFraction(0.75m);
                AssertTrue(f3.HasValue && f3.Value.Num == 3 && f3.Value.Denom == 4, "ToFraction(0.75) == 3/4");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "ToFraction: " + ex.Message);
            }

            // Test 3: Multi-root interval scanning
            try
            {
                // sin(x) = 0 in [-3.5, 3.5] has roots at -pi, 0, pi
                var roots = NumericalMethods.FindAllRoots("sin(x)", -3.5m, 3.5m, "x");
                AssertTrue(roots.Count >= 3, $"FindAllRoots for sin(x) found {roots.Count} roots (expected >= 3)");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "FindAllRoots: " + ex.Message);
            }

            // Test 4: Single ODE (y' = y, y(0) = 1 => y(1) = e ≈ 2.71828)
            try
            {
                var eulerRes = DifferentialEquations.SolveOde("y", 0m, 1m, 1m, 100, "Euler");
                decimal yEuler = eulerRes[eulerRes.Count - 1].Y;
                AssertTrue(Math.Abs((double)yEuler - Math.E) < 0.05, $"ODE Euler y(1) = {yEuler:F4} (expected ≈ e)");

                var rk4Res = DifferentialEquations.SolveOde("y", 0m, 1m, 1m, 100, "RK4");
                decimal yRk4 = rk4Res[rk4Res.Count - 1].Y;
                AssertTrue(Math.Abs((double)yRk4 - Math.E) < 1e-4, $"ODE RK4 y(1) = {yRk4:F6} (expected ≈ e)");

                var rk8Res = DifferentialEquations.SolveOde("y", 0m, 1m, 1m, 100, "RK8");
                decimal yRk8 = rk8Res[rk8Res.Count - 1].Y;
                AssertTrue(Math.Abs((double)yRk8 - Math.E) < 1e-5, $"ODE RK8 y(1) = {yRk8:F8} (expected ≈ e)");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "SolveOde: " + ex.Message);
            }

            // Test 5: ODE System (Harmonic Oscillator: y1' = y2, y2' = -y1 with y1(0)=1, y2(0)=0)
            // Exact solution: y1(t) = cos(t), y2(t) = -sin(t)
            // At t = 2*pi, y1 ≈ 1.0, y2 ≈ 0.0
            try
            {
                List<string> sysEqs = new List<string> { "y2", "-y1" };
                List<string> vars = new List<string> { "y1", "y2" };
                Dictionary<string, decimal> inits = new Dictionary<string, decimal> { { "y1", 1m }, { "y2", 0m } };
                decimal tEnd = (decimal)(2.0 * Math.PI);

                var sysRes = DifferentialEquations.SolveOdeSystem(sysEqs, vars, inits, 0m, tEnd, 200, "RK4");
                var last = sysRes[sysRes.Count - 1];
                decimal y1End = last.YValues["y1"];
                decimal y2End = last.YValues["y2"];
                AssertTrue(Math.Abs((double)y1End - 1.0) < 1e-3 && Math.Abs((double)y2End) < 1e-3, 
                    $"ODE System RK4 Harmonic Oscillator y1={y1End:F5}, y2={y2End:F5} (expected ~1, ~0)");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "SolveOdeSystem: " + ex.Message);
            }

            // Test 6: 1D Heat Equation (u_t = alpha * u_xx)
            // With u(x,0) = sin(pi*x), exact solution decays as exp(-alpha*pi^2*t)*sin(pi*x)
            try
            {
                var pdeHeat = DifferentialEquations.Solve1DPde(
                    DifferentialEquations.PdeType.HeatEquation,
                    "sin(pi * x)",
                    1.0m,
                    0.2m,
                    21,
                    100,
                    0.1m,
                    0m,
                    0m
                );
                decimal midU = pdeHeat.U[10, 99]; // x = 0.5 at final time
                double expected = Math.Exp(-0.1 * Math.PI * Math.PI * 0.2) * Math.Sin(Math.PI * 0.5);
                AssertTrue(Math.Abs((double)midU - expected) < 0.02, 
                    $"1D Heat FTCS mid-point U={midU:F4} (expected ≈ {expected:F4})");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "Solve1DPde Heat: " + ex.Message);
            }

            // Test 7: Coupled Reaction-Diffusion PDE System
            try
            {
                var coupledPde = DifferentialEquations.SolveCoupledPdeSystem(
                    "-u * v^2 + 0.04 * (1 - u)",
                    "u * v^2 - 0.09 * v",
                    "1 - 0.5 * exp(-100 * (x - 0.5)^2)",
                    "0.25 * exp(-100 * (x - 0.5)^2)",
                    1.0m,
                    0.5m,
                    21,
                    100,
                    0.001m,
                    0.0005m
                );
                AssertTrue(coupledPde.U != null && coupledPde.V != null && coupledPde.U[10, 99] > 0m, 
                    "Coupled PDE System (Gray-Scott Reaction-Diffusion)");
            }
            catch (Exception ex)
            {
                AssertTrue(false, "SolveCoupledPdeSystem: " + ex.Message);
            }

            Console.WriteLine("========================================");
            Console.WriteLine($"RESULTS: {passed}/{total} tests passed ({(passed * 100.0 / total):F1}%)");
            Console.WriteLine("========================================");

            if (passed != total) Environment.Exit(1);
        }
    }
}
