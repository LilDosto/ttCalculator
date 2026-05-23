using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationSystem2 : MonoBehaviour
{
    
}



public class Equation2
{
    public static char[] AllVariables = new char[] { 'x', 'y', 'z', 't', 'w', 'r' };

    public string EquationString = "";

    public enum Type { Variable, Value, UnaryOperator, Operator, Function, Result, Bracket, Comma, Error }
    public struct Symbol
    {
        string SYM_Name;
        double SYM_Value;
        Type SYM_Type;
        public override string ToString()
        {
            return SYM_Name;
        }
    }

    public ArrayList m_equation = new ArrayList();

    public Equation2(string eq)
    {
        EquationString = eq;
    }
}
