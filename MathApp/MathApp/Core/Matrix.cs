using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DotNetMatrix;

public class Matrix
{
    private static Random random = new Random();

    public static decimal[][] GenerateRandomMatrix(int rows, int columns, decimal min, decimal max)
    {
        decimal[][] res = new decimal[rows][];
        for (int i = 0; i < rows; i++)
        {
            res[i] = new decimal[columns];
            for (int j = 0; j < columns; j++)
            {
                double range = (double)(max - min);
                double sample = random.NextDouble();
                double scaled = (sample * range) + (double)min;
                decimal value = (decimal)System.Math.Round(scaled, 2);
                res[i][j] = value;
            }
        }
        return res;
    }

    public static decimal[][] Transpoze(decimal[][] _m)
    {
        if (_m == null || _m.Length == 0) return new decimal[0][];
        decimal[][] res = new decimal[_m[0].Length][];
        for (int i = 0; i < _m[0].Length; i++)
        {
            res[i] = new decimal[_m.Length];
        }
        for (int i = 0; i < _m.Length; i++)
        {
            for (int j = 0; j < _m[0].Length; j++)
            {
                res[j][i] = _m[i][j];
            }
        }
        return res;
    }

    public static decimal MatrixDeterminant(decimal[][] _m)
    {
        if (_m == null || _m.Length == 0) return 0;
        if (_m.Length != _m[0].Length)
        {
            throw new ArgumentException("Determinant hesabı için matris kare (N x N) olmalıdır.");
        }

        int n = _m.Length;
        if (n == 1) return _m[0][0];
        if (n == 2) return _m[0][0] * _m[1][1] - _m[0][1] * _m[1][0];

        // Use O(n^3) LU Decomposition instead of O(n!) recursive Laplace expansion
        GeneralMatrix gm = GeneralMatrix.ConstructWithCopy(_m);
        return gm.Determinant();
    }

    public static decimal[] SolveEquation(decimal[][] _m, decimal[] _v)
    {
        if (_m == null || _v == null || _m.Length == 0) return new decimal[0];
        int len = _m.Length;

        GeneralMatrix A = GeneralMatrix.ConstructWithCopy(_m);
        GeneralMatrix b = new GeneralMatrix(len, 1);
        for (int i = 0; i < len; i++)
        {
            b.Array[i][0] = _v[i];
        }

        GeneralMatrix x = A.Solve(b);
        decimal[] res = new decimal[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = x.Array[i][0];
        }
        return res;
    }

    public static decimal[][] ConvertLinearEquationsToMatrix(List<string> equations, List<string> variables)
    {
        if (equations == null || variables == null || equations.Count != variables.Count) return null;

        int n = equations.Count;
        decimal[][] res = new decimal[n][];
        for (int i = 0; i < n; i++)
        {
            res[i] = new decimal[n];
            string eq = equations[i];
            if (eq.Contains("="))
            {
                string[] parts = eq.Split('=');
                eq = parts[0] + "-(" + parts[1] + ")";
            }

            // Use numerical evaluation to extract exact linear coefficients reliably:
            // f(0, 0, ...) = c
            // f(0, ..., x_k=1, ..., 0) - c = coeff_k
            Dictionary<string, decimal> zeroPts = new Dictionary<string, decimal>();
            foreach (string v in variables) zeroPts[v] = 0m;

            decimal f0 = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, zeroPts);

            for (int k = 0; k < n; k++)
            {
                string varName = variables[k];
                zeroPts[varName] = 1m;
                decimal f1 = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, zeroPts);
                res[i][k] = f1 - f0;
                zeroPts[varName] = 0m;
            }
        }
        return res;
    }

    public static decimal[][] MatrixMultiplication(decimal[][] _m1, decimal[][] _m2)
    {
        if (_m1 == null || _m2 == null || _m1.Length == 0 || _m2.Length == 0) return null;
        if (_m1[0].Length == _m2.Length)
        {
            decimal[][] res = new decimal[_m1.Length][];
            for (int i = 0; i < _m1.Length; i++)
            {
                res[i] = new decimal[_m2[0].Length];
            }

            for (int i = 0; i < res.Length; i++)
            {
                for (int j = 0; j < res[0].Length; j++)
                {
                    decimal sum = 0;
                    for (int k = 0; k < _m2.Length; k++)
                    {
                        sum += _m1[i][k] * _m2[k][j];
                    }
                    res[i][j] = sum;
                }
            }
            return res;
        }
        else
        {
            throw new ArgumentException("Birinci matrisin sütun sayısı ile ikinci matrisin satır sayısı eşit olmalıdır.");
        }
    }

    public static decimal[][] MatrixMultiplication(decimal[][] _m, decimal _c)
    {
        if (_m == null) return null;
        decimal[][] res = new decimal[_m.Length][];
        for (int i = 0; i < _m.Length; i++)
        {
            res[i] = new decimal[_m[i].Length];
            for (int j = 0; j < _m[i].Length; j++)
            {
                res[i][j] = _m[i][j] * _c;
            }
        }
        return res;
    }

    public static decimal[] TurnListToVector(List<decimal> _vec)
    {
        return _vec.ToArray();
    }

    public static decimal[][] TurnListToMatrix(List<List<decimal>> _m)
    {
        decimal[][] res = new decimal[_m.Count][];
        for (int i = 0; i < _m.Count; i++)
        {
            res[i] = _m[i].ToArray();
        }
        return res;
    }

    public static string DebugMatrix(decimal[][] _m, string _sep = "\t")
    {
        if (_m == null || _m.Length == 0) return "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < _m.Length; i++)
        {
            for (int j = 0; j < _m[i].Length; j++)
            {
                sb.Append(_m[i][j].ToString("0.######", CultureInfo.InvariantCulture));
                sb.Append(j == _m[i].Length - 1 ? Environment.NewLine : _sep);
            }
        }
        return sb.ToString();
    }

    public static string DebugVector(decimal[] _v, string _sep = " ")
    {
        if (_v == null) return "";
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < _v.Length; i++)
        {
            sb.AppendLine(_v[i].ToString("0.######", CultureInfo.InvariantCulture));
        }
        return sb.ToString();
    }
}
