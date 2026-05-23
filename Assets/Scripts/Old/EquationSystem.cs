using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationSystem : MonoBehaviour
{
    public static List<Equation> Equations = new List<Equation>();
    public static int VarCount = 3;
}

public class Equation
{
    public string EquationString = "";
    public bool MultiVariable = false;

    public List<float> Coefficients = new List<float>();
    public List<string> Variables = new List<string>();
    public float Constant;
    public List<Operation> Operations = new List<Operation>();

    public Equation(List<float> coeffs, float cons, List<string> vars = null, bool multiVar = false)
    {
        Coefficients = coeffs;
        Variables = vars;
        Constant = cons;
        MultiVariable = multiVar;
        EquationString = ConvertEquationToString();
    }
    public Equation(string eq)
    {
        List<float> coeffs = new List<float>();
        float cons = 0;
        string testSTR = "";
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '+' || eq[i] == '-')
            {
                testSTR = "";
            }
            if (eq[i] == '*')
            {
                coeffs.Add(float.Parse(testSTR));
                testSTR = "";
            }
            testSTR += eq[i];
        }

        char op = 'a';
        testSTR = "";
        for (int i = eq.Length - 1; i >= 0; i--)
        {
            if (eq[i] == '+' || eq[i] == '-')
            {
                op = eq[i];
                string newstr = "";
                for (int j = testSTR.Length - 1; j >= 0; j--)
                {
                    newstr += testSTR[j];
                }
                testSTR = op + newstr;
                cons = (testSTR.Length > 0) ? float.Parse(testSTR) : 0f;
                break;
            }
            testSTR += eq[i];
        }

        Coefficients = coeffs;
        Constant = cons;
        Variables = null;
        MultiVariable = false;
    }

    public string ConvertEquationToString()
    {
        string newEq = "";
        for(int i = 0; i < Coefficients.Count; i++)
        {
            if (i == Coefficients.Count - 1)
            {
                if (Coefficients[i] > 0)
                {
                    newEq += "+" + Coefficients[i].ToString() + "*x";
                }
                else if (Coefficients[i] < 0)
                {
                    newEq += Coefficients[i].ToString() + "*x";
                }
                else if (Coefficients[i] == 0)
                {
                    continue;
                }
            }
            else if (i == 0)
            {
                if (Coefficients[i] != 0)
                {
                    newEq += Coefficients[i].ToString() + "*x^" + (Coefficients.Count - i).ToString();
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (Coefficients[i] > 0)
                {
                    newEq += "+" + Coefficients[i].ToString() + "*x^" + (Coefficients.Count - i).ToString();
                }
                else if (Coefficients[i] < 0)
                {
                    newEq += Coefficients[i].ToString() + "*x^" + (Coefficients.Count - i).ToString();
                }
                else if (Coefficients[i] == 0)
                {
                    continue;
                }
            }
        }
        if (Constant > 0)
        {
            newEq += "+" + Constant;
        }
        else if (Constant < 0)
        {
            newEq += Constant;
        }

        string newSTR = "";
        for (int i = 0; i < newEq.Length; i++)
        {
            if (newEq[i] == ',')
            {
                newSTR += '.';
            }
            else
            {
                newSTR += newEq[i];
            }
        }
        newEq = newSTR;

        return newEq;
    }

    public void SubstituteEquation(Equation eq)
    {
        if (!MultiVariable)
        {

        }
    }
}

public enum Operation
{
    ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION, EXPONENTIATION
}
