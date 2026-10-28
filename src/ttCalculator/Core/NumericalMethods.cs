using DotNetMatrix;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class NumericalMethods
{
    public static string NormalizeEquation(string eq)
    {
        if (string.IsNullOrWhiteSpace(eq)) return "";
        eq = eq.Trim();
        if (eq.Contains("="))
        {
            string[] parts = eq.Split('=');
            if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                return "(" + parts[0] + ")-(" + parts[1] + ")";
            }
        }
        return eq;
    }

    public static Function CompileFunction(string eq)
    {
        string norm = NormalizeEquation(eq);
        Function f = new Function();
        f.Parse(norm);
        if (f.Error)
        {
            throw new ArgumentException("Denklem ayrıştırma hatası: " + f.ErrorDescription);
        }
        f.Infix2Postfix();
        return f;
    }

    public static decimal EvaluateCompiled(Function f, string variable, decimal point)
    {
        f.SetVariable(variable, point);
        f.EvaluatePostfix();
        if (f.Error)
        {
            throw new ArithmeticException(f.ErrorDescription);
        }
        return f.Result;
    }

    public static decimal EvaluateCompiledMultiVar(Function f, Dictionary<string, decimal> points)
    {
        f.SetVariables(points);
        f.EvaluatePostfix();
        if (f.Error)
        {
            throw new ArithmeticException(f.ErrorDescription);
        }
        return f.Result;
    }

    public static decimal Method_EvaluateFunction(string eq, string variable, decimal point)
    {
        Function f = CompileFunction(eq);
        return EvaluateCompiled(f, variable, point);
    }

    public static decimal MethodMultiVar_EvaluateFunction(string eq, Dictionary<string, decimal> points)
    {
        Function f = CompileFunction(eq);
        return EvaluateCompiledMultiVar(f, points);
    }

    /// <summary>
    /// Solves a nonlinear or linear system of equations using Multivariable Newton-Raphson with analytical/numerical Jacobian
    /// and returns step-by-step convergence logs as well as the final solution dictionary.
    /// </summary>
    public static Dictionary<string, decimal> MethodMultiVar_SolveEquationSystem(
        List<string> eqs,
        Dictionary<string, decimal> initPoints,
        int iterationCount = 50,
        decimal h = 0.0001m)
    {
        var logs = new List<string>();
        return SolveEquationSystemWithLogs(eqs, initPoints, iterationCount, out logs, h);
    }

    public static Dictionary<string, decimal> SolveEquationSystemWithLogs(
        List<string> eqs,
        Dictionary<string, decimal> initPoints,
        int iterationCount,
        out List<string> logs,
        decimal h = 0.0001m)
    {
        logs = new List<string>();
        if (eqs == null || initPoints == null || eqs.Count == 0 || eqs.Count != initPoints.Count)
        {
            throw new ArgumentException("Denklem sayısı ile değişken sayısı birbirine eşit olmalıdır.");
        }

        int n = eqs.Count;
        List<string> vars = initPoints.Keys.ToList();
        Dictionary<string, decimal> currentPoints = new Dictionary<string, decimal>(initPoints);

        List<Function> compiledEqs = new List<Function>(n);
        for (int i = 0; i < n; i++)
        {
            compiledEqs.Add(CompileFunction(eqs[i]));
        }

        decimal tolerance = 0.0000000001m; // 1e-10m

        for (int iter = 1; iter <= iterationCount; iter++)
        {
            decimal[] F = new decimal[n];
            decimal maxAbsF = 0m;

            for (int i = 0; i < n; i++)
            {
                F[i] = EvaluateCompiledMultiVar(compiledEqs[i], currentPoints);
                decimal absF = DecimalMath.Abs(F[i]);
                if (absF > maxAbsF) maxAbsF = absF;
            }

            if (maxAbsF <= tolerance)
            {
                logs.Add($"[Adım {iter - 1}] Yakınsama sağlandı! Maksimum hata |F| = {maxAbsF:E4}");
                break;
            }

            // Compute Jacobian Matrix J[i][j] = df_i / dx_j via central finite difference
            decimal[][] J = new decimal[n][];
            for (int i = 0; i < n; i++)
            {
                J[i] = new decimal[n];
                for (int j = 0; j < n; j++)
                {
                    string vName = vars[j];
                    decimal origVal = currentPoints[vName];

                    currentPoints[vName] = origVal + h;
                    decimal fPlus = EvaluateCompiledMultiVar(compiledEqs[i], currentPoints);

                    currentPoints[vName] = origVal - h;
                    decimal fMinus = EvaluateCompiledMultiVar(compiledEqs[i], currentPoints);

                    currentPoints[vName] = origVal;
                    J[i][j] = (fPlus - fMinus) / (2m * h);
                }
            }

            decimal[] negF = new decimal[n];
            for (int i = 0; i < n; i++) negF[i] = -F[i];

            decimal[] delta;
            try
            {
                delta = Matrix.SolveEquation(J, negF);
            }
            catch
            {
                // Perturb slightly if Jacobian is singular at initial guess (e.g. (0,0) for x^2+y^2)
                for (int j = 0; j < n; j++)
                {
                    currentPoints[vars[j]] += 0.01m * (j + 1);
                }
                logs.Add($"[Adım {iter}] Jakobiyen matrisi tekil (singular) noktaya denk geldi, nokta hafifçe kaydırılıyor...");
                continue;
            }

            decimal maxStep = 0m;
            for (int j = 0; j < n; j++)
            {
                currentPoints[vars[j]] += delta[j];
                decimal absD = DecimalMath.Abs(delta[j]);
                if (absD > maxStep) maxStep = absD;
            }

            string ptSummary = string.Join(", ", vars.Select(v => $"{v} = {currentPoints[v].ToString("0.########", CultureInfo.InvariantCulture)}"));
            logs.Add($"[Adım {iter}] {ptSummary}   (|Δ| = {maxStep:E3})");

            if (maxStep <= tolerance)
            {
                logs.Add($"[Adım {iter}] Çözüm başarıyla yakınsadı.");
                break;
            }
        }

        return currentPoints;
    }

    public static string Method_TaylorSeries(string eq, int degree, string variable, decimal point, decimal h = 0.005m)
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        decimal constant = Method_EvaluateFunction(eq, variable, point);
        if (DecimalMath.Abs(constant) <= 0.000001m) constant = 0;

        List<string> terms = new List<string>();
        if (constant != 0)
        {
            terms.Add(FormatCoeff(constant));
        }

        for (int i = 1; i <= degree; i++)
        {
            decimal step = i >= 4 ? 0.01m : h;
            decimal der = Method_NumericalDifferentiation(eq, i, variable, point, 4, step);
            decimal fact = Factorial(i);
            decimal coeff = der / fact;

            if (DecimalMath.Abs(coeff) <= 0.000001m) continue;

            string varPart = FormatVarPower(variable, point, i);
            string coeffStr = FormatTermWithCoeff(coeff, varPart, terms.Count == 0);
            terms.Add(coeffStr);
        }

        if (terms.Count == 0) return "0";
        return string.Join(" ", terms);
    }

    public static string MethodMultiVar_TaylorSeries(string eq, int degree, Dictionary<string, decimal> points, decimal h = 0.005m)
    {
        if (points == null || points.Count == 0) return "0";

        Function compiled = CompileFunction(eq);
        decimal constant = EvaluateCompiledMultiVar(compiled, points);
        if (DecimalMath.Abs(constant) <= 0.00001m) constant = 0;

        List<string> terms = new List<string>();
        if (constant != 0)
        {
            terms.Add(FormatCoeff(constant));
        }

        List<string> varNames = points.Keys.ToList();
        int kVars = varNames.Count;

        for (int totalDeg = 1; totalDeg <= degree; totalDeg++)
        {
            List<int[]> multiIndices = GenerateMultiIndices(kVars, totalDeg);
            foreach (int[] alpha in multiIndices)
            {
                decimal mixedDer = ComputeMixedPartialDerivative(compiled, varNames, alpha, points, h);
                decimal denom = 1m;
                for (int j = 0; j < kVars; j++)
                {
                    denom *= Factorial(alpha[j]);
                }

                decimal coeff = mixedDer / denom;
                if (DecimalMath.Abs(coeff) <= 0.00001m) continue;

                List<string> factors = new List<string>();
                for (int j = 0; j < kVars; j++)
                {
                    if (alpha[j] > 0)
                    {
                        string vName = varNames[j];
                        factors.Add(FormatVarPower(vName, points[vName], alpha[j]));
                    }
                }

                string varPart = string.Join(" * ", factors);
                terms.Add(FormatTermWithCoeff(coeff, varPart, terms.Count == 0));
            }
        }

        if (terms.Count == 0) return "0";
        return string.Join(" ", terms);
    }

    public static (string Fractional, string Factorial, string Decimal, List<string> Details) MethodMultiVar_TaylorSeriesDetailed(
        string eq, int degree, Dictionary<string, decimal> points, decimal h = 0.005m)
    {
        if (points == null || points.Count == 0) return ("0", "0", "0", new List<string>());

        Function compiled = CompileFunction(eq);
        decimal constant = EvaluateCompiledMultiVar(compiled, points);
        if (DecimalMath.Abs(constant) <= 0.00001m) constant = 0m;

        List<string> fracTerms = new List<string>();
        List<string> factTerms = new List<string>();
        List<string> decTerms = new List<string>();
        List<string> details = new List<string>();

        string constFrac = FormatFractionalCoeff(constant);
        details.Add($"f(merkez) = {constFrac}  ({Math.Round(constant, 6).ToString("0.######", CultureInfo.InvariantCulture)})");

        if (constant != 0m)
        {
            fracTerms.Add(constFrac);
            factTerms.Add(constFrac);
            decTerms.Add(Math.Round(constant, 6).ToString("0.######", CultureInfo.InvariantCulture));
        }

        List<string> varNames = points.Keys.ToList();
        int kVars = varNames.Count;

        for (int totalDeg = 1; totalDeg <= degree; totalDeg++)
        {
            List<int[]> multiIndices = GenerateMultiIndices(kVars, totalDeg);
            foreach (int[] alpha in multiIndices)
            {
                decimal mixedDer = ComputeMixedPartialDerivative(compiled, varNames, alpha, points, h);
                decimal denom = 1m;
                List<string> factDenomParts = new List<string>();
                for (int j = 0; j < kVars; j++)
                {
                    denom *= Factorial(alpha[j]);
                    if (alpha[j] > 1) factDenomParts.Add($"{alpha[j]}!");
                }

                decimal coeff = mixedDer / denom;
                if (DecimalMath.Abs(coeff) <= 0.00001m) continue;

                List<string> factors = new List<string>();
                List<string> idxNames = new List<string>();
                for (int j = 0; j < kVars; j++)
                {
                    if (alpha[j] > 0)
                    {
                        string vName = varNames[j];
                        factors.Add(FormatVarPower(vName, points[vName], alpha[j]));
                        idxNames.Add($"{vName}^{alpha[j]}");
                    }
                }

                string varPart = string.Join(" * ", factors);

                // 1. Fractional
                var frac = ToFraction(coeff);
                string termFrac;
                if (frac.HasValue)
                {
                    long num = frac.Value.Num;
                    long den = frac.Value.Denom;
                    string sign = num < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                    long absNum = Math.Abs(num);
                    if (den == 1)
                        termFrac = absNum == 1 ? $"{sign}{varPart}" : $"{sign}{absNum}*{varPart}";
                    else
                        termFrac = absNum == 1 ? $"{sign}(1/{den})*{varPart}" : $"{sign}({absNum}/{den})*{varPart}";
                }
                else
                {
                    string sign = coeff < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                    termFrac = $"{sign}{Math.Round(DecimalMath.Abs(coeff), 5).ToString("0.#####", CultureInfo.InvariantCulture)}*{varPart}";
                }
                fracTerms.Add(termFrac);

                // 2. Factorial
                string factDenomStr = factDenomParts.Count == 0 ? $"{totalDeg}!" : string.Join("·", factDenomParts);
                var derFrac = ToFraction(mixedDer);
                string factSign = mixedDer < 0 ? (factTerms.Count == 0 ? "-" : "- ") : (factTerms.Count == 0 ? "" : "+ ");
                decimal absDer = DecimalMath.Abs(mixedDer);
                string termFact;
                if (derFrac.HasValue && derFrac.Value.Denom == 1)
                {
                    long dVal = Math.Abs(derFrac.Value.Num);
                    termFact = dVal == 1 ? $"{factSign}(1/{factDenomStr})*{varPart}" : $"{factSign}({dVal}/{factDenomStr})*{varPart}";
                }
                else
                {
                    termFact = $"{factSign}({Math.Round(absDer, 4).ToString("0.####", CultureInfo.InvariantCulture)}/{factDenomStr})*{varPart}";
                }
                factTerms.Add(termFact);

                // 3. Decimal
                decTerms.Add(FormatTermWithCoeff(coeff, varPart, decTerms.Count == 0));

                string derName = "∂^" + totalDeg + "f / " + string.Join("", idxNames.Select(s => "∂" + s));
                string coeffFracStr = frac.HasValue ? (frac.Value.Denom == 1 ? $"{frac.Value.Num}" : $"{frac.Value.Num}/{frac.Value.Denom}") : Math.Round(coeff, 5).ToString("0.#####", CultureInfo.InvariantCulture);
                details.Add($"• {derName,-28} = {Math.Round(mixedDer, 5).ToString("0.#####", CultureInfo.InvariantCulture),-10} | Payda: {denom,-5} => Kesirli Katsayı: {coeffFracStr,-10} | Ondalık: {Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture)}");
            }
        }

        string finalFrac = fracTerms.Count == 0 ? "0" : string.Join(" ", fracTerms);
        string finalFact = factTerms.Count == 0 ? "0" : string.Join(" ", factTerms);
        string finalDec = decTerms.Count == 0 ? "0" : string.Join(" ", decTerms);

        return (finalFrac, finalFact, finalDec, details);
    }

    private static List<int[]> GenerateMultiIndices(int numVars, int totalSum)
    {
        List<int[]> results = new List<int[]>();
        int[] current = new int[numVars];
        GenerateMultiIndicesRecursive(numVars, totalSum, 0, current, results);
        return results;
    }

    private static void GenerateMultiIndicesRecursive(int numVars, int remaining, int idx, int[] current, List<int[]> results)
    {
        if (idx == numVars - 1)
        {
            current[idx] = remaining;
            results.Add((int[])current.Clone());
            return;
        }
        for (int v = remaining; v >= 0; v--)
        {
            current[idx] = v;
            GenerateMultiIndicesRecursive(numVars, remaining - v, idx + 1, current, results);
        }
    }

    private static decimal ComputeMixedPartialDerivative(
        Function f,
        List<string> varNames,
        int[] orders,
        Dictionary<string, decimal> basePoints,
        decimal h)
    {
        Dictionary<string, decimal> workPoints = new Dictionary<string, decimal>(basePoints);
        return ComputeMixedRecursive(f, varNames, orders, 0, workPoints, h);
    }

    private static decimal ComputeMixedRecursive(
        Function f,
        List<string> varNames,
        int[] orders,
        int varIndex,
        Dictionary<string, decimal> workPoints,
        decimal h)
    {
        while (varIndex < orders.Length && orders[varIndex] == 0)
        {
            varIndex++;
        }

        if (varIndex >= orders.Length)
        {
            return EvaluateCompiledMultiVar(f, workPoints);
        }

        string vName = varNames[varIndex];
        int m = orders[varIndex];
        decimal origVal = workPoints[vName];

        // Central finite difference weights for order m
        decimal sum = 0m;
        for (int i = 0; i <= m; i++)
        {
            decimal shift = (m - 2m * i) * (h * 0.5m);
            workPoints[vName] = origVal + shift;
            decimal sign = (i % 2 == 0) ? 1m : -1m;
            decimal comb = Combination(m, i);
            decimal subVal = ComputeMixedRecursive(f, varNames, orders, varIndex + 1, workPoints, h);
            sum += sign * comb * subVal;
        }

        workPoints[vName] = origVal;
        return sum / DecimalMath.Pow(h, m);
    }

    private static string FormatCoeff(decimal c)
    {
        decimal rounded = Math.Round(c, 6);
        foreach (int denom in new[] { 1, 2, 3, 4, 6, 8, 12 })
        {
            decimal candidate = Math.Round(c * denom) / (decimal)denom;
            if (DecimalMath.Abs(c - candidate) < 0.00005m)
            {
                rounded = candidate;
                break;
            }
        }
        return rounded.ToString("0.######", CultureInfo.InvariantCulture);
    }

    private static string FormatVarPower(string variable, decimal center, int power)
    {
        string core;
        if (center == 0m)
        {
            core = variable;
        }
        else if (center > 0m)
        {
            core = $"({variable} - {FormatCoeff(center)})";
        }
        else
        {
            core = $"({variable} + {FormatCoeff(DecimalMath.Abs(center))})";
        }

        if (power == 1) return core;
        return $"{core}^{power}";
    }

    private static string FormatTermWithCoeff(decimal coeff, string varPart, bool isFirst)
    {
        decimal absC = DecimalMath.Abs(coeff);
        string sign = coeff < 0 ? (isFirst ? "-" : "- ") : (isFirst ? "" : "+ ");
        string cStr = FormatCoeff(absC);

        if (cStr == "1")
        {
            return $"{sign}{varPart}";
        }
        return $"{sign}{cStr}*{varPart}";
    }

    public static (long Num, long Denom)? ToFraction(decimal value, decimal tolerance = 0.0001m, int maxDenom = 50000)
    {
        if (value == 0m) return (0, 1);
        long sign = value < 0 ? -1 : 1;
        decimal val = DecimalMath.Abs(value);

        long intPart = (long)Math.Round(val);
        if (DecimalMath.Abs(val - intPart) <= tolerance)
        {
            return (sign * intPart, 1);
        }

        long h1 = 1, h2 = 0;
        long k1 = 0, k2 = 1;
        decimal b = val;

        for (int iter = 0; iter < 15; iter++)
        {
            long a = (long)Math.Floor(b);
            long auxH = a * h1 + h2;
            long auxK = a * k1 + k2;

            if (auxK > maxDenom || auxK <= 0) break;

            h2 = h1; h1 = auxH;
            k2 = k1; k1 = auxK;

            decimal fractionVal = (decimal)h1 / (decimal)k1;
            if (DecimalMath.Abs(val - fractionVal) <= tolerance)
            {
                return (sign * h1, k1);
            }

            decimal diff = b - a;
            if (DecimalMath.Abs(diff) < 1e-12m) break;
            b = 1m / diff;
        }

        for (int d = 2; d <= 240; d++)
        {
            decimal num = Math.Round(val * d);
            if (DecimalMath.Abs(val - (num / d)) <= tolerance)
            {
                long n = (long)num;
                long g = Gcd(n, d);
                return (sign * (n / g), d / g);
            }
        }

        return null;
    }

    public static string FormatFractionalCoeff(decimal coeff)
    {
        var frac = ToFraction(coeff);
        if (frac.HasValue)
        {
            if (frac.Value.Denom == 1) return frac.Value.Num.ToString();
            return $"{frac.Value.Num}/{frac.Value.Denom}";
        }
        return Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture);
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long t = b;
            b = a % b;
            a = t;
        }
        return Math.Abs(a);
    }

    public static decimal Method_NewtonRaphson(string eq, string variable, decimal initialValue, int iterationCount, decimal h = 0.0001m)
    {
        Function f = CompileFunction(eq);
        decimal curr = initialValue;
        for (int i = 0; i < iterationCount; i++)
        {
            decimal fx = EvaluateCompiled(f, variable, curr);
            if (DecimalMath.Abs(fx) <= 1e-12m) break;

            decimal dfx = (EvaluateCompiled(f, variable, curr + h) - EvaluateCompiled(f, variable, curr - h)) / (2m * h);
            if (DecimalMath.Abs(dfx) <= 1e-15m) break;

            curr = curr - fx / dfx;
        }
        return curr;
    }

    public static decimal Method_BisectionMethod(string eq, string variable, decimal lowerValue, decimal higherValue, int iterationCount)
    {
        if (lowerValue >= higherValue) return lowerValue;

        Function f = CompileFunction(eq);
        decimal fa = EvaluateCompiled(f, variable, lowerValue);
        if (DecimalMath.Abs(fa) <= 1e-12m) return lowerValue;
        decimal fb = EvaluateCompiled(f, variable, higherValue);
        if (DecimalMath.Abs(fb) <= 1e-12m) return higherValue;

        decimal mp = (lowerValue + higherValue) / 2m;

        for (int i = 0; i < iterationCount; i++)
        {
            mp = (lowerValue + higherValue) / 2m;
            decimal evMiddle = EvaluateCompiled(f, variable, mp);

            if (DecimalMath.Abs(evMiddle) <= 1e-12m) break;

            if (fa * evMiddle <= 0)
            {
                higherValue = mp;
                fb = evMiddle;
            }
            else
            {
                lowerValue = mp;
                fa = evMiddle;
            }
        }

        return mp;
    }

    public static decimal Method_Derivative(string eq, int degree, string variable, decimal point, decimal h = 0.001m)
    {
        return Method_NumericalDifferentiation(eq, degree, variable, point, 4, h);
    }

    public static decimal Method_PartialDerivative(string eq, int degree, string toVar, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        return Method_NumericalPartialDifferentiation(eq, degree, toVar, points, 4, h);
    }

    public static decimal Method_NumericalDifferentiation(string eq, int degree, string variable, decimal point, int accuracy = 2, decimal h = 0.001m)
    {
        int m = degree, n = accuracy;
        if (m >= 11) { n = 2; h = 0.08m; }
        else if (m == 10) { n = 2; h = 0.04m; }
        else if (m == 9) { n = 4; h = 0.015m; }
        else if (m >= 7 && h < 0.012m) h = 0.012m;

        Function f = CompileFunction(eq);
        int p = (2 * (int)Math.Floor((m + 1) / 2.0) - 2 + n) / 2;

        decimal[][] _matrix = new decimal[2 * p + 1][];
        decimal[] _rhsVector = new decimal[2 * p + 1], functionPointsX = new decimal[2 * p + 1],
            functionPointsY = new decimal[2 * p + 1];

        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = new decimal[2 * p + 1];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                _matrix[i][j] = DecimalMath.PowerN(-p + j, i);
            }

            _rhsVector[i] = (i == m) ? Factorial(m) : 0m;
            functionPointsX[i] = point - (p - i) * h;
            functionPointsY[i] = EvaluateCompiled(f, variable, functionPointsX[i]);
        }

        GeneralMatrix _A = new GeneralMatrix(_matrix);
        GeneralMatrix _b = new GeneralMatrix(_rhsVector.Length, 1);

        for (int i = 0; i < _rhsVector.Length; i++)
        {
            _b.Array[i][0] = _rhsVector[i];
        }

        GeneralMatrix coeffs = _A.Solve(_b);
        decimal sum = 0;

        for (int i = 0; i < coeffs.Array.Length; i++)
        {
            sum += coeffs.Array[i][0] * functionPointsY[i];
        }

        return sum / DecimalMath.Pow(h, degree);
    }

    public static decimal Method_NumericalPartialDifferentiation(string eq, int degree, string toVar, Dictionary<string, decimal> points, int accuracy = 2, decimal h = 0.001m)
    {
        Function f = CompileFunction(eq);
        int m = degree, n = accuracy;
        int p = (2 * (int)Math.Floor((m + 1) / 2.0) - 2 + n) / 2;

        decimal[][] _matrix = new decimal[2 * p + 1][];
        Dictionary<string, decimal[]> functionPointsDomain = new Dictionary<string, decimal[]>();
        decimal[] _rhsVector = new decimal[2 * p + 1], functionPointsRange = new decimal[2 * p + 1];

        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = new decimal[2 * p + 1];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                _matrix[i][j] = DecimalMath.PowerN(-p + j, i);
            }

            _rhsVector[i] = (i == m) ? Factorial(m) : 0m;
        }

        foreach (KeyValuePair<string, decimal> item in points)
        {
            decimal[] ps = new decimal[2 * p + 1];
            if (item.Key == toVar)
            {
                for (int i = 0; i < ps.Length; i++) ps[i] = points[item.Key] - (p - i) * h;
            }
            else
            {
                for (int i = 0; i < ps.Length; i++) ps[i] = points[item.Key];
            }
            functionPointsDomain.Add(item.Key, ps);
        }

        for (int i = 0; i < 2 * p + 1; i++)
        {
            Dictionary<string, decimal> pair = new Dictionary<string, decimal>();
            foreach (KeyValuePair<string, decimal[]> item in functionPointsDomain)
            {
                pair.Add(item.Key, item.Value[i]);
            }
            functionPointsRange[i] = EvaluateCompiledMultiVar(f, pair);
        }

        GeneralMatrix _A = new GeneralMatrix(_matrix);
        GeneralMatrix _b = new GeneralMatrix(_rhsVector.Length, 1);

        for (int i = 0; i < _rhsVector.Length; i++) _b.Array[i][0] = _rhsVector[i];

        GeneralMatrix coeffs = _A.Solve(_b);
        decimal sum = 0;

        for (int i = 0; i < coeffs.Array.Length; i++)
        {
            sum += coeffs.Array[i][0] * functionPointsRange[i];
        }

        return sum / DecimalMath.Pow(h, degree);
    }

    public static decimal Factorial(int num)
    {
        if (num < 0) throw new ArgumentException("Faktöriyel negatif sayılar için tanımlı değildir.");
        if (num > 27) throw new OverflowException("Faktöriyel taşması: 27'den büyük değerler 128-bit Decimal sınırını aşar.");
        if (num == 0 || num == 1) return 1m;
        decimal res = 1m;
        for (int i = 2; i <= num; i++) res *= i;
        return res;
    }

    public static decimal Combination(int n, int k)
    {
        if (k < 0 || k > n || n < 0) return 0m;
        if (k == 0 || k == n) return 1m;
        if (k > n - k) k = n - k;
        decimal res = 1m;
        for (int i = 1; i <= k; i++)
        {
            res = (res * (n - k + i)) / i;
        }
        return res;
    }

    public static decimal Permutation(int n, int k)
    {
        if (k < 0 || k > n || n < 0) return 0m;
        if (k == 0) return 1m;
        decimal res = 1m;
        for (int i = 0; i < k; i++)
        {
            res *= (n - i);
        }
        return res;
    }

    public static string GenerateCombinations(string chars, string prefix, int length)
    {
        StringBuilder sb = new StringBuilder();
        GenerateCombinationsRecursive(prefix, chars, length, sb);
        return sb.ToString();
    }

    static void GenerateCombinationsRecursive(string prefix, string chars, int length, StringBuilder sb)
    {
        if (length == 0)
        {
            sb.AppendLine(prefix);
            return;
        }

        foreach (char c in chars)
        {
            GenerateCombinationsRecursive(prefix + c, chars, length - 1, sb);
        }
    }

    /// <summary>
    /// Gauss-Seidel Iterative Linear System Solver with Diagonal Dominance Verification & Row Reordering.
    /// Ported and enhanced from NumericalMethods / NumericalMethods2.
    /// </summary>
    public static Dictionary<string, decimal> Method_GaussSeidel(
        List<string> eqs,
        Dictionary<string, decimal> initPoints,
        int iterationCount,
        out List<string> logs)
    {
        logs = new List<string>();
        if (eqs == null || initPoints == null || eqs.Count == 0 || eqs.Count != initPoints.Count)
        {
            throw new ArgumentException("Denklem sayısı ile değişken sayısı birbirine eşit olmalıdır.");
        }

        int n = eqs.Count;
        List<string> vars = initPoints.Keys.ToList();

        // Extract linear matrix A and right-hand vector b such that A * x = b
        // Note: ConvertLinearEquationsToMatrix evaluates f(x) = LHS - RHS = A*x - b
        decimal[][] A = Matrix.ConvertLinearEquationsToMatrix(eqs, vars);
        decimal[] b = new decimal[n];
        Dictionary<string, decimal> zeroPts = new Dictionary<string, decimal>();
        foreach (string v in vars) zeroPts[v] = 0m;

        for (int i = 0; i < n; i++)
        {
            string normEq = NormalizeEquation(eqs[i]);
            // f(0) = -b  =>  b = -f(0)
            b[i] = -MethodMultiVar_EvaluateFunction(normEq, zeroPts);
        }

        // Check / reorder rows for diagonal dominance: |a_ii| > sum_{j != i} |a_ij|
        int[] dominantColForRow = new int[n];
        bool strictDominance = true;
        for (int i = 0; i < n; i++)
        {
            int domCol = -1;
            for (int j = 0; j < n; j++)
            {
                decimal sumOthers = 0m;
                for (int k = 0; k < n; k++)
                {
                    if (k != j) sumOthers += DecimalMath.Abs(A[i][k]);
                }
                if (DecimalMath.Abs(A[i][j]) >= sumOthers && DecimalMath.Abs(A[i][j]) > 0m)
                {
                    domCol = j;
                    break;
                }
            }
            dominantColForRow[i] = domCol;
            if (domCol == -1) strictDominance = false;
        }

        // Attempt to permute rows if each row has a distinct dominant column
        if (strictDominance && dominantColForRow.Distinct().Count() == n)
        {
            decimal[][] newA = new decimal[n][];
            decimal[] newB = new decimal[n];
            for (int i = 0; i < n; i++)
            {
                int targetRow = dominantColForRow[i];
                newA[targetRow] = A[i];
                newB[targetRow] = b[i];
            }
            A = newA;
            b = newB;
            logs.Add("✓ Köşegen baskınlığı (Diagonal Dominance) doğrulandı (|a_ii| ≥ Σ|a_ij|).");
        }
        else
        {
            logs.Add("⚠️ Uyarı: Matris tam köşegen baskın değil; Gauss-Seidel yakınsamayabilir.");
        }

        decimal[] x = new decimal[n];
        for (int i = 0; i < n; i++) x[i] = initPoints[vars[i]];

        decimal tol = 1e-10m;
        for (int iter = 1; iter <= iterationCount; iter++)
        {
            decimal maxDiff = 0m;
            for (int i = 0; i < n; i++)
            {
                if (DecimalMath.Abs(A[i][i]) <= 1e-15m)
                {
                    throw new ArithmeticException($"Köşegen elemanı A[{i + 1},{i + 1}] = 0 olduğu için Gauss-Seidel uygulanamaz.");
                }

                decimal sigma = 0m;
                for (int j = 0; j < n; j++)
                {
                    if (j != i) sigma += A[i][j] * x[j];
                }

                decimal nextXi = (b[i] - sigma) / A[i][i];
                decimal diff = DecimalMath.Abs(nextXi - x[i]);
                if (diff > maxDiff) maxDiff = diff;
                x[i] = nextXi;
            }

            string stepStr = string.Join(", ", Enumerable.Range(0, n).Select(idx => $"{vars[idx]} = {x[idx].ToString("0.########", CultureInfo.InvariantCulture)}"));
            logs.Add($"[Adım {iter,2}] {stepStr}   (|Δ| = {maxDiff:E3})");

            if (maxDiff <= tol)
            {
                logs.Add($"✓ Gauss-Seidel {iter}. adımda başarıyla yakınsadı.");
                break;
            }
        }

        Dictionary<string, decimal> result = new Dictionary<string, decimal>();
        for (int i = 0; i < n; i++) result[vars[i]] = x[i];
        return result;
    }

    public static Dictionary<string, decimal> Method_LinearMatrixSolve(
        List<string> eqs,
        List<string> vars,
        string methodType,
        out List<string> logs)
    {
        logs = new List<string>();
        int n = eqs.Count;
        decimal[][] A = Matrix.ConvertLinearEquationsToMatrix(eqs, vars);
        decimal[] b = new decimal[n];

        Dictionary<string, decimal> zeroPts = new Dictionary<string, decimal>();
        foreach (string v in vars) zeroPts[v] = 0m;
        bool isNonLinear = false;
        for (int i = 0; i < n; i++)
        {
            string normEq = NormalizeEquation(eqs[i]);
            b[i] = -MethodMultiVar_EvaluateFunction(normEq, zeroPts);

            if (vars.Count > 0)
            {
                zeroPts[vars[0]] = 2m;
                decimal f2 = MethodMultiVar_EvaluateFunction(normEq, zeroPts);
                zeroPts[vars[0]] = 0m;
                if (DecimalMath.Abs(f2 - (2m * A[i][0] - b[i])) > 1e-4m)
                {
                    isNonLinear = true;
                }
            }
        }

        if (isNonLinear)
        {
            logs.Add("UYARI: Denklemler doğrusal değil. Çözüm doğrusal yaklaşım olarak sunulmuştur.");
        }

        logs.Add("--- Katsayılar Matrisi (A) ve Sağ Taraf Vektörü (b) ---");
        for (int i = 0; i < n; i++)
        {
            string rowStr = string.Join("  ", A[i].Select(v => $"{v,8:0.####}"));
            logs.Add($"  [ {rowStr} ]  |  [ {b[i],8:0.####} ]");
        }
        logs.Add("");

        GeneralMatrix gA = new GeneralMatrix(A);
        GeneralMatrix gB = new GeneralMatrix(n, 1);
        for (int i = 0; i < n; i++) gB.Array[i][0] = b[i];

        GeneralMatrix gX;
        if (methodType == "INVERSE")
        {
            logs.Add("Yöntem: Ters Matris (X = A⁻¹ · b)");
            GeneralMatrix invA = gA.Inverse();
            gX = invA.Multiply(gB);
        }
        else if (methodType == "QR")
        {
            logs.Add("Yöntem: QR Ayrışımı (Q·R·X = b)");
            gX = gA.QRD().Solve(gB);
        }
        else
        {
            logs.Add("Yöntem: LU Ayrışımı (L·U·X = P·b)");
            gX = gA.LUD().Solve(gB);
        }

        Dictionary<string, decimal> res = new Dictionary<string, decimal>();
        for (int i = 0; i < n; i++)
        {
            res[vars[i]] = gX.Array[i][0];
            logs.Add($"  {vars[i]} = {Math.Round(gX.Array[i][0], 8).ToString("0.########", CultureInfo.InvariantCulture)}");
        }
        return res;
    }

    public static decimal Method_BackwardDifferenceDerivative(string eq, int degree, string variable, decimal point, decimal h = 0.001m)
    {
        Function f = CompileFunction(eq);
        decimal f0 = EvaluateCompiled(f, variable, point);
        decimal f1 = EvaluateCompiled(f, variable, point - h);
        decimal f2 = EvaluateCompiled(f, variable, point - 2m * h);

        if (degree == 1)
        {
            return (3m * f0 + f2 - 4m * f1) / (2m * h);
        }
        else if (degree == 2)
        {
            return (f0 + f2 - 2m * f1) / (h * h);
        }
        else if (degree == 3)
        {
            decimal f3 = EvaluateCompiled(f, variable, point - 3m * h);
            decimal f4 = EvaluateCompiled(f, variable, point - 4m * h);
            return (3m * f4 - 14m * f3 + 24m * f2 - 18m * f1 + 5m * f0) / (2m * h * h * h);
        }
        else if (degree == 4)
        {
            decimal f3 = EvaluateCompiled(f, variable, point - 3m * h);
            decimal f4 = EvaluateCompiled(f, variable, point - 4m * h);
            decimal f5 = EvaluateCompiled(f, variable, point - 5m * h);
            return (-2m * f5 + 11m * f4 - 24m * f3 + 26m * f2 - 14m * f1 + 3m * f0) / (h * h * h * h);
        }
        else
        {
            return Method_ForwardBinomialDerivative(eq, degree, variable, point, h);
        }
    }

    public static decimal Method_ForwardBinomialDerivative(string eq, int degree, string variable, decimal point, decimal h = 0.001m)
    {
        Function f = CompileFunction(eq);
        decimal sum = 0m;
        for (int i = 0; i <= degree; i++)
        {
            decimal sign = (i % 2 == 0) ? 1m : -1m;
            sum += sign * Combination(degree, i) * EvaluateCompiled(f, variable, point + (degree - i) * h);
        }
        return sum / DecimalMath.Pow(h, degree);
    }

    public static decimal Method_ForwardBinomialPartialDerivative(string eq, int degree, string toVar, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        Function f = CompileFunction(eq);
        decimal sum = 0m;
        Dictionary<string, decimal> testPS = new Dictionary<string, decimal>(points);
        decimal firstToVar = testPS[toVar];
        for (int i = 0; i <= degree; i++)
        {
            testPS[toVar] = firstToVar + (degree - i) * h;
            decimal sign = (i % 2 == 0) ? 1m : -1m;
            sum += sign * Combination(degree, i) * EvaluateCompiledMultiVar(f, testPS);
        }
        return sum / DecimalMath.Pow(h, degree);
    }

    // --- Root Finding Methods with Full Iteration Logs & Variable Support ---

    public static List<string> Method_NewtonRaphson(string _eq, decimal _initialPoint, int _iterationCount, string variable = "x")
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        Function f = CompileFunction(_eq);
        List<string> logs = new List<string>();
        decimal x = _initialPoint;
        decimal h = 0.0001m;
        decimal tol = 1e-12m;

        logs.Add($"--- Newton-Raphson Yöntemi (Başlangıç: {variable}₀ = {x.ToString(CultureInfo.InvariantCulture)}) ---");

        for (int i = 1; i <= _iterationCount; i++)
        {
            decimal fx = EvaluateCompiled(f, variable, x);
            decimal dfx = (EvaluateCompiled(f, variable, x + h) - EvaluateCompiled(f, variable, x - h)) / (2m * h);

            if (DecimalMath.Abs(dfx) <= 1e-15m)
            {
                logs.Add($"[Adım {i}] Türev sıfıra çok yakın (f'({variable}) ≈ 0). İşlem durduruldu.");
                break;
            }

            decimal nextX = x - fx / dfx;
            decimal err = DecimalMath.Abs(nextX - x);
            x = nextX;

            logs.Add($"[Adım {i,2}] {variable} = {x.ToString("0.############", CultureInfo.InvariantCulture),-18} | f({variable}) = {fx.ToString("E3", CultureInfo.InvariantCulture),-11} | |Δ| = {err.ToString("E3", CultureInfo.InvariantCulture)}");

            if (err <= tol || DecimalMath.Abs(fx) <= tol)
            {
                logs.Add($"✓ Yakınsama {i}. adımda tamamlandı.");
                break;
            }
        }

        decimal finalFx = EvaluateCompiled(f, variable, x);
        logs.Add("");
        logs.Add($">>> BULUNAN KÖK: {variable} = {x.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = {finalFx.ToString("0.##########", CultureInfo.InvariantCulture)})");
        return logs;
    }

    public static List<string> Method_Bisection(string _eq, decimal _min, decimal _max, int _iterationCount, string variable = "x")
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        Function f = CompileFunction(_eq);
        List<string> logs = new List<string>();

        decimal a = _min;
        decimal b = _max;
        if (a >= b)
        {
            return new List<string> { "Hata: Alt sınır (Min) üst sınırdan (Max) küçük olmalıdır." };
        }

        decimal fa = EvaluateCompiled(f, variable, a);
        decimal fb = EvaluateCompiled(f, variable, b);

        if (DecimalMath.Abs(fa) <= 1e-12m)
        {
            logs.Add($"✓ Alt sınır {variable} = {a} zaten bir köktür: f({a}) = {fa}");
            logs.Add($">>> BULUNAN KÖK: {variable} = {a.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = 0)");
            return logs;
        }
        if (DecimalMath.Abs(fb) <= 1e-12m)
        {
            logs.Add($"✓ Üst sınır {variable} = {b} zaten bir köktür: f({b}) = {fb}");
            logs.Add($">>> BULUNAN KÖK: {variable} = {b.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = 0)");
            return logs;
        }

        if (fa * fb > 0)
        {
            return new List<string>
            {
                $"Uyarı: Seçilen [{a}, {b}] aralığında işaret değişimi yok!",
                $"f({a}) = {fa:0.######} ve f({b}) = {fb:0.######} aynı işaretli.",
                "Bisection (İkiye Bölme) yöntemi için f(Min) * f(Max) <= 0 olmalıdır."
            };
        }

        logs.Add($"--- Bisection (İkiye Bölme) Yöntemi [{a}, {b}] ---");
        decimal mid = (a + b) / 2m;
        decimal tol = 1e-11m;

        for (int i = 1; i <= _iterationCount; i++)
        {
            mid = (a + b) / 2m;
            decimal fMid = EvaluateCompiled(f, variable, mid);
            decimal width = (b - a) / 2m;

            logs.Add($"[Adım {i,2}] {variable} = {mid.ToString("0.############", CultureInfo.InvariantCulture),-18} | f({variable}) = {fMid.ToString("E3", CultureInfo.InvariantCulture),-11} | Aralık: [{a:0.####}, {b:0.####}]");

            if (DecimalMath.Abs(fMid) <= tol || width <= tol)
            {
                logs.Add($"✓ Yakınsama {i}. adımda tamamlandı.");
                break;
            }

            if (fa * fMid <= 0)
            {
                b = mid;
                fb = fMid;
            }
            else
            {
                a = mid;
                fa = fMid;
            }
        }

        decimal finalFx = EvaluateCompiled(f, variable, mid);
        logs.Add("");
        logs.Add($">>> BULUNAN KÖK: {variable} = {mid.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = {finalFx.ToString("0.##########", CultureInfo.InvariantCulture)})");
        return logs;
    }

    public static List<string> Method_Secant(string _eq, decimal _min, decimal _max, int _iterationCount, string variable = "x")
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        Function f = CompileFunction(_eq);
        List<string> logs = new List<string>();

        decimal x0 = _min;
        decimal x1 = _max;
        decimal x2 = x1;
        decimal tol = 1e-11m;

        logs.Add($"--- Secant (Kiriş) Yöntemi ({variable}₀ = {x0}, {variable}₁ = {x1}) ---");

        for (int i = 1; i <= _iterationCount; i++)
        {
            decimal f0 = EvaluateCompiled(f, variable, x0);
            decimal f1 = EvaluateCompiled(f, variable, x1);

            if (DecimalMath.Abs(f1 - f0) <= 1e-15m)
            {
                logs.Add($"[Adım {i}] f({variable}₁) - f({variable}₀) ≈ 0 oldu. İşlem durduruldu.");
                break;
            }

            x2 = x1 - f1 * (x1 - x0) / (f1 - f0);
            decimal err = DecimalMath.Abs(x2 - x1);
            x0 = x1;
            x1 = x2;

            decimal f2 = EvaluateCompiled(f, variable, x2);
            logs.Add($"[Adım {i,2}] {variable} = {x2.ToString("0.############", CultureInfo.InvariantCulture),-18} | f({variable}) = {f2.ToString("E3", CultureInfo.InvariantCulture),-11} | |Δ| = {err.ToString("E3", CultureInfo.InvariantCulture)}");

            if (err <= tol || DecimalMath.Abs(f2) <= tol)
            {
                logs.Add($"✓ Yakınsama {i}. adımda tamamlandı.");
                break;
            }
        }

        decimal finalFx = EvaluateCompiled(f, variable, x2);
        logs.Add("");
        logs.Add($">>> BULUNAN KÖK: {variable} = {x2.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = {finalFx.ToString("0.##########", CultureInfo.InvariantCulture)})");
        return logs;
    }

    public static List<string> Method_RegulaFalsi(string _eq, decimal _min, decimal _max, int _iterationCount, string variable = "x")
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        Function f = CompileFunction(_eq);
        List<string> logs = new List<string>();

        decimal a = _min;
        decimal b = _max;
        if (a >= b)
        {
            return new List<string> { "Hata: Alt sınır (Min) üst sınırdan (Max) küçük olmalıdır." };
        }

        decimal fa = EvaluateCompiled(f, variable, a);
        decimal fb = EvaluateCompiled(f, variable, b);

        if (DecimalMath.Abs(fa) <= 1e-12m)
        {
            logs.Add($"✓ Alt sınır {variable} = {a} zaten bir köktür: f({a}) = {fa}");
            logs.Add($">>> BULUNAN KÖK: {variable} = {a.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = 0)");
            return logs;
        }
        if (DecimalMath.Abs(fb) <= 1e-12m)
        {
            logs.Add($"✓ Üst sınır {variable} = {b} zaten bir köktür: f({b}) = {fb}");
            logs.Add($">>> BULUNAN KÖK: {variable} = {b.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = 0)");
            return logs;
        }

        if (fa * fb > 0)
        {
            return new List<string>
            {
                $"Uyarı: Seçilen [{a}, {b}] aralığında işaret değişimi yok!",
                $"f({a}) = {fa:0.######} ve f({b}) = {fb:0.######} aynı işaretli.",
                "Regula Falsi yöntemi için f(Min) * f(Max) <= 0 olmalıdır."
            };
        }

        logs.Add($"--- Regula Falsi (Yer Değiştirme) Yöntemi [{a}, {b}] ---");
        decimal c = a;
        decimal tol = 1e-11m;
        int side = 0;

        for (int i = 1; i <= _iterationCount; i++)
        {
            if (DecimalMath.Abs(fb - fa) <= 1e-15m) break;

            decimal prevC = c;
            c = (a * fb - b * fa) / (fb - fa);
            decimal fc = EvaluateCompiled(f, variable, c);
            decimal err = DecimalMath.Abs(c - prevC);

            logs.Add($"[Adım {i,2}] {variable} = {c.ToString("0.############", CultureInfo.InvariantCulture),-18} | f({variable}) = {fc.ToString("E3", CultureInfo.InvariantCulture),-11} | |Δ| = {err.ToString("E3", CultureInfo.InvariantCulture)}");

            if (DecimalMath.Abs(fc) <= tol || (i > 1 && err <= tol))
            {
                logs.Add($"✓ Yakınsama {i}. adımda tamamlandı.");
                break;
            }

            if (fa * fc <= 0)
            {
                b = c;
                fb = fc;
                if (side == -1) fa *= 0.5m;
                side = -1;
            }
            else
            {
                a = c;
                fa = fc;
                if (side == 1) fb *= 0.5m;
                side = 1;
            }
        }

        decimal finalFx = EvaluateCompiled(f, variable, c);
        logs.Add("");
        logs.Add($">>> BULUNAN KÖK: {variable} = {c.ToString("0.##########", CultureInfo.InvariantCulture)}   (f({variable}) = {finalFx.ToString("0.##########", CultureInfo.InvariantCulture)})");
        return logs;
    }

    /// <summary>
    /// Scans the given [minX, maxX] interval with a fine grid, finds all sign changes and touching roots,
    /// and refines each root with high precision. Avoids poles/asymptotes.
    /// </summary>
    public static List<(decimal Root, decimal FVal)> FindAllRoots(string eq, decimal minX, decimal maxX, string variable = "x", int samples = 500)
    {
        if (string.IsNullOrWhiteSpace(variable)) variable = "x";
        if (minX > maxX) (minX, maxX) = (maxX, minX);
        if (minX == maxX) maxX = minX + 1m;

        Function f = CompileFunction(eq);
        List<(decimal Root, decimal FVal)> results = new List<(decimal, decimal)>();
        decimal step = (maxX - minX) / samples;

        decimal? prevX = null;
        decimal? prevY = null;

        for (int i = 0; i <= samples; i++)
        {
            decimal curX = minX + i * step;
            decimal curY;
            try
            {
                curY = EvaluateCompiled(f, variable, curX);
                if (DecimalMath.Abs(curY) > 1e7m) curY = 1e8m;
            }
            catch
            {
                prevX = null;
                prevY = null;
                continue;
            }

            if (DecimalMath.Abs(curY) <= 1e-9m)
            {
                AddUniqueRoot(results, curX, curY);
            }
            else if (prevX.HasValue && prevY.HasValue && (prevY.Value * curY < 0))
            {
                // Check if this is a pole jump rather than a true crossing
                if (DecimalMath.Abs(curY - prevY.Value) < 500m)
                {
                    decimal r = RefineRootBisection(f, variable, prevX.Value, curX, prevY.Value, curY);
                    try
                    {
                        decimal fr = EvaluateCompiled(f, variable, r);
                        if (DecimalMath.Abs(fr) <= 1e-4m)
                        {
                            AddUniqueRoot(results, r, fr);
                        }
                    }
                    catch { }
                }
            }

            prevX = curX;
            prevY = curY;
        }

        // Also check if any local minima dip close to zero (even multiplicity roots e.g. (x-2)^2)
        decimal checkStep = (maxX - minX) / Math.Min(samples, 100);
        for (int i = 0; i <= 100; i++)
        {
            decimal pt = minX + i * checkStep;
            try
            {
                decimal yPt = EvaluateCompiled(f, variable, pt);
                if (DecimalMath.Abs(yPt) < 0.2m)
                {
                    decimal nrRoot = RefineRootNewton(f, variable, pt, minX, maxX);
                    decimal fnr = EvaluateCompiled(f, variable, nrRoot);
                    if (DecimalMath.Abs(fnr) <= 1e-5m && nrRoot >= minX - 0.05m && nrRoot <= maxX + 0.05m)
                    {
                        AddUniqueRoot(results, nrRoot, fnr);
                    }
                }
            }
            catch { }
        }

        results.Sort((a, b) => a.Root.CompareTo(b.Root));
        return results;
    }

    private static decimal RefineRootBisection(Function f, string variable, decimal a, decimal b, decimal fa, decimal fb)
    {
        decimal mid = (a + b) / 2m;
        for (int step = 0; step < 50; step++)
        {
            mid = (a + b) / 2m;
            if (DecimalMath.Abs(b - a) < 1e-13m) break;

            decimal fmid;
            try { fmid = EvaluateCompiled(f, variable, mid); }
            catch { break; }

            if (DecimalMath.Abs(fmid) < 1e-13m) return mid;

            if (fa * fmid <= 0)
            {
                b = mid;
                fb = fmid;
            }
            else
            {
                a = mid;
                fa = fmid;
            }
        }
        return mid;
    }

    private static decimal RefineRootNewton(Function f, string variable, decimal x0, decimal minX, decimal maxX)
    {
        decimal x = x0;
        decimal h = 0.0001m;
        for (int step = 0; step < 25; step++)
        {
            decimal fx;
            try { fx = EvaluateCompiled(f, variable, x); }
            catch { break; }

            if (DecimalMath.Abs(fx) < 1e-12m) return x;

            decimal df;
            try
            {
                decimal fPlus = EvaluateCompiled(f, variable, x + h);
                decimal fMinus = EvaluateCompiled(f, variable, x - h);
                df = (fPlus - fMinus) / (2m * h);
            }
            catch { break; }

            if (DecimalMath.Abs(df) < 1e-12m) break;

            decimal nextX = x - fx / df;
            if (nextX < minX - 1m || nextX > maxX + 1m) break;
            if (DecimalMath.Abs(nextX - x) < 1e-12m) return nextX;
            x = nextX;
        }
        return x;
    }

    private static void AddUniqueRoot(List<(decimal Root, decimal FVal)> list, decimal root, decimal fval)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (DecimalMath.Abs(list[i].Root - root) < 1e-4m)
            {
                // If existing root has worse accuracy, update it
                if (DecimalMath.Abs(fval) < DecimalMath.Abs(list[i].FVal))
                {
                    list[i] = (root, fval);
                }
                return;
            }
        }
        list.Add((root, fval));
    }
}
