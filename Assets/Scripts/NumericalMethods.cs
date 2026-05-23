using DotNetMatrix;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class NumericalMethods
{
    #region GaussSeidal Yap Burayı
    /*public static List<decimal> Method_GaussSeidal(List<string> _eq, Dictionary<string, decimal> initialValues, int iterationCount)
    {
        //yakınsama şartı (3x3 matris için) -> |a11|>|a12|+|a13| && |a22|>|a21|+|a23| && |a33|>|a31|+|a32|

        bool convergence = true;
        List<int> dominantTerms = new List<int>();
        for (int i = 0; i < _eq.Count; i++)
        {
            int currentDominant = -1;
            bool dominantFound = false;
            for (int j = 0; j < _eq.Count; j++)
            {
                decimal sum0 = 0;

                for (int k = 0; k < _eq.Count; k++)
                {
                    if (k != j)
                    {
                        sum0 += Mathf.Abs(_eq[i].Coefficients[k]);
                    }
                }

                if (Mathf.Abs(_eq[i].Coefficients[j]) > sum0)
                {
                    currentDominant = j;
                    dominantFound = true;
                    break;
                }
            }

            if (dominantFound)
            {
                dominantTerms.Add(currentDominant);
            }
            else
            {
                dominantTerms.Add(-1);
            }
        }

        for (int i = 0; i < dominantTerms.Count && convergence; i++)
        {
            for (int j = 0; j < dominantTerms.Count; j++)
            {
                if (i != j)
                {
                    if (dominantTerms[i] == dominantTerms[j])
                    {
                        convergence = false;
                        break;
                    }
                }
            }
        }

        if (convergence == false)
        {
            Debug.LogError("This matris cannot be solved with Gauss-Seidal Method!");
            return new List<decimal>();
        }
        else
        {
            List<decimal> answers = initialValues;
            for (int i = 0; i < iterationCount; i++)
            {
                for (int j = 0; j < _eq.Count; j++)
                {
                    decimal sum = _eq[j].Constant + _eq[j].Coefficients[j] * answers[j];
                    for (int k = 0; k < _eq.Count; k++)
                    {
                        sum += -_eq[j].Coefficients[k] * answers[k];
                    }
                    answers[j] = sum / _eq[j].Coefficients[j];
                }
            }
            string ansdeb = "";
            for (int i = 0; i < answers.Count; i++)
            {
                ansdeb += answers[i] + ((i == answers.Count - 1) ? "" : ", ");
            }
            Debug.Log(ansdeb);

            return answers;
        }
    }*/
    #endregion


    public static decimal Method_EvaluateFunction(string eq, string variable, decimal point)
    {
        string newEq = eq;

        newEq = newEq.Replace(variable, point.ToString(/*new CultureInfo("en-US")*/));

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
            newEq = newEq.Replace(pt.Key, pt.Value.ToString(/*new CultureInfo("en-US")*/));
        }
        //Debug.Log(newEq);

        Function f = new Function();
        f.Parse(newEq);
        f.Infix2Postfix();
        f.EvaluatePostfix();
        //Debug.Log(f.ErrorDescription);
        //Debug.Log(Math.Round(f.m_result, 5));
        return f.m_result;

    }


    public static Dictionary<string, decimal> MethodMultiVar_SolveEquationSystem(List<string> eqs, Dictionary<string, decimal> initPoints, int iterationCount = 5, decimal h = 0.001m)
    {
        Dictionary<string, decimal> dynamicPoints = initPoints;
        for (int itC = 0; itC < iterationCount; itC++)
        {
            List<string> linearizedEqs = new List<string>(), variablesStr = new List<string>();
            List<decimal> leftVector = new List<decimal>();
            for (int i = 0; i < eqs.Count; i++)
            {
                string le = MethodMultiVar_TaylorSeries(eqs[i], 1, dynamicPoints, h);

                int charCount = 0;
                for (int j = 0; j < le.Length; j++)
                {
                    if ((le[j] == '+' || le[j] == '-') && j != 0) break;
                    if (le[j] == '*')
                    {
                        charCount = 0;
                        break;
                    }
                    charCount++;
                    if (j == le.Length - 1 && charCount != 0) charCount = 0;
                }

                //Debug.Log(le + " | " + charCount);

                decimal leftV = charCount != 0 ? decimal.Parse(le.Substring(0, charCount)) * -1 : 0;


                string sum = "";
                foreach (KeyValuePair<string, decimal> p in dynamicPoints)
                {
                    sum += p.Key + ": " + p.Value.ToString("0." + new string('#', 339)) + ", ";
                }

                //Debug.Log(le + " | " + charCount + " | " + sum + " | " + leftV);

                le = le.Remove(0, le[charCount] == '-' ? charCount : charCount + 1);


                foreach (KeyValuePair<string, decimal> p in dynamicPoints)
                {
                    le = le.Replace("(" + p.Key + (p.Value < 0 ? "+" + DecimalMath.Abs(p.Value).ToString("0." + new string('#', 339)) : "-" + p.Value.ToString("0." + new string('#', 339))) + ")", p.Key);
                }
                //Debug.Log(le);


                linearizedEqs.Add(le);
                leftVector.Add(leftV);
            }

            foreach (KeyValuePair<string, decimal> p in dynamicPoints)
            {
                variablesStr.Add(p.Key);
            }

            decimal[][] jacobianMatrix = Matrix.ConvertLinearEquationsToMatrix(linearizedEqs, variablesStr);
            decimal[] leftHandVector = leftVector.ToArray();

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
        string newEq = constant != 0 ? constant.ToString("0." + new string('#', 339)) : "";
        for (int i = 1; i < degree + 1; i++)
        {
            //decimal der = Method_Derivative(eq, i, variable, point);
            decimal der = Method_NumericalDifferentiation(eq, i, variable, point, 4, h);
            decimal fact = Factorial(i);
            decimal coeff = (der / fact);
            string varSTR = "";
            if (coeff > 0) varSTR = "+";
            if (coeff == 0) continue;

            string addingSTR = "";
            if (point < 0)
            {
                addingSTR = "+" + DecimalMath.Abs(point).ToString("0." + new string('#', 339));
            }
            else if (point > 0)
            {
                addingSTR = "-" + point.ToString("0." + new string('#', 339));
            }

            varSTR += coeff + "*(" + variable + addingSTR + (i == 1 ? ")" : ")^" + i.ToString("0." + new string('#', 339)) + "");
            newEq += varSTR;
        }
        return newEq;
    }
    /*public static string Method_TaylorSeries2(string eq, int degree, string variable, decimal point)
    {
        decimal constant = Method_EvaluateFunction(eq, variable, point);
        constant = DecimalMath.Abs(constant) <= 0.0001m ? 0 : constant;

        string newEq = constant.ToString("0." + new string('#', 339));
        for (int i = 1; i < degree + 1; i++)
        {
            decimal der = Method_Derivative(eq, i, variable, point);
            //Debug.Log("derivative at " + i + ": " + der.ToString());
            if (DecimalMath.Abs(der) <= 0.0001m)
            {
                der = 0;
                Debug.Log(der + ": " + 0);
            }
            decimal fact = Factorial(i);
            decimal coeff = (der / fact);

            string varSTR = "";
            if (coeff > 0) varSTR = "+";
            if (coeff == 0) continue;

            string addingSTR = "";
            if (point < 0)
            {
                addingSTR = "+" + DecimalMath.Abs(point).ToString("0." + new string('#', 339));
            }
            else if (point > 0)
            {
                addingSTR = "-" + point.ToString("0." + new string('#', 339));
            }

            varSTR += coeff + "*(" + variable + addingSTR + (i == 1 ? ")" : ")^" + i.ToString("0." + new string('#', 339)));
            newEq += varSTR;
        }
        return newEq;
    }*/
    public static string MethodMultiVar_TaylorSeries(string eq, int degree, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        decimal constant = MethodMultiVar_EvaluateFunction(eq, points);
        constant = DecimalMath.Abs(constant) <= 0.0001m ? 0 : constant;
        string newEq = constant != 0 ? constant.ToString("0." + new string('#', 339)) : "";
        for (int i = 1; i < degree + 1; i++)
        {
            foreach (KeyValuePair<string, decimal> pt in points)
            {
                decimal partDer = Method_NumericalPartialDifferentiation(eq, i, pt.Key, points, 4, h);
                //decimal partDer = Method_PartialDerivative(eq, i, pt.Key, points, h);
                decimal fact = Factorial(i);
                decimal coeff = partDer / fact;
                string varSTR = "";
                if (coeff > 0) varSTR = "+";
                if (coeff == 0) continue;

                string addingSTR = "";
                if (pt.Value < 0)
                {
                    addingSTR = "+" + DecimalMath.Abs(pt.Value).ToString("0." + new string('#', 339));
                }
                else if (pt.Value > 0)
                {
                    addingSTR = "-" + pt.Value.ToString("0." + new string('#', 339));
                }
                varSTR += coeff + "*(" + pt.Key + addingSTR + (i == 1 ? ")" : ")^" + i.ToString("0." + new string('#', 339)));
                newEq += varSTR;
            }
        }
        //Debug.Log(newEq + " + " + constant + " | " + eq);
        if (constant == 0) newEq.Substring(0, 1);
        return newEq;
    }


    public static decimal Method_NewtonRaphson(string eq, string variable, decimal initialValue, int iterationCount, decimal h = 0.001m)
    {
        decimal Lipschitz = Method_EvaluateFunction(eq, variable, initialValue) * Method_NumericalDifferentiation(eq, 2, variable, initialValue, 6, h) /
            Method_NumericalDifferentiation(eq, 1, variable, initialValue, 6, h) / Method_NumericalDifferentiation(eq, 1, variable, initialValue, 6, h);

        if (Lipschitz < 1) { }
        else
        {
            Debug.LogError("Wrong initial value!");
            return 0;
        }

        decimal init = initialValue;
        for (int i = 0; i < iterationCount; i++)
        {
            //decimal next = init - Method_EvaluateFunction(eq, variable, init) / Method_Derivative(eq, 1, variable, init, h);
            decimal next = init - Method_EvaluateFunction(eq, variable, init) / Method_NumericalDifferentiation(eq, 1, variable, init, 6, h);
            init = next;
        }
        return init;
    }
    public static decimal Method_BisectionMethod(string eq, string variable, decimal lowerValue, decimal higherValue, int iterationCount)
    {
        if (lowerValue >= higherValue)
            return 0;

        decimal mp = (lowerValue + higherValue) / 2;

        for (int i = 0; i < iterationCount; i++)
        {
            mp = (lowerValue + higherValue) / 2;

            decimal evLower = Method_EvaluateFunction(eq, variable, lowerValue);
            decimal evHigher = Method_EvaluateFunction(eq, variable, higherValue);
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
            //Debug.Log(sum);
        }

        decimal res = sum / DecimalMath.Pow(h, degree);

        //Debug.Log(sum.ToString("0." + new string('#', 339)) + "  ||  " + Math.Pow((double)h, degree).ToString() +  " | " + Math.Pow((double)h, degree).ToString("0." + new string('#', 339)));
        //Debug.Log(res + "  ||  " + sum.ToString("0." + new string('#', 339)) + "  ||  " + DecimalMath.Pow(h, degree).ToString() +  " | " + DecimalMath.Pow(h, degree).ToString("0." + new string('#', 339)));
        
        return res;
    }
    public static decimal Method_PartialDerivative(string eq, int degree, string toVar, Dictionary<string, decimal> points, decimal h = 0.001m)
    {
        decimal sum = 0;
        Dictionary<string, decimal> testPS = new Dictionary<string, decimal>();
        foreach (KeyValuePair<string, decimal> item in points)
        {
            testPS.Add(item.Key, item.Value);
        }
        decimal firstToVar = testPS[toVar];
        for (int i = 0; i <= degree; i++)
        {
            testPS[toVar] = firstToVar + (degree - i) * h;
            sum += DecimalMath.Pow(-1, i) * Combination(degree, i) * MethodMultiVar_EvaluateFunction(eq, testPS);
        }
        decimal res = sum / DecimalMath.Pow(h, degree);
        return res;
    }


    public static decimal Method_NumericalDifferentiation(string eq, int degree, string variable, decimal point, int accuracy = 2, decimal h = 0.001m)
    {
        decimal result = 0;

        int m = degree, n = accuracy;
        int p = (2 * Mathf.FloorToInt((m + 1) / 2) - 2 + n) / 2;

        decimal[][] _matrix = new decimal[2 * p + 1][];
        decimal[] _rhsVector = new decimal[2 * p + 1], functionPointsX = new decimal[2 * p + 1], 
            functionPointsY = new decimal[2 * p + 1];


        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = new decimal[2 * p + 1];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                _matrix[i][j] = (decimal)Mathf.Pow(-p + j, i);
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

        //decimal[] coeffs = Matrix.SolveEquation(_matrix, _rhsVector);
        GeneralMatrix coeffs = _A.Solve(_b);

        decimal sum = 0;

        for (int i = 0; i < coeffs.Array.Length; i++)
        {
            sum += coeffs.Array[i][0] * functionPointsY[i];
        }

        result = sum / DecimalMath.Pow(h, degree);

        return result;
    }
    public static decimal Method_NumericalPartialDifferentiation(string eq, int degree, string toVar, Dictionary<string, decimal> points, int accuracy = 2, decimal h = 0.001m)
    {
        decimal result = 0;

        int m = degree, n = accuracy;
        int p = (2 * Mathf.FloorToInt((m + 1) / 2) - 2 + n) / 2;

        decimal[][] _matrix = new decimal[2 * p + 1][];
        Dictionary<string, decimal[]> functionPointsDomain = new Dictionary<string, decimal[]>();
        decimal[] _rhsVector = new decimal[2 * p + 1], functionPointsRange = new decimal[2 * p + 1];


        for (int i = 0; i < _matrix.Length; i++)
        {
            _matrix[i] = new decimal[2 * p + 1];
            for (int j = 0; j < _matrix[i].Length; j++)
            {
                _matrix[i][j] = (decimal)Mathf.Pow(-p + j, i);
            }

            if (i != m)
            {
                _rhsVector[i] = 0;
            }
            else if (i == m)
            {
                _rhsVector[i] = Factorial(m);
            }
        }

        foreach (KeyValuePair<string, decimal> item in points)
        {
            decimal[] ps = new decimal[2 * p + 1];

            if (item.Key == toVar)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i] = points[item.Key] - (p - i) * h;
                }
            }
            else
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i] = points[item.Key];
                }
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

        for (int i = 0; i < _rhsVector.Length; i++)
        {
            _b.Array[i][0] = _rhsVector[i];
        }

        //decimal[] coeffs = Matrix.SolveEquation(_matrix, _rhsVector);
        GeneralMatrix coeffs = _A.Solve(_b);

        decimal sum = 0;

        for (int i = 0; i < coeffs.Array.Length; i++)
        {
            sum += coeffs.Array[i][0] * functionPointsRange[i];
        }

        result = sum / DecimalMath.Pow(h, degree);

        return result;
    }

    #region testPow
    /*public static decimal Pow(decimal x, uint y)
    {
        decimal A = 1m;
        BitArray e = new BitArray(BitConverter.GetBytes(y));
        int t = e.Count;

        for (int i = t - 1; i >= 0; --i)
        {
            A *= A;
            if (e[i] == true)
            {
                A *= x;
            }
        }
        return A;
    }*/
    #endregion

    public static int Factorial(int num)
    {
        if (num >= 2)
        {
            return num * Factorial(num -1);
        }
        else
        {
            return 1;
        }
    }
    public static int Combination(int n, int k)
    {
        int top = Factorial(n);
        int bot1 = Factorial(n - k);
        int bot2 = Factorial(k);

        return top / bot1 / bot2;
    }
    public static int Permutation(int n, int k)
    {
        int top = Factorial(n);
        int bot = Factorial(n - k);

        return top / bot;
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
}

