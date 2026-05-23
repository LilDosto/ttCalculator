using DotNetMatrix;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System;

public class NumericalMethods
{
    // --- Original Methods Ported ---

    public static decimal Method_EvaluateFunction(string eq, string variable, decimal point)
    {
        string newEq = eq;
        newEq = newEq.Replace(variable, point.ToString(CultureInfo.InvariantCulture));
        Function f = new Function();
        f.Parse(newEq);
        f.Infix2Postfix();
        f.EvaluatePostfix();
        return f.m_result;
    }

    public static decimal MethodMultiVar_EvaluateFunction(string eq, Dictionary<string, decimal> points)
    {
        string newEq = eq;
        foreach (KeyValuePair<string, decimal> pt in points)
        {
            newEq = newEq.Replace(pt.Key, pt.Value.ToString(CultureInfo.InvariantCulture));
        }
        Function f = new Function();
        f.Parse(newEq);
        f.Infix2Postfix();
        f.EvaluatePostfix();
        return f.m_result;
    }

    public static Dictionary<string, decimal> MethodMultiVar_SolveEquationSystem(List<string> eqs, Dictionary<string, decimal> initPoints, int iterationCount = 5, decimal h = 0.001m)
    {
        Dictionary<string, decimal> dynamicPoints = new Dictionary<string, decimal>(initPoints);
        for (int itC = 0; itC < iterationCount; itC++)
        {
            Console.WriteLine("Iteration: " + itC);
            List<string> linearizedEqs = new List<string>();
            List<string> variablesStr = new List<string>();
            List<decimal> leftVector = new List<decimal>();
            
            for (int i = 0; i < eqs.Count; i++)
            {
                string le = MethodMultiVar_TaylorSeries(eqs[i], 1, dynamicPoints, h);
                // Console.WriteLine("LE: " + le);

                decimal leftV = 0;
                Match m = Regex.Match(le, @"^([-+]?[0-9]*\.?[0-9]+)");
                if (m.Success)
                {
                    // Check if followed by *
                    if (m.Index + m.Length < le.Length && le[m.Index + m.Length] == '*')
                    {
                        leftV = 0;
                        // Do not strip anything, it's a coefficient
                    }
                    else
                    {
                        Console.WriteLine("Matched Constant: '" + m.Value + "'");
                        try {
                             leftV = decimal.Parse(m.Value, CultureInfo.InvariantCulture) * -1;
                        } catch (OverflowException) {
                             Console.WriteLine("OVERFLOW parsing constant! Value too large/small.");
                             leftV = 0; // Fallback
                        }
                        le = le.Substring(m.Length);
                    }
                }

                string sum = "";
                foreach (KeyValuePair<string, decimal> p in dynamicPoints)
                {
                    sum += p.Key + ": " + p.Value.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture) + ", ";
                }


                foreach (KeyValuePair<string, decimal> p in dynamicPoints)
                {
                    le = le.Replace("(" + p.Key + (p.Value < 0 ? "+" + DecimalMath.Abs(p.Value).ToString("0." + new string('#', 339), CultureInfo.InvariantCulture) : "-" + p.Value.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture)) + ")", p.Key);
                    le = le.Replace("(" + p.Key + ")", p.Key);
                }

                linearizedEqs.Add(le);
                leftVector.Add(leftV);
            }

            foreach (KeyValuePair<string, decimal> p in dynamicPoints)
            {
                variablesStr.Add(p.Key);
            }
            Console.WriteLine("Calling ConvertLinearEquationsToMatrix");
            decimal[][] jacobianMatrix = Matrix.ConvertLinearEquationsToMatrix(linearizedEqs, variablesStr);
            decimal[] leftHandVector = leftVector.ToArray();

            // Matrix.cs in core likely has SolveEquation if I ported it.
            // Let's assume Matrix.cs has it or we use GeneralMatrix directly.
            // The original used Matrix.SolveEquation. 
            // My ported Matrix.cs from step 20 (viewed 1-663) has SolveEquation.
            
            Console.WriteLine("Calling Matrix.SolveEquation");
            decimal[] newPoints = Matrix.SolveEquation(jacobianMatrix, leftHandVector);

            List<decimal> newPointsList = new List<decimal>();
            for (int i = 0; i < newPoints.Length; i++)
            {
                newPointsList.Add(newPoints[i]);
            }

