using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FunctionEvaluator : MonoBehaviour
{
    public bool Check = false;
    public string MainEquation1, MainEquation2, MainEquation3, MainEquation4, MainEquation5;
    public int Count;
    public double var1;
    public int var2;


    public List<string> Equations = new List<string>(), VariableNames = new List<string>();
    public List<double> VariableInitialValues = new List<double>();

    Dictionary<string, decimal> dict = new Dictionary<string, decimal>();


    private void Update()
    {
        if (Check)
        {
            Check = false;

            //Debug.Log(NumericalMethods.Method_Derivative(MainEquation1, var1, "x", 0).ToString("0." + new string('#', 339)));
            //Debug.Log(NumericalMethods.Method_TaylorSeries(MainEquation1, 6, "x", 0));

            dict.Clear();
            for (int i = 0; i < VariableNames.Count; i++)
            {
                dict.Add(VariableNames[i], (decimal)VariableInitialValues[i]);
            }

            Debug.Log(DecimalMath.Cos(90));
            string res = NumericalMethods.Method_TaylorSeries(MainEquation1, Count, "x", 1, 0.01m).Replace(',', '.');
            Debug.Log(res);



            //Debug.Log(NumericalMethods.Method_NumericalPartialDifferentiation("x^2+2*x*y", 2, "x", dict, 6, 0.01m));
            //Debug.Log(NumericalMethods.Method_NumericalDifferentiation("log(x)", 3, "x", 2, 2));

            /*Dictionary<string, decimal> results = NumericalMethods.MethodMultiVar_SolveEquationSystem(Equations, dict, Count);

            string sum = "";
            foreach (KeyValuePair<string, decimal> p in results)
            {
                sum += p.Key + ": " + p.Value + ", ";
            }
            Debug.Log(sum);*/




            /*string res = NumericalMethods.Method_TaylorSeries(MainEquation1, Count, "x", 0).Replace(',', '.');
            Debug.Log(res);*/

            //Debug.Log(NumericalMethods.Method_Derivative(MainEquation1, Count, "x", (decimal)var1));


            //Debug.Log(NumericalMethods.Method_BisectionMethod(MainEquation1, "x", -4, -2, 20));




            /*Dictionary<string, float> root = NumericalMethods.MethodMultiVar_BisectionMethod(new List<string> { MainEquation1, MainEquation2 }, new List<string> { "x", "y" }, var1, var2, taylorCount);
            foreach (KeyValuePair<string, float> r in root)
            {
                Debug.Log(r.Key + ": " + r.Value);
            }*/





            /*string str = NumericalMethods.GenerateCombinations("-+", "s", 3);
            Debug.Log(str);
            string[] test = str.Split('s');

            string str2 = "";
            for (int i = 0; i < test.Length; i++)
            {
                str2 += test[i];
            }
            Debug.Log(str2);*/





            /*string e1 = NumericalMethods.MethodMultiVar_TaylorSeries(MainEquation1, 1, dict);
            string e2 = NumericalMethods.MethodMultiVar_TaylorSeries(MainEquation2, 1, dict);
            string e3 = NumericalMethods.MethodMultiVar_TaylorSeries(MainEquation3, 1, dict);
            Debug.Log(e1 + " | " + e2 + " | " + e3);

            float[][] tea = new float[][] { new float[] { 3, 7, -2 }, new float[] { -1, 1, 2 }, new float[] { 3, -2, -1 } };
            float[] leftv = new float[] { -5, 2, -1 };
            float[] res = Matrix.SolveEquation(tea, leftv);
            string sum = "";
            for (int i = 0; i < res.Length; i++) sum += res[i] + ", ";
            Debug.Log(sum);*/





            /*Dictionary<string, float> testt = new Dictionary<string, float>();
            testt.Add("x", 1);
            testt.Add("y", 2);
            Debug.Log(NumericalMethods.Method_PartialDerivative("x^(3*y)*y^x", 1, "x", testt, 0.001f));*/




            /*List<string> LES = new List<string>();
            LES.Add(MainEquation1);
            LES.Add(MainEquation2);
            LES.Add(MainEquation3);
            List<string> VARS = new List<string>();
            VARS.Add("x");
            VARS.Add("y");
            VARS.Add("z");
            float[][] test = Matrix.ConvertLinearEquationsToMatrix(LES, VARS);

            string deb = "";
            for (int i = 0; i < test.Length; i++)
            {
                for (int j = 0; j < test[i].Length; j++)
                {
                    deb += test[i][j].ToString() + ", ";
                }
                deb += "\n";
            }
            Debug.Log(deb);*/




            //Debug.Log(NumericalMethods.MethodMultiVar_TaylorSeries(MainEquation2, 1, dict));




            //Debug.Log(NumericalMethods.Method_PartialDerivative(MainEquation, 1, "x", dict, 0.01f));




            /*Function f = new Function();
            f.Parse(MainEquation1);
            f.Infix2Postfix();
            f.EvaluatePostfix();
            Debug.Log(f.m_result);




            for (int i = 0; i < f.m_equation.Count; i++)
            {
                Debug.Log("EEEEE:   " + f.m_equation[i].ToString());
            }
            for (int i = 0; i < f.m_postfix.Count; i++)
            {
                Debug.Log("PPPPP:   " + f.m_postfix[i].ToString());
            }*/
        }
    }
}
