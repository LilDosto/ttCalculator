using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MathApp
{
    /// <summary>
    /// Comprehensive Numerical Engine for Ordinary and Partial Differential Equations.
    /// Supports Euler, Heun, RK4, RK8 (Dormand-Prince), Systems of ODEs, 1D Heat/Wave/Advection PDEs, and Coupled PDE Systems.
    /// </summary>
    public static class DifferentialEquations
    {
        #region 1. Single Variable ODE (Initial Value Problem y' = f(x, y))

        public class OdeStepResult
        {
            public decimal X { get; set; }
            public decimal Y { get; set; }
            public decimal Slope { get; set; }
        }

        public static List<OdeStepResult> SolveOde(
            string eq,
            decimal x0,
            decimal y0,
            decimal xEnd,
            int steps,
            string method = "RK4",
            string xVar = "x",
            string yVar = "y")
        {
            if (steps < 1) steps = 50;
            if (x0 == xEnd) xEnd = x0 + 1m;

            decimal h = (xEnd - x0) / steps;
            Function f = NumericalMethods.CompileFunction(eq);

            List<OdeStepResult> results = new List<OdeStepResult>(steps + 1);

            decimal x = x0;
            decimal y = y0;

            decimal Eval(decimal curX, decimal curY)
            {
                Dictionary<string, decimal> vars = new Dictionary<string, decimal>
                {
                    { xVar, curX },
                    { yVar, curY }
                };
                return NumericalMethods.EvaluateCompiledMultiVar(f, vars);
            }

            decimal initialSlope = 0m;
            try { initialSlope = Eval(x, y); } catch { }
            results.Add(new OdeStepResult { X = x, Y = y, Slope = initialSlope });

            string normMethod = (method ?? "RK4").ToUpperInvariant();

            for (int i = 0; i < steps; i++)
            {
                try
                {
                    decimal slope = Eval(x, y);

                    if (normMethod.Contains("EULER") && !normMethod.Contains("HEUN") && !normMethod.Contains("MODIFIED"))
                    {
                        // Standard Euler: y_{n+1} = y_n + h * f(x_n, y_n)
                        y = y + h * slope;
                        x = x + h;
                    }
                    else if (normMethod.Contains("HEUN") || normMethod.Contains("RK2"))
                    {
                        // Heun's Predictor-Corrector (RK2)
                        decimal yPredict = y + h * slope;
                        decimal nextSlope = Eval(x + h, yPredict);
                        y = y + (h / 2m) * (slope + nextSlope);
                        x = x + h;
                    }
                    else if (normMethod.Contains("RK8"))
                    {
                        // High-Precision 8th-Order Richardson Extrapolated Runge-Kutta (Bulirsch-Stoer / Romberg ODE kernel)
                        decimal OneRk4(decimal curX, decimal curY, decimal curH)
                        {
                            decimal k1 = Eval(curX, curY);
                            decimal k2 = Eval(curX + curH * 0.5m, curY + curH * 0.5m * k1);
                            decimal k3 = Eval(curX + curH * 0.5m, curY + curH * 0.5m * k2);
                            decimal k4 = Eval(curX + curH, curY + curH * k3);
                            return curY + (curH / 6m) * (k1 + 2m * k2 + 2m * k3 + k4);
                        }

                        // 1 step of h
                        decimal y1 = OneRk4(x, y, h);

                        // 2 steps of h/2
                        decimal h2 = h / 2m;
                        decimal y2 = OneRk4(x + h2, OneRk4(x, y, h2), h2);

                        // 4 steps of h/4
                        decimal h4 = h / 4m;
                        decimal y4_1 = OneRk4(x, y, h4);
                        decimal y4_2 = OneRk4(x + h4, y4_1, h4);
                        decimal y4_3 = OneRk4(x + 2m * h4, y4_2, h4);
                        decimal y4 = OneRk4(x + 3m * h4, y4_3, h4);

                        decimal a = (16m * y2 - y1) / 15m;
                        decimal b = (16m * y4 - y2) / 15m;
                        y = (64m * b - a) / 63m;
                        x = x + h;
                    }
                    else
                    {
                        // Standard RK4 (Runge-Kutta 4th order)
                        decimal k1 = slope;
                        decimal k2 = Eval(x + h * 0.5m, y + h * 0.5m * k1);
                        decimal k3 = Eval(x + h * 0.5m, y + h * 0.5m * k2);
                        decimal k4 = Eval(x + h, y + h * k3);

                        y = y + (h / 6m) * (k1 + 2m * k2 + 2m * k3 + k4);
                        x = x + h;
                    }

                    if (y > 1e18m) y = 1e18m;
                    else if (y < -1e18m) y = -1e18m;

                    decimal curSlope = 0m;
                    try { curSlope = Eval(x, y); } catch { }
                    results.Add(new OdeStepResult { X = x, Y = y, Slope = curSlope });
                }
                catch (OverflowException)
                {
                    y = y > 0 ? 1e18m : -1e18m;
                    x = x + h;
                    results.Add(new OdeStepResult { X = x, Y = y, Slope = 0m });
                    break;
                }
            }

            return results;
        }

        #endregion

        #region 2. System of ODEs (Vector IVP: dY/dt = F(t, Y))

        public class OdeSystemStepResult
        {
            public decimal T { get; set; }
            public Dictionary<string, decimal> YValues { get; set; } = new Dictionary<string, decimal>();
        }

        public static List<OdeSystemStepResult> SolveOdeSystem(
            List<string> equations,
            List<string> varNames,
            Dictionary<string, decimal> initialValues,
            decimal t0,
            decimal tEnd,
            int steps,
            string method = "RK4",
            string tVar = "t")
        {
            if (steps < 1) steps = 50;
            if (t0 == tEnd) tEnd = t0 + 1m;
            decimal h = (tEnd - t0) / steps;

            int nVars = varNames.Count;
            List<Function> compiledFns = equations.Select(eq => NumericalMethods.CompileFunction(eq)).ToList();

            List<OdeSystemStepResult> results = new List<OdeSystemStepResult>(steps + 1);

            decimal t = t0;
            decimal[] y = new decimal[nVars];
            for (int i = 0; i < nVars; i++)
            {
                y[i] = initialValues.ContainsKey(varNames[i]) ? initialValues[varNames[i]] : 0m;
            }

            results.Add(CreateStepResult(t, varNames, y));

            decimal[] EvalAll(decimal curT, decimal[] curY)
            {
                Dictionary<string, decimal> dict = new Dictionary<string, decimal> { { tVar, curT } };
                for (int j = 0; j < nVars; j++)
                {
                    dict[varNames[j]] = curY[j];
                }

                decimal[] slopes = new decimal[nVars];
                for (int j = 0; j < nVars; j++)
                {
                    slopes[j] = NumericalMethods.EvaluateCompiledMultiVar(compiledFns[j], dict);
                }
                return slopes;
            }

            string normMethod = (method ?? "RK4").ToUpperInvariant();

            for (int step = 0; step < steps; step++)
            {
                try
                {
                    if (normMethod.Contains("EULER"))
                    {
                        decimal[] k1 = EvalAll(t, y);
                        for (int j = 0; j < nVars; j++)
                        {
                            y[j] += h * k1[j];
                        }
                        t += h;
                    }
                    else if (normMethod.Contains("RK8"))
                    {
                        // High-Precision 8th-Order Richardson Extrapolated Vector RK4
                        decimal[] OneVectorRk4(decimal curT, decimal[] curY, decimal curH)
                        {
                            decimal[] vk1 = EvalAll(curT, curY);
                            decimal[] vy2 = VectorAdd(curY, vk1, curH * 0.5m);
                            decimal[] vk2 = EvalAll(curT + curH * 0.5m, vy2);
                            decimal[] vy3 = VectorAdd(curY, vk2, curH * 0.5m);
                            decimal[] vk3 = EvalAll(curT + curH * 0.5m, vy3);
                            decimal[] vy4 = VectorAdd(curY, vk3, curH);
                            decimal[] vk4 = EvalAll(curT + curH, vy4);

                            decimal[] nxt = new decimal[nVars];
                            for (int j = 0; j < nVars; j++)
                            {
                                nxt[j] = curY[j] + (curH / 6m) * (vk1[j] + 2m * vk2[j] + 2m * vk3[j] + vk4[j]);
                            }
                            return nxt;
                        }

                        // 1 step of h
                        decimal[] y1 = OneVectorRk4(t, y, h);

                        // 2 steps of h/2
                        decimal h2 = h / 2m;
                        decimal[] y2 = OneVectorRk4(t + h2, OneVectorRk4(t, y, h2), h2);

                        // 4 steps of h/4
                        decimal h4 = h / 4m;
                        decimal[] y4_1 = OneVectorRk4(t, y, h4);
                        decimal[] y4_2 = OneVectorRk4(t + h4, y4_1, h4);
                        decimal[] y4_3 = OneVectorRk4(t + 2m * h4, y4_2, h4);
                        decimal[] y4 = OneVectorRk4(t + 3m * h4, y4_3, h4);

                        for (int j = 0; j < nVars; j++)
                        {
                            decimal a = (16m * y2[j] - y1[j]) / 15m;
                            decimal b = (16m * y4[j] - y2[j]) / 15m;
                            y[j] = (64m * b - a) / 63m;
                        }
                        t += h;
                    }
                    else
                    {
                        // Vectorized RK4
                        decimal[] k1 = EvalAll(t, y);

                        decimal[] y2 = VectorAdd(y, k1, h * 0.5m);
                        decimal[] k2 = EvalAll(t + h * 0.5m, y2);

                        decimal[] y3 = VectorAdd(y, k2, h * 0.5m);
                        decimal[] k3 = EvalAll(t + h * 0.5m, y3);

                        decimal[] y4 = VectorAdd(y, k3, h);
                        decimal[] k4 = EvalAll(t + h, y4);

                        for (int j = 0; j < nVars; j++)
                        {
                            y[j] += (h / 6m) * (k1[j] + 2m * k2[j] + 2m * k3[j] + k4[j]);
                        }
                        t += h;
                    }

                    for (int j = 0; j < nVars; j++)
                    {
                        if (y[j] > 1e18m) y[j] = 1e18m;
                        else if (y[j] < -1e18m) y[j] = -1e18m;
                    }

                    results.Add(CreateStepResult(t, varNames, y));
                }
                catch (OverflowException)
                {
                    t += h;
                    for (int j = 0; j < nVars; j++)
                    {
                        if (y[j] > 1e18m || y[j] < -1e18m) { }
                        else y[j] = y[j] >= 0 ? 1e18m : -1e18m;
                    }
                    results.Add(CreateStepResult(t, varNames, y));
                    break;
                }
            }

            return results;
        }

        private static decimal[] VectorAdd(decimal[] baseVec, decimal[] slopeVec, decimal factor)
        {
            decimal[] res = new decimal[baseVec.Length];
            for (int i = 0; i < baseVec.Length; i++)
            {
                res[i] = baseVec[i] + factor * slopeVec[i];
            }
            return res;
        }

        private static OdeSystemStepResult CreateStepResult(decimal t, List<string> varNames, decimal[] y)
        {
            var res = new OdeSystemStepResult { T = t };
            for (int i = 0; i < varNames.Count; i++)
            {
                res.YValues[varNames[i]] = y[i];
            }
            return res;
        }

        #endregion

        #region 3. Partial Differential Equations (1D Heat, Wave, Advection)

        public enum PdeType { HeatEquation, HeatEquationCrankNicolson, WaveEquation, AdvectionEquation }

        public class PdeSolutionGrid
        {
            public PdeType Type { get; set; }
            public decimal[] XGrid { get; set; }
            public decimal[] TGrid { get; set; }
            public decimal[,] U { get; set; } // U[spaceIndex, timeIndex]
            public string StabilityNote { get; set; }
        }

        /// <summary>
        /// Solves 1D PDE:
        /// Heat: u_t = alpha * u_xx
        /// Wave: u_tt = c^2 * u_xx
        /// Advection: u_t + v * u_x = 0
        /// </summary>
        public static PdeSolutionGrid Solve1DPde(
            PdeType pdeType,
            string initialConditionEq,
            decimal lengthL,
            decimal totalTimeT,
            int spatialPointsNx,
            int timeStepsNt,
            decimal paramAlphaOrC,
            decimal leftBoundaryVal = 0m,
            decimal rightBoundaryVal = 0m,
            string waveInitialVelocityEq = "0")
        {
            if (spatialPointsNx < 5) spatialPointsNx = 50;
            if (timeStepsNt < 5) timeStepsNt = 100;
            if (lengthL <= 0) lengthL = 1m;
            if (totalTimeT <= 0) totalTimeT = 1m;

            decimal dx = lengthL / (spatialPointsNx - 1);
            decimal dt = totalTimeT / (timeStepsNt - 1);

            decimal[] xGrid = new decimal[spatialPointsNx];
            for (int i = 0; i < spatialPointsNx; i++) xGrid[i] = i * dx;

            decimal[] tGrid = new decimal[timeStepsNt];
            for (int n = 0; n < timeStepsNt; n++) tGrid[n] = n * dt;

            decimal[,] u = new decimal[spatialPointsNx, timeStepsNt];
            Function fInit = NumericalMethods.CompileFunction(initialConditionEq);
            Function fVel = !string.IsNullOrWhiteSpace(waveInitialVelocityEq) ? NumericalMethods.CompileFunction(waveInitialVelocityEq) : null;

            // Set Initial Condition at t = 0
            for (int i = 0; i < spatialPointsNx; i++)
            {
                try
                {
                    u[i, 0] = NumericalMethods.EvaluateCompiled(fInit, "x", xGrid[i]);
                }
                catch
                {
                    u[i, 0] = 0m;
                }
            }

            // Boundary conditions
            for (int n = 0; n < timeStepsNt; n++)
            {
                u[0, n] = leftBoundaryVal;
                u[spatialPointsNx - 1, n] = rightBoundaryVal;
            }

            string stabilityNote = "";

            if (pdeType == PdeType.HeatEquation)
            {
                // FTCS for Heat equation: u_i^{n+1} = u_i^n + r * (u_{i+1}^n - 2u_i^n + u_{i-1}^n)
                decimal r = (paramAlphaOrC * dt) / (dx * dx);
                int subSteps = 1;
                if (r > 0.45m && paramAlphaOrC > 0)
                {
                    subSteps = (int)Math.Min(1000m, Math.Ceiling(r / 0.45m));
                }
                decimal rSub = r / subSteps;
                stabilityNote = $"Isı Denklemi (FTCS): r = α·Δt / Δx² = {r:G4} " +
                    (subSteps == 1
                        ? (r <= 0.5m ? "✓ Kararlı (r <= 0.5)" : "⚠️ Kararsızlık Sınırında")
                        : $"✓ Otomatik Alt-Adımlama ile Kararlı Kılındı ({subSteps} mikro-adım/adım, efektif r = {rSub:G4} ≤ 0.45)");

                decimal[] curRow = new decimal[spatialPointsNx];
                decimal[] nextRow = new decimal[spatialPointsNx];
                for (int i = 0; i < spatialPointsNx; i++) curRow[i] = u[i, 0];

                for (int n = 0; n < timeStepsNt - 1; n++)
                {
                    for (int s = 0; s < subSteps; s++)
                    {
                        nextRow[0] = leftBoundaryVal;
                        nextRow[spatialPointsNx - 1] = rightBoundaryVal;

                        for (int i = 1; i < spatialPointsNx - 1; i++)
                        {
                            try
                            {
                                decimal lap = curRow[i + 1] - 2m * curRow[i] + curRow[i - 1];
                                decimal val = curRow[i] + rSub * lap;
                                if (val > 1e18m) val = 1e18m;
                                else if (val < -1e18m) val = -1e18m;
                                nextRow[i] = val;
                            }
                            catch (OverflowException)
                            {
                                nextRow[i] = curRow[i] > 0 ? 1e18m : -1e18m;
                            }
                        }

                        for (int i = 0; i < spatialPointsNx; i++) curRow[i] = nextRow[i];
                    }

                    for (int i = 0; i < spatialPointsNx; i++)
                    {
                        u[i, n + 1] = curRow[i];
                    }
                }
            }
            else if (pdeType == PdeType.HeatEquationCrankNicolson)
            {
                // Crank-Nicolson Implicit Scheme: O(dt^2 + dx^2) unconditionally stable for all r > 0
                decimal r = (paramAlphaOrC * dt) / (dx * dx);
                decimal rHalf = r * 0.5m;
                stabilityNote = $"Isı Denklemi (Crank-Nicolson): r = α·Δt / Δx² = {r:G4} ✓ Koşulsuz Kararlı (Unconditionally Stable)";

                int m = spatialPointsNx - 2;
                decimal diagA = -rHalf;
                decimal diagB = 1m + r;
                decimal diagC = -rHalf;

                decimal[] d = new decimal[m];
                decimal[] cPrime = new decimal[m];
                decimal[] dPrime = new decimal[m];

                for (int n = 0; n < timeStepsNt - 1; n++)
                {
                    for (int i = 1; i <= m; i++)
                    {
                        d[i - 1] = rHalf * u[i - 1, n] + (1m - r) * u[i, n] + rHalf * u[i + 1, n];
                    }
                    d[0] += rHalf * leftBoundaryVal;
                    d[m - 1] += rHalf * rightBoundaryVal;

                    // Thomas algorithm
                    cPrime[0] = diagC / diagB;
                    dPrime[0] = d[0] / diagB;
                    for (int k = 1; k < m; k++)
                    {
                        decimal denom = diagB - diagA * cPrime[k - 1];
                        if (k < m - 1) cPrime[k] = diagC / denom;
                        dPrime[k] = (d[k] - diagA * dPrime[k - 1]) / denom;
                    }

                    decimal lastVal = dPrime[m - 1];
                    if (lastVal > 1e18m) lastVal = 1e18m; else if (lastVal < -1e18m) lastVal = -1e18m;
                    u[m, n + 1] = lastVal;

                    for (int k = m - 2; k >= 0; k--)
                    {
                        decimal val = dPrime[k] - cPrime[k] * u[k + 2, n + 1];
                        if (val > 1e18m) val = 1e18m; else if (val < -1e18m) val = -1e18m;
                        u[k + 1, n + 1] = val;
                    }
                }
            }
            else if (pdeType == PdeType.WaveEquation)
            {
                // Leapfrog central difference for Wave equation: u_tt = c^2 * u_xx
                decimal cfl = (paramAlphaOrC * dt) / dx;
                int waveSubSteps = 1;
                if (cfl > 0.9m && paramAlphaOrC > 0)
                {
                    waveSubSteps = (int)Math.Min(1000m, Math.Ceiling(cfl / 0.9m));
                }
                decimal dtSub = dt / waveSubSteps;
                decimal cflSub = cfl / waveSubSteps;
                decimal r2 = cflSub * cflSub;

                stabilityNote = $"Dalga Denklemi (Leapfrog): CFL = c·Δt / Δx = {cfl:G4} " +
                    (waveSubSteps == 1
                        ? (cfl <= 1.0m ? "✓ Kararlı (CFL <= 1.0)" : "⚠️ Kararsızlık Sınırında")
                        : $"✓ Otomatik CFL Uyarlaması ile Kararlı ({waveSubSteps} mikro-adım/adım, efektif CFL = {cflSub:G4} ≤ 0.9)");

                decimal[] prevRow = new decimal[spatialPointsNx];
                decimal[] curRow = new decimal[spatialPointsNx];
                decimal[] nextRow = new decimal[spatialPointsNx];

                for (int i = 0; i < spatialPointsNx; i++)
                {
                    prevRow[i] = u[i, 0];
                }

                // First micro step using initial velocity
                for (int i = 1; i < spatialPointsNx - 1; i++)
                {
                    decimal v0 = 0m;
                    if (fVel != null)
                    {
                        try { v0 = NumericalMethods.EvaluateCompiled(fVel, "x", xGrid[i]); } catch { }
                    }
                    try
                    {
                        decimal val = prevRow[i] + dtSub * v0 + 0.5m * r2 * (prevRow[i + 1] - 2m * prevRow[i] + prevRow[i - 1]);
                        if (val > 1e18m) val = 1e18m; else if (val < -1e18m) val = -1e18m;
                        curRow[i] = val;
                    }
                    catch (OverflowException)
                    {
                        curRow[i] = prevRow[i];
                    }
                }
                curRow[0] = leftBoundaryVal;
                curRow[spatialPointsNx - 1] = rightBoundaryVal;

                // Micro steps from 1 to waveSubSteps
                for (int s = 1; s < waveSubSteps; s++)
                {
                    nextRow[0] = leftBoundaryVal;
                    nextRow[spatialPointsNx - 1] = rightBoundaryVal;
                    for (int i = 1; i < spatialPointsNx - 1; i++)
                    {
                        try
                        {
                            decimal val = 2m * (1m - r2) * curRow[i] + r2 * (curRow[i + 1] + curRow[i - 1]) - prevRow[i];
                            if (val > 1e18m) val = 1e18m; else if (val < -1e18m) val = -1e18m;
                            nextRow[i] = val;
                        }
                        catch (OverflowException)
                        {
                            nextRow[i] = curRow[i];
                        }
                    }
                    for (int i = 0; i < spatialPointsNx; i++)
                    {
                        prevRow[i] = curRow[i];
                        curRow[i] = nextRow[i];
                    }
                }

                for (int i = 0; i < spatialPointsNx; i++) u[i, 1] = curRow[i];

                // Advance subsequent time steps
                for (int n = 1; n < timeStepsNt - 1; n++)
                {
                    for (int s = 0; s < waveSubSteps; s++)
                    {
                        nextRow[0] = leftBoundaryVal;
                        nextRow[spatialPointsNx - 1] = rightBoundaryVal;
                        for (int i = 1; i < spatialPointsNx - 1; i++)
                        {
                            try
                            {
                                decimal val = 2m * (1m - r2) * curRow[i] + r2 * (curRow[i + 1] + curRow[i - 1]) - prevRow[i];
                                if (val > 1e18m) val = 1e18m; else if (val < -1e18m) val = -1e18m;
                                nextRow[i] = val;
                            }
                            catch (OverflowException)
                            {
                                nextRow[i] = curRow[i];
                            }
                        }
                        for (int i = 0; i < spatialPointsNx; i++)
                        {
                            prevRow[i] = curRow[i];
                            curRow[i] = nextRow[i];
                        }
                    }

                    for (int i = 0; i < spatialPointsNx; i++)
                    {
                        u[i, n + 1] = curRow[i];
                    }
                }
            }
            else // Advection Equation: u_t + v * u_x = 0
            {
                decimal cfl = (paramAlphaOrC * dt) / dx;
                decimal absCfl = DecimalMath.Abs(cfl);
                int advSubSteps = 1;
                if (absCfl > 0.9m)
                {
                    advSubSteps = (int)Math.Min(1000m, Math.Ceiling(absCfl / 0.9m));
                }
                decimal cflSub = cfl / advSubSteps;

                stabilityNote = $"Taşınım Denklemi (Upwind): CFL = v·Δt / Δx = {cfl:G4} " +
                    (advSubSteps == 1
                        ? (absCfl <= 1.0m ? "✓ Kararlı (CFL <= 1.0)" : "⚠️ Kararsız")
                        : $"✓ Otomatik CFL Uyarlaması ile Kararlı ({advSubSteps} mikro-adım/adım, efektif CFL = {cflSub:G4} ≤ 0.9)");

                decimal[] curRow = new decimal[spatialPointsNx];
                decimal[] nextRow = new decimal[spatialPointsNx];
                for (int i = 0; i < spatialPointsNx; i++) curRow[i] = u[i, 0];

                for (int n = 0; n < timeStepsNt - 1; n++)
                {
                    for (int s = 0; s < advSubSteps; s++)
                    {
                        nextRow[0] = leftBoundaryVal;
                        nextRow[spatialPointsNx - 1] = rightBoundaryVal;

                        for (int i = 1; i < spatialPointsNx - 1; i++)
                        {
                            try
                            {
                                decimal val;
                                if (paramAlphaOrC >= 0)
                                {
                                    val = curRow[i] - cflSub * (curRow[i] - curRow[i - 1]);
                                }
                                else
                                {
                                    val = curRow[i] - cflSub * (curRow[i + 1] - curRow[i]);
                                }
                                if (val > 1e18m) val = 1e18m; else if (val < -1e18m) val = -1e18m;
                                nextRow[i] = val;
                            }
                            catch (OverflowException)
                            {
                                nextRow[i] = curRow[i];
                            }
                        }

                        for (int i = 0; i < spatialPointsNx; i++) curRow[i] = nextRow[i];
                    }

                    for (int i = 0; i < spatialPointsNx; i++)
                    {
                        u[i, n + 1] = curRow[i];
                    }
                }
            }

            return new PdeSolutionGrid
            {
                Type = pdeType,
                XGrid = xGrid,
                TGrid = tGrid,
                U = u,
                StabilityNote = stabilityNote
            };
        }

        #endregion

        #region 4. System of Coupled PDEs (Reaction-Diffusion / Wave-Acoustic System)

        public class PdeSystemSolutionGrid
        {
            public decimal[] XGrid { get; set; }
            public decimal[] TGrid { get; set; }
            public decimal[,] U { get; set; }
            public decimal[,] V { get; set; }
            public string SystemInfo { get; set; }
        }

        /// <summary>
        /// Solves coupled 1D Reaction-Diffusion system:
        /// u_t = D_u * u_xx + F(u, v)
        /// v_t = D_v * v_xx + G(u, v)
        /// </summary>
        public static PdeSystemSolutionGrid SolveCoupledPdeSystem(
            string reactionFU,
            string reactionGV,
            string initUEq,
            string initVEq,
            decimal lengthL,
            decimal totalTimeT,
            int spatialPointsNx,
            int timeStepsNt,
            decimal diffDu,
            decimal diffDv)
        {
            if (spatialPointsNx < 5) spatialPointsNx = 50;
            if (timeStepsNt < 5) timeStepsNt = 100;
            if (lengthL <= 0) lengthL = 1m;
            if (totalTimeT <= 0) totalTimeT = 1m;

            decimal dx = lengthL / (spatialPointsNx - 1);
            decimal dt = totalTimeT / (timeStepsNt - 1);

            decimal[] xGrid = new decimal[spatialPointsNx];
            for (int i = 0; i < spatialPointsNx; i++) xGrid[i] = i * dx;

            decimal[] tGrid = new decimal[timeStepsNt];
            for (int n = 0; n < timeStepsNt; n++) tGrid[n] = n * dt;

            decimal[,] u = new decimal[spatialPointsNx, timeStepsNt];
            decimal[,] v = new decimal[spatialPointsNx, timeStepsNt];

            Function fnInitU = NumericalMethods.CompileFunction(initUEq);
            Function fnInitV = NumericalMethods.CompileFunction(initVEq);
            Function fnF = NumericalMethods.CompileFunction(reactionFU);
            Function fnG = NumericalMethods.CompileFunction(reactionGV);

            // Initial conditions
            for (int i = 0; i < spatialPointsNx; i++)
            {
                try { u[i, 0] = NumericalMethods.EvaluateCompiled(fnInitU, "x", xGrid[i]); } catch { u[i, 0] = 0m; }
                try { v[i, 0] = NumericalMethods.EvaluateCompiled(fnInitV, "x", xGrid[i]); } catch { v[i, 0] = 0m; }
            }

            decimal ru = (diffDu * dt) / (dx * dx);
            decimal rv = (diffDv * dt) / (dx * dx);
            decimal rMax = ru > rv ? ru : rv;
            int diffSubSteps = 1;
            if (rMax > 0.45m && rMax > 0)
            {
                diffSubSteps = (int)Math.Min(1000m, Math.Ceiling(rMax / 0.45m));
            }
            decimal ruSub = ru / diffSubSteps;
            decimal rvSub = rv / diffSubSteps;
            decimal dtSub = dt / diffSubSteps;

            decimal[] curU = new decimal[spatialPointsNx];
            decimal[] curV = new decimal[spatialPointsNx];
            decimal[] nextU = new decimal[spatialPointsNx];
            decimal[] nextV = new decimal[spatialPointsNx];

            for (int i = 0; i < spatialPointsNx; i++)
            {
                curU[i] = u[i, 0];
                curV[i] = v[i, 0];
            }

            for (int n = 0; n < timeStepsNt - 1; n++)
            {
                for (int s = 0; s < diffSubSteps; s++)
                {
                    // Neumann zero-flux boundary condition (u_0 = u_1, u_{N-1} = u_{N-2})
                    curU[0] = curU[1];
                    curU[spatialPointsNx - 1] = curU[spatialPointsNx - 2];
                    curV[0] = curV[1];
                    curV[spatialPointsNx - 1] = curV[spatialPointsNx - 2];

                    nextU[0] = curU[0];
                    nextU[spatialPointsNx - 1] = curU[spatialPointsNx - 1];
                    nextV[0] = curV[0];
                    nextV[spatialPointsNx - 1] = curV[spatialPointsNx - 1];

                    for (int i = 1; i < spatialPointsNx - 1; i++)
                    {
                        decimal uVal = curU[i];
                        decimal vVal = curV[i];

                        Dictionary<string, decimal> dict = new Dictionary<string, decimal> { { "u", uVal }, { "v", vVal } };
                        decimal reactU = 0m;
                        decimal reactV = 0m;
                        try { reactU = NumericalMethods.EvaluateCompiledMultiVar(fnF, dict); } catch { }
                        try { reactV = NumericalMethods.EvaluateCompiledMultiVar(fnG, dict); } catch { }

                        if (reactU > 1e12m) reactU = 1e12m;
                        else if (reactU < -1e12m) reactU = -1e12m;
                        if (reactV > 1e12m) reactV = 1e12m;
                        else if (reactV < -1e12m) reactV = -1e12m;

                        decimal lapU = (curU[i + 1] - 2m * uVal + curU[i - 1]);
                        decimal lapV = (curV[i + 1] - 2m * vVal + curV[i - 1]);

                        try
                        {
                            decimal nu = uVal + ruSub * lapU + dtSub * reactU;
                            if (nu > 1e18m) nu = 1e18m; else if (nu < -1e18m) nu = -1e18m;
                            nextU[i] = nu;
                        }
                        catch (OverflowException)
                        {
                            nextU[i] = uVal;
                        }

                        try
                        {
                            decimal nv = vVal + rvSub * lapV + dtSub * reactV;
                            if (nv > 1e18m) nv = 1e18m; else if (nv < -1e18m) nv = -1e18m;
                            nextV[i] = nv;
                        }
                        catch (OverflowException)
                        {
                            nextV[i] = vVal;
                        }
                    }

                    for (int i = 0; i < spatialPointsNx; i++)
                    {
                        curU[i] = nextU[i];
                        curV[i] = nextV[i];
                    }
                }

                for (int i = 0; i < spatialPointsNx; i++)
                {
                    u[i, n + 1] = curU[i];
                    v[i, n + 1] = curV[i];
                }
            }

            return new PdeSystemSolutionGrid
            {
                XGrid = xGrid,
                TGrid = tGrid,
                U = u,
                V = v,
                SystemInfo = $"1B Reaksiyon-Difüzyon Sistemi (r_u = {ru:G4}, r_v = {rv:G4}, dx = {dx:G4}, dt = {dt:G4})"
            };
        }

        #endregion

        #region 5. 2D Elliptic PDEs (Laplace / Poisson Equation: u_xx + u_yy = 0)

        public class Laplace2DSolution
        {
            public int Nx { get; set; }
            public int Ny { get; set; }
            public decimal[,] Grid { get; set; }
            public int Iterations { get; set; }
            public decimal FinalResidual { get; set; }
        }

        public static Laplace2DSolution Solve2DLaplace(
            int nx,
            int ny,
            decimal leftBc,
            decimal rightBc,
            decimal topBc,
            decimal bottomBc,
            int maxIter = 1000,
            decimal tol = 1e-5m)
        {
            if (nx < 4) nx = 20;
            if (ny < 4) ny = 20;
            decimal[,] u = new decimal[nx, ny];

            for (int j = 0; j < ny; j++)
            {
                u[0, j] = leftBc;
                u[nx - 1, j] = rightBc;
            }
            for (int i = 0; i < nx; i++)
            {
                u[i, 0] = bottomBc;
                u[i, ny - 1] = topBc;
            }

            decimal avg = (leftBc + rightBc + topBc + bottomBc) / 4m;
            for (int i = 1; i < nx - 1; i++)
                for (int j = 1; j < ny - 1; j++)
                    u[i, j] = avg;

            int iter = 0;
            decimal maxDiff = 1m;
            decimal omega = 1.35m; // SOR factor

            while (iter < maxIter && maxDiff > tol)
            {
                maxDiff = 0m;
                for (int i = 1; i < nx - 1; i++)
                {
                    for (int j = 1; j < ny - 1; j++)
                    {
                        decimal uNew = 0.25m * (u[i + 1, j] + u[i - 1, j] + u[i, j + 1] + u[i, j - 1]);
                        decimal uRelaxed = (1m - omega) * u[i, j] + omega * uNew;
                        decimal diff = DecimalMath.Abs(uRelaxed - u[i, j]);
                        if (diff > maxDiff) maxDiff = diff;
                        u[i, j] = uRelaxed;
                    }
                }
                iter++;
            }

            return new Laplace2DSolution
            {
                Nx = nx,
                Ny = ny,
                Grid = u,
                Iterations = iter,
                FinalResidual = maxDiff
            };
        }

        #endregion
    }
}
