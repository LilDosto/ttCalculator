using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericalMethods2
{
    public List<Equation> Equations = new List<Equation>();

    #region Create
    /*public void Create()
    {
        Equation eq1 = new Equation(new List<float> { -6, 4, -1 }, -4);
        Equation eq2 = new Equation(new List<float> { 1, 4, -1 }, 2);
        Equation eq3 = new Equation(new List<float> { -2, 4, -7 }, 18);
        Equations.Add(eq1);
        Equations.Add(eq2);
        Equations.Add(eq3);


        Equations.Clear();

        for (int i = 0; i < EquationSystem.VarCount; i++)
        {
            List<float> coeffs = new List<float>();
            for (int j = 0; j < EquationSystem.VarCount; j++)
            {
                coeffs.Add(Random.Range(-10f, 10f));
            }
            Equation eq = new Equation(coeffs, Random.Range(-10f, 10f));
            Equations.Add(eq);
        }

        string log = "";
        for (int i = 0; i < EquationSystem.VarCount; i++)
        {
            for (int j = 0; j < EquationSystem.VarCount; j++)
            {
                log += Equations[i].Coefficients[j] + ((j == EquationSystem.VarCount - 1) ? " " + Equations[i].Constant + "\n" : " ");
            }
        }
        Debug.Log(log);

        MethodLinear_GaussSeidal(Equations, new List<float> { 2f, 2f, 2f }, 3);
    }*/
    #endregion

    public static List<float> Method_GaussSeidal(List<Equation> _eq, List<float> initialValues, int iterationCount)
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
                float sum0 = 0;

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
            return new List<float>();
        }
        else
        {
            List<float> answers = initialValues;
            for (int i = 0; i < iterationCount; i++)
            {
                for (int j = 0; j < _eq.Count; j++)
                {
                    float sum = _eq[j].Constant + _eq[j].Coefficients[j] * answers[j];
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
    }


    public static float Method_EvaluateFunction(Equation eq, float point)
    {
        float result = 0;
        if (!eq.MultiVariable)
        {
            float sum = 0;
            for (int i = 0; i < eq.Coefficients.Count; i++)
            {
                sum += eq.Coefficients[i] * Mathf.Pow(point, eq.Coefficients.Count - i);
            }
            result = sum + eq.Constant;
        }
        else
        {
            Debug.LogError("Method_EvaluateFunction: The equation is multivariable!");
        }
        return result;
    }


    public static Equation Method_TaylorSeries(Equation eq, int degree)
    {
        float constant = Method_EvaluateFunction(eq, 0);
        List<float> coeffs = new List<float>();
        for (int i = 1; i < degree + 1; i++)
        {
            float c = Method_Derivative(eq, i, 0, 0.1f) / Factorial(i);
            coeffs.Add(c);
        }

        coeffs.Reverse();

        Equation taylorEq = new Equation(coeffs, constant);
        return taylorEq;
    }

    public static float Method_NewtonRaphson(Equation eq, float initialValue, int iterationCount)
    {
        float Lipschitz = Method_EvaluateFunction(eq, initialValue) * Method_SecondDerivative(eq, initialValue, 0.1f) /
            Method_FirstDerivative(eq, initialValue, 0.1f) / Method_FirstDerivative(eq, initialValue, 0.1f);

        if (Lipschitz < 1) { }
        else
        {
            Debug.LogError("Wrong initial value!");
            return 0;
        }

        float init = initialValue;
        for (int i = 0; i < iterationCount; i++)
        {
            float next = init - Method_EvaluateFunction(eq, init) / Method_FirstDerivative(eq, init, 0.1f);
            init = next;
        }
        return init;
    }

    public static float Method_Derivative(Equation eq, int degree, float point, float h)
    {
        Equation mainEq = eq, resEq = eq;
        float sum = 0;
        for (int i = 0; i <= degree; i++)
        {
            sum += Mathf.Pow(-1, i) * Combination(degree, i) * Method_EvaluateFunction(eq, point + (degree - i) * h);
        }
        float res = sum / Mathf.Pow(h, degree);
        return res;
    }

    public static float Method_FirstDerivative(Equation eq, float point, float h)
    {
        float fxi = Method_EvaluateFunction(eq, point), fxi1 = Method_EvaluateFunction(eq, point - h), fxi2 = Method_EvaluateFunction(eq, point - 2 * h);
        return (3 * fxi + fxi2 - 4 * fxi1) / 2 / h;
    }
    public static float Method_SecondDerivative(Equation eq, float point, float h)
    {
        float fxi = Method_EvaluateFunction(eq, point), fxi1 = Method_EvaluateFunction(eq, point - h), fxi2 = Method_EvaluateFunction(eq, point - 2 * h);
        return (fxi + fxi2 - 2 * fxi1) / h / h;
    }
    public static float Method_ThirdDerivative(Equation eq, float point, float h)
    {
        float fxi = Method_EvaluateFunction(eq, point), fxi1 = Method_EvaluateFunction(eq, point - h), fxi2 = Method_EvaluateFunction(eq, point - 2 * h),
            fxi3 = Method_EvaluateFunction(eq, point - 3 * h), fxi4 = Method_EvaluateFunction(eq, point - 4 * h);
        return (3 * fxi4 - 14 * fxi3 + 24 * fxi2 - 18 * fxi1 + 5 * fxi) / 2 / h / h / h;

    }
    public static float Method_FourthDerivative(Equation eq, float point, float h)
    {
        float fxi = Method_EvaluateFunction(eq, point), fxi1 = Method_EvaluateFunction(eq, point - h), fxi2 = Method_EvaluateFunction(eq, point - 2 * h),
            fxi3 = Method_EvaluateFunction(eq, point - 3 * h), fxi4 = Method_EvaluateFunction(eq, point - 4 * h), fxi5 = Method_EvaluateFunction(eq, point - 5 * h);
        return (-2 * fxi5 + 11 * fxi4 - 24 * fxi3 + 26 * fxi2 - 14 * fxi1 + 3 * fxi) / h / h / h / h;
    }

    public static int Factorial(int num)
    {
        if (num >= 1)
        {
            return num * Factorial(num - 1);
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
}