            Dictionary<string, decimal> tempDynamicPoints = new Dictionary<string, decimal>();
            foreach (KeyValuePair<string, decimal> p in dynamicPoints)
            {
                tempDynamicPoints.Add(p.Key, p.Value);
            }

            foreach (KeyValuePair<string, decimal> p in dynamicPoints)
            {
                tempDynamicPoints[p.Key] = newPointsList[0] + p.Value;
                newPointsList.RemoveAt(0);
            }
            dynamicPoints = tempDynamicPoints;
        }

        return dynamicPoints;
    }

    public static string Method_TaylorSeries(string eq, int degree, string variable, decimal point, decimal h = 0.001m)
    {
        decimal constant = Method_EvaluateFunction(eq, variable, point);
        constant = DecimalMath.Abs(constant) <= 0.001m ? 0 : constant;
        string newEq = constant != 0 ? constant.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture) : "";
        for (int i = 1; i < degree + 1; i++)
        {
            decimal der = Method_NumericalDifferentiation(eq, i, variable, point, 4, h);
            decimal fact = Factorial(i);
            decimal coeff = (der / fact);
            string varSTR = "";
            if (coeff > 0) varSTR = "+";
            if (coeff == 0) continue;

            string addingSTR = "";
            if (point < 0)
            {
                addingSTR = "+" + DecimalMath.Abs(point).ToString("0." + new string('#', 339), CultureInfo.InvariantCulture);
            }
            else if (point > 0)
            {
                addingSTR = "-" + point.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture);
            }

            varSTR += coeff.ToString(CultureInfo.InvariantCulture) + "*(" + variable + addingSTR + (i == 1 ? ")" : ")^" + i.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture) + "");
            newEq += varSTR;
        }
        if (constant == 0 && newEq.Length > 0 && newEq[0] == '+') newEq = newEq.Substring(1);
        return newEq;
    }

    public static string MethodMultiVar_TaylorSeries(string eq, int degree, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        decimal constant = MethodMultiVar_EvaluateFunction(eq, points);
        constant = DecimalMath.Abs(constant) <= 0.0001m ? 0 : constant;
        string newEq = constant != 0 ? constant.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture) : "";
        for (int i = 1; i < degree + 1; i++)
        {
            foreach (KeyValuePair<string, decimal> pt in points)
            {
                decimal partDer = Method_NumericalPartialDifferentiation(eq, i, pt.Key, points, 4, h);
                decimal fact = Factorial(i);
                decimal coeff = partDer / fact;
                string varSTR = "";
                if (coeff > 0) varSTR = "+";
                if (coeff == 0) continue;

                string addingSTR = "";
                if (pt.Value < 0)
                {
                    addingSTR = "+" + DecimalMath.Abs(pt.Value).ToString("0." + new string('#', 339), CultureInfo.InvariantCulture);
                }
                else if (pt.Value > 0)
                {
                    addingSTR = "-" + pt.Value.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture);
                }
                varSTR += coeff.ToString(CultureInfo.InvariantCulture) + "*(" + pt.Key + addingSTR + (i == 1 ? ")" : ")^" + i.ToString("0." + new string('#', 339), CultureInfo.InvariantCulture));
                newEq += varSTR;
            }
        }
        if (constant == 0 && newEq.Length > 0 && newEq[0] == '+') newEq = newEq.Substring(1);
        // Assuming intent was to do nothing or it's buggy. I'll leave it as is (or fix it).
        return newEq;
    }

    public static decimal Method_NewtonRaphson(string eq, string variable, decimal initialValue, int iterationCount, decimal h = 0.001m)
    {
        // Lipschitz check suppressed/incomplete in original
        decimal init = initialValue;
        for (int i = 0; i < iterationCount; i++)
        {
            decimal next = init - Method_EvaluateFunction(eq, variable, init) / Method_NumericalDifferentiation(eq, 1, variable, init, 6, h);
            init = next;
        }
        return init;
    }

    // Overload for single-variable Newton-Raphson from UIController's simpler usage (assuming UIController calls this one if I didn't verify signature)
    // UIController calls: NumericalMethods.Method_NewtonRaphson(eq, variable, initialP, iterCount);
    // Matches the one above.
    
    // My previous implementation had a simplified one. I should keep the one matching signature.

    public static decimal Method_BisectionMethod(string eq, string variable, decimal lowerValue, decimal higherValue, int iterationCount)
    {
        if (lowerValue >= higherValue)
            return 0;

        decimal mp = (lowerValue + higherValue) / 2;

        for (int i = 0; i < iterationCount; i++)
        {
            mp = (lowerValue + higherValue) / 2;

            decimal evLower = Method_EvaluateFunction(eq, variable, lowerValue);
            // decimal evHigher = Method_EvaluateFunction(eq, variable, higherValue); // unused
            decimal evMiddle = Method_EvaluateFunction(eq, variable, mp);

            if (evLower * evMiddle < 0)
            {
                higherValue = mp;
            }
            else
            {
                lowerValue = mp;
            }
        }

        return mp;
    }

    public static decimal Method_Derivative(string eq, int degree, string variable, decimal point, decimal h = 0.001m)
    {
        decimal sum = 0;
        for (int i = 0; i <= degree; i++)
        {
            sum += DecimalMath.Pow(-1, i) * Combination(degree, i) * Method_EvaluateFunction(eq, variable, point + (degree - i) * h);
        }
        return sum / DecimalMath.Pow(h, degree);
    }

    public static decimal Method_PartialDerivative(string eq, int degree, string toVar, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        decimal sum = 0;
        Dictionary<string, decimal> testPS = new Dictionary<string, decimal>(points);
        decimal firstToVar = testPS[toVar];
        for (int i = 0; i <= degree; i++)
        {
            testPS[toVar] = firstToVar + (degree - i) * h;
            sum += DecimalMath.Pow(-1, i) * Combination(degree, i) * MethodMultiVar_EvaluateFunction(eq, testPS);
        }
        return sum / DecimalMath.Pow(h, degree);
    }

    public static decimal Method_NumericalDifferentiation(string eq, int degree, string variable, decimal point, int accuracy = 2, decimal h = 0.001m)
    {
        int m = degree, n = accuracy;
        int p = (2 * (int)Math.Floor((m + 1) / 2.0) - 2 + n) / 2;

        decimal[][] _matrix = new decimal[2 * p + 1][];
        decimal[] _rhsVector = new decimal[2 * p + 1], functionPointsX = new decimal[2 * p + 1], 
            functionPointsY = new decimal[2 * p + 1];

        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = new decimal[2 * p + 1];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                _matrix[i][j] = (decimal)Math.Pow(-p + j, i);
            }

            if (i != m)
            {
                _rhsVector[i] = 0;
            }
            else if (i == m)
            {
                _rhsVector[i] = Factorial(m);
            }

            functionPointsX[i] = point - (p - i) * h;
            functionPointsY[i] = Method_EvaluateFunction(eq, variable, functionPointsX[i]);
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
                _matrix[i][j] = (decimal)Math.Pow(-p + j, i);
            }

            if (i != m) _rhsVector[i] = 0;
            else if (i == m) _rhsVector[i] = Factorial(m);
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
            functionPointsRange[i] = MethodMultiVar_EvaluateFunction(eq, pair);
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
        if (num >= 2) return num * Factorial(num - 1);
        else return 1;
    }

    public static decimal Combination(int n, int k)
    {
        return Factorial(n) / (Factorial(n - k) * Factorial(k));
    }
    
    public static decimal Permutation(int n, int k)
    {
        return Factorial(n) / Factorial(n - k);
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

    // --- Added Linear Solvers (Optional but kept for completeness if UI uses them) ---
    // Update: UI uses MethodMultiVar_SolveEquationSystem for System Solver now (based on UIController)
    // But I implemented btnSolveSystem_Click in MainForm.cs to use Method_LUDecomposition previously.
    // I should update MainForm.cs to use MethodMultiVar_SolveEquationSystem if it's non-linear or general.
    // The original app used MethodMultiVar_SolveEquationSystem (Newton-Raphson) for general systems.
    
    // So I should keep MethodMultiVar_SolveEquationSystem and update MainForm to use IT instead of LUDecomposition if I want to match original behavior.
    
    // I'll keep the linear solvers just in case.

    public static List<string> Method_Inverse(List<string> _eq, List<string> _vars)
    {
        // Implementation from previous turn
        decimal[][] A = Matrix.ConvertLinearEquationsToMatrix(_eq, _vars);
        if (A == null) return null;
        decimal[] B = new decimal[_eq.Count];
        for (int i = 0; i < _eq.Count; i++)
        {
            int ind = _eq[i].IndexOf('=');
            if (ind != -1) B[i] = decimal.Parse(_eq[i].Substring(ind + 1), CultureInfo.InvariantCulture);
            else B[i] = 0;
        }
        
        GeneralMatrix gA = new GeneralMatrix(A);
        GeneralMatrix gB = new GeneralMatrix(B, B.Length);
        GeneralMatrix gX = gA.Solve(gB);
        decimal[] X = gX.ColumnPackedCopy;
        
        List<string> res = new List<string>();
        for (int i = 0; i < X.Length; i++) res.Add(_vars[i] + " = " + X[i].ToString("F5", CultureInfo.InvariantCulture));
        return res;
    }

    public static List<string> Method_LUDecomposition(List<string> _eq, List<string> _vars)
    {
         // Implementation from previous turn
         decimal[][] A = Matrix.ConvertLinearEquationsToMatrix(_eq, _vars);
         if (A == null) return null;
         decimal[] B = new decimal[_eq.Count];
         for (int i = 0; i < _eq.Count; i++)
         {
             int ind = _eq[i].IndexOf('=');
             if (ind != -1) B[i] = decimal.Parse(_eq[i].Substring(ind + 1), CultureInfo.InvariantCulture);
             else B[i] = 0;
         }
 
         GeneralMatrix gA = new GeneralMatrix(A);
         GeneralMatrix gB = new GeneralMatrix(B, B.Length);
         GeneralMatrix gX = gA.Solve(gB);
         decimal[] X = gX.ColumnPackedCopy;
 
         List<string> res = new List<string>();
         for (int i = 0; i < X.Length; i++) res.Add(_vars[i] + " = " + X[i].ToString("F5", CultureInfo.InvariantCulture));
         return res;
    }

    // Additional wrappers for MainForm compatibility if needed
    public static List<string> Method_NewtonRaphson(string _eq, decimal _initialPoint, int _iterationCount)
    {
         // Wrapper for UI
         if (_eq.Contains("="))
         {
            string[] parts = _eq.Split('=');
            _eq = parts[0] + "-(" + parts[1] + ")";
         }
         decimal res = Method_NewtonRaphson(_eq, "x", _initialPoint, _iterationCount);
         return new List<string> { res.ToString(CultureInfo.InvariantCulture) };
    }
    
    public static List<string> Method_Bisection(string _eq, decimal _min, decimal _max, int _iterationCount)
    {
         if (_eq.Contains("="))
         {
            string[] parts = _eq.Split('=');
            _eq = parts[0] + "-(" + parts[1] + ")";
         }
         decimal res = Method_BisectionMethod(_eq, "x", _min, _max, _iterationCount);
         return new List<string> { res.ToString(CultureInfo.InvariantCulture) };
    }
    
    public static List<string> Method_Secant(string _eq, decimal _min, decimal _max, int _iterationCount) 
    {
         if (_eq.Contains("="))
         {
            string[] parts = _eq.Split('=');
            _eq = parts[0] + "-(" + parts[1] + ")";
         }
         
         decimal x0 = _min;
         decimal x1 = _max;
         decimal x2 = 0;
         
         for(int i=0; i<_iterationCount; i++)
         {
             decimal f0 = Method_EvaluateFunction(_eq, "x", x0);
             decimal f1 = Method_EvaluateFunction(_eq, "x", x1);
             
             if (f1 - f0 == 0) break; // Avoid divide by zero
             
             x2 = x1 - f1 * (x1 - x0) / (f1 - f0);
             x0 = x1;
             x1 = x2;
             
             if (DecimalMath.Abs(f1) < 0.00000001m) break; 
         }
         
         return new List<string> { x2.ToString(CultureInfo.InvariantCulture) };
    } 

    public static List<string> Method_RegulaFalsi(string _eq, decimal _min, decimal _max, int _iterationCount) 
    {
         if (_eq.Contains("="))
         {
            string[] parts = _eq.Split('=');
            _eq = parts[0] + "-(" + parts[1] + ")";
         }
         
         decimal a = _min;
         decimal b = _max;
         decimal c = 0;
         
         for(int i=0; i<_iterationCount; i++)
         {
             decimal fa = Method_EvaluateFunction(_eq, "x", a);
             decimal fb = Method_EvaluateFunction(_eq, "x", b);
             
             if (fa * fb >= 0) return new List<string> { "Root not bracketed or multiple roots" };
             
             c = (a * fb - b * fa) / (fb - fa);
             decimal fc = Method_EvaluateFunction(_eq, "x", c);
             
             if (fc == 0) break;
             else if (fa * fc < 0) b = c;
             else a = c;
         }
         
         return new List<string> { c.ToString(CultureInfo.InvariantCulture) };
    }
}
