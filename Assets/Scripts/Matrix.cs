using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    public static decimal[][] GenerateRandomMatrix(int rows, int columns, decimal min, decimal max)
    {
        decimal[][] res = new decimal[rows][];
        for (int i = 0; i < rows; i++)
        {
            res[i] = new decimal[columns];
            for (int j = 0; j < columns; j++)
            {
                decimal value = (decimal)UnityEngine.Random.Range((float)min, (float)max);
                value = (decimal)System.Math.Round(value, 2);
                res[i][j] = value;
            }
        }
        return res;
    }

    public static decimal[][] Transpoze(decimal[][] _m)
    {
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
        if (_m.Length == _m[0].Length)
        {
            int c = _m.Length;
            if (c == 2)
            {
                return (decimal)Math.Round(_m[0][0] * _m[1][1] - _m[0][1] * _m[1][0], 5);
            }
            else if (c == 1)
            {
                return (decimal)Math.Round(_m[0][0], 5);
            }
            else if (c == 0)
            {
                Debug.LogError("Matrix is empty!");
                return 0;
            }
            else
            {
                decimal resDet = 0;
                for (int i = 0; i < c; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        decimal[][] newM = new decimal[c - 1][];
                        for (int k = 0; k < c - 1; k++)
                        {
                            newM[k] = new decimal[c - 1];
                        }

                        bool isAddedK = false, isAddedL = false;
                        int offsetK = 0, offsetL = 0;

                        for (int k = 0; k < c; k++)
                        {
                            isAddedL = false;
                            offsetL = 0;
                            for (int l = 0; l < c; l++)
                            {
                                if (!(k == i || l == j))
                                {
                                    newM[k + offsetK][l + offsetL] = _m[k][l];
                                }
                                else
                                {
                                    if (!isAddedK && k == i)
                                    {
                                        offsetK = -1;
                                        isAddedK = true;
                                    }
                                    if (!isAddedL && l == j)
                                    {
                                        offsetL = -1;
                                        isAddedL = true;
                                    }
                                }
                            }
                        }

                        decimal cofactor = (decimal)Math.Pow(-1, 0 + j + 2) * MatrixDeterminant(newM);
                        resDet += cofactor * _m[0][j];
                    }
                }
                return (decimal)Math.Round(resDet, 5);
            }
        }
        else
        {
            Debug.LogError("This matrix is not a square matrix!");
            return 0;
        }
    }

    public static decimal[] SolveEquation(decimal[][] _m, decimal[] _v)
    {
        int len = _m.Length;

        decimal[] res = new decimal[len];
        decimal d0 = MatrixDeterminant(_m);

        for (int i = 0; i < len; i++)
        {
            decimal[][] testMa = new decimal[len][];
            for (int j = 0; j < len; j++)
            {
                testMa[j] = new decimal[len];
                for (int k = 0; k < len; k++)
                {
                    testMa[j][k] = _m[j][k];
                }
            }
            for (int j = 0; j < len; j++)
            {
                testMa[j][i] = _v[j];
            }
            decimal di = MatrixDeterminant(testMa);
            res[i] = (decimal)Math.Round(di / d0, 5);
        }

        return res;
    }

    public static decimal[][] ConvertLinearEquationsToMatrix(List<string> equations, List<string> variables)
    {
        if (equations.Count == variables.Count)
        {
            decimal[][] res = new decimal[equations.Count][];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new decimal[equations.Count];
            }
            for (int i = 0; i < equations.Count; i++)
            {
                for (int j = 0; j < equations[i].Length; j++)
                {
                    for (int k = 0; k < variables.Count; k++)
                    {
                        if (equations[i][j] == variables[k][0])
                        {
                            string coeff = "";
                            for (int l = j - 1; l >= 0; l--)
                            {
                                if (equations[i][l] != '*' && equations[i][l] != '+' && equations[i][l] != '-')
                                {
                                    coeff += equations[i][l];
                                }
                                if (equations[i][l] == '-')
                                {
                                    coeff += '-';
                                    break;
                                }
                                if (equations[i][l] == '+')
                                {
                                    break;
                                }
                            }
                            char[] charArray = coeff.ToCharArray();
                            Array.Reverse(charArray);
                            coeff = new string(charArray);
                            res[i][k] = decimal.Parse(coeff);
                        }
                    }
                }
            }
            return res;
        }
        else return null;
    }

    public static decimal[][] MatrixMultiplication(decimal[][] _m1, decimal[][] _m2)
    {
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
            Debug.LogError("Matrices cannot be multiplied!");
            return null;
        }
    }
    public static decimal[][] MatrixMultiplication(decimal[][] _m, decimal _c)
    {
        decimal[][] res = _m;
        for (int i = 0; i < _m.Length; i++)
        {
            for (int j = 0; j < _m[0].Length; j++)
            {
                res[i][j] *= _c;
            }
        }
        return res;
    }

    public static decimal[] TurnListToVector(List<decimal> _vec)
    {
        decimal[] res = new decimal[_vec.Count];
        for (int i = 0; i < _vec.Count; i++)
        {
            res[i] = _vec[i];
        }
        return res;
    }
    public static decimal[][] TurnListToMatrix(List<List<decimal>> _m)
    {
        decimal[][] res = new decimal[_m.Count][];
        for (int i = 0; i < _m.Count; i++)
        {
            res[i] = new decimal[_m[i].Count];
            for (int j = 0; j < _m[i].Count; j++)
            {
                res[i][j] = _m[i][j];
            }
        }
        return res;
    }

    public static string DebugMatrix(decimal[][] _m, string _sep = " ")
    {
        string sum = "";
        for (int i = 0; i < _m.Length; i++)
        {
            for (int j = 0; j < _m[0].Length; j++)
            {
                sum += _m[i][j] + ((j == _m[0].Length - 1) ? "\n" : _sep);
            }
        }
        return sum;
    }
    public static string DebugVector(decimal[] _v, string _sep = " ")
    {
        string sum = "";
        for (int i = 0; i < _v.Length; i++)
        {
            sum += _v[i] + "\n";
        }
        return sum;
    }

}
