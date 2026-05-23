using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// </summary>
/// 
public enum Type { Variable, Value, UnaryOperator, Operator, Function, Result, Bracket, Comma, Error }
public struct Symbol
{
    public string m_name;
    public decimal m_value;
    public Type m_type;
    public override string ToString()
    {
        return m_name;
    }
}
public delegate Symbol EvaluateFunctionDelegate(string name, params Object[] args);
public class Function
{
    public decimal Result
    {
        get
        {
            return m_result;
        }
    }

    public ArrayList Equation
    {
        get
        {
            return (ArrayList)m_equation.Clone();
        }
    }
    public ArrayList Postfix
    {
        get
        {
            return (ArrayList)m_postfix.Clone();
        }
    }

    public EvaluateFunctionDelegate DefaultFunctionEvaluation
    {
        set
        {
            m_defaultFunctionEvaluation = value;
        }
    }

    public bool Error
    {
        get
        {
            return m_bError;
        }
    }

    public string ErrorDescription
    {
        get
        {
            return m_sErrorDescription;
        }
    }

    public ArrayList Variables
    {
        get
        {
            ArrayList var = new ArrayList();
            foreach (Symbol sym in m_equation)
            {
                if ((sym.m_type == Type.Variable) && (!var.Contains(sym)))
                    var.Add(sym);
            }
            return var;
        }
        set
        {
            foreach (Symbol sym in value)
            {
                for (int i = 0; i < m_postfix.Count; i++)
                {
                    if ((sym.m_name == ((Symbol)m_postfix[i]).m_name) && (((Symbol)m_postfix[i]).m_type == Type.Variable))
                    {
                        Symbol sym1 = (Symbol)m_postfix[i];
                        sym1.m_value = sym.m_value;
                        m_postfix[i] = sym1;
                    }
                }
            }
        }
    }

    public Function()
    { }

    public void Parse(string equation)
    {
        int state = 1;
        string temp = "";
        Symbol ctSymbol = new Symbol(); // Initialized to avoid error

        m_bError = false;
        m_sErrorDescription = "None";

        m_equation.Clear();
        m_postfix.Clear();

        int nPos = 0;
        //-- Remove all white spaces from the equation string --
        equation = equation.Trim();
        while ((nPos = equation.IndexOf(' ')) != -1)
            equation = equation.Remove(nPos, 1);

        for (int i = 0; i < equation.Length; i++)
        {
            switch (state)
            {
                case 1:
                    if (Char.IsNumber(equation[i]))
                    {
                        state = 2;
                        temp += equation[i];
                    }
                    else if (Char.IsLetter(equation[i]))
                    {
                        state = 3;
                        temp += equation[i];
                    }
                    else
                    {
                        ctSymbol.m_name = equation[i].ToString();
                        ctSymbol.m_value = 0;
                        switch (ctSymbol.m_name)
                        {
                            case ",":
                            case ".":
                                ctSymbol.m_type = Type.Comma;
                                break;
                            case "(":
                            case ")":
                            case "[":
                            case "]":
                            case "{":
                            case "}":
                                ctSymbol.m_type = Type.Bracket;
                                break;
                            default:
                                if (m_equation.Count == 0)
                                    ctSymbol.m_type = Type.UnaryOperator;
                                else if ((((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Value) || (((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Variable)
                                  || (((Symbol)m_equation[m_equation.Count - 1]).m_name == ")") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "]") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "}"))
                                    ctSymbol.m_type = Type.Operator;
                                else
                                    ctSymbol.m_type = Type.UnaryOperator;
                                break;
                        }
                        m_equation.Add(ctSymbol);
                    }
                    break;
                case 2:
                    if ((Char.IsNumber(equation[i])) || (equation[i] == '.') || (equation[i] == ','))
                        temp += equation[i];
                    else if (!Char.IsLetter(equation[i]))
                    {
                        state = 1;
                        ctSymbol.m_name = temp;
                        ctSymbol.m_value = Decimal.Parse(temp, System.Globalization.CultureInfo.InvariantCulture);
                        ctSymbol.m_type = Type.Value;
                        m_equation.Add(ctSymbol);
                        ctSymbol.m_name = equation[i].ToString();
                        ctSymbol.m_value = 0;
                        switch (ctSymbol.m_name)
                        {
                            case ",":
                                ctSymbol.m_type = Type.Comma;
                                break;
                            case "(":
                            case ")":
                            case "[":
                            case "]":
                            case "{":
                            case "}":
                                ctSymbol.m_type = Type.Bracket;
                                break;
                            default:
                                if (m_equation.Count == 0)
                                    ctSymbol.m_type = Type.UnaryOperator;
                                else if ((((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Value) || (((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Variable)
                                  || (((Symbol)m_equation[m_equation.Count - 1]).m_name == ")") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "]") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "}"))
                                    ctSymbol.m_type = Type.Operator;
                                else
                                    ctSymbol.m_type = Type.UnaryOperator;
                                break;
                        }
                        m_equation.Add(ctSymbol);
                        temp = "";
                    }
                    break;
                case 3:
                    if (Char.IsLetterOrDigit(equation[i]))
                        temp += equation[i];
                    else
                    {
                        state = 1;
                        ctSymbol.m_name = temp;
                        ctSymbol.m_value = 0;
                        if (equation[i] == '(')
                            ctSymbol.m_type = Type.Function;
                        else
                        {
                            if (ctSymbol.m_name == "pi")
                                ctSymbol.m_value = DecimalMath.PI;
                            else if (ctSymbol.m_name == "e")
                                ctSymbol.m_value = DecimalMath.E;
                            ctSymbol.m_type = Type.Variable;
                        }
                        m_equation.Add(ctSymbol);
                        ctSymbol.m_name = equation[i].ToString();
                        ctSymbol.m_value = 0;
                        switch (ctSymbol.m_name)
                        {
                            case ",":
                                ctSymbol.m_type = Type.Comma;
                                break;
                            case "(":
                            case ")":
                            case "[":
                            case "]":
                            case "{":
                            case "}":
                                ctSymbol.m_type = Type.Bracket;
                                break;
                            default:
                                if (m_equation.Count == 0)
                                    ctSymbol.m_type = Type.UnaryOperator;
                                else if ((((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Value) || (((Symbol)m_equation[m_equation.Count - 1]).m_type == Type.Variable)
                                  || (((Symbol)m_equation[m_equation.Count - 1]).m_name == ")") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "]") || (((Symbol)m_equation[m_equation.Count - 1]).m_name == "}"))
                                    ctSymbol.m_type = Type.Operator;
                                else
                                    ctSymbol.m_type = Type.UnaryOperator;
                                break;
                        }
                        m_equation.Add(ctSymbol);
                        temp = "";
                    }
                    break;
            }
        }
        if (temp != "")
        {
            ctSymbol.m_name = temp;
            if (state == 2)
            {
                ctSymbol.m_value = Decimal.Parse(temp, System.Globalization.CultureInfo.InvariantCulture);
                ctSymbol.m_type = Type.Value;
            }
            else
            {
                if (ctSymbol.m_name == "pi")
                    ctSymbol.m_value = DecimalMath.PI;
                else if (ctSymbol.m_name == "e")
                    ctSymbol.m_value = DecimalMath.E;
                else
                    ctSymbol.m_value = 0;
                ctSymbol.m_type = Type.Variable;
            }
            m_equation.Add(ctSymbol);
        }
    }

    public void Infix2Postfix()
    {
        Symbol tpSym;
        Stack tpStack = new Stack();
        foreach (Symbol sym in m_equation)
        {
            if ((sym.m_type == Type.Value) || (sym.m_type == Type.Variable))
                m_postfix.Add(sym);
            else if ((sym.m_name == "(") || (sym.m_name == "[") || (sym.m_name == "{"))
                tpStack.Push(sym);
            else if ((sym.m_name == ")") || (sym.m_name == "]") || (sym.m_name == "}"))
            {
                if (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Pop();
                    while ((tpSym.m_name != "(") && (tpSym.m_name != "[") && (tpSym.m_name != "{"))
                    {
                        m_postfix.Add(tpSym);
                        tpSym = (Symbol)tpStack.Pop();
                    }
                }
            }
            else if (sym.m_type == Type.UnaryOperator)
            {
                tpStack.Push(sym);
            }
            else
            {
                if (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Pop();
                    while ((tpStack.Count != 0) && ((tpSym.m_type == Type.UnaryOperator) || (tpSym.m_type == Type.Operator) || (tpSym.m_type == Type.Function) || (tpSym.m_type == Type.Comma)) && (Precedence(tpSym) >= Precedence(sym)))
                    {
                        m_postfix.Add(tpSym);
                        tpSym = (Symbol)tpStack.Pop();
                    }
                    if (((tpSym.m_type == Type.UnaryOperator) || (tpSym.m_type == Type.Operator) || (tpSym.m_type == Type.Function) || (tpSym.m_type == Type.Comma)) && (Precedence(tpSym) >= Precedence(sym)))
                        m_postfix.Add(tpSym);
                    else
                        tpStack.Push(tpSym);
                }
                tpStack.Push(sym);
            }
        }
        while (tpStack.Count > 0)
        {
            tpSym = (Symbol)tpStack.Pop();
            m_postfix.Add(tpSym);
        }
    }

    public void EvaluatePostfix()
    {
        Symbol tpSym1, tpSym2, tpResult;
        Stack tpStack = new Stack();
        ArrayList fnParam = new ArrayList();
        m_bError = false;
        foreach (Symbol sym in m_postfix)
        {
            if ((sym.m_type == Type.Value) || (sym.m_type == Type.Variable) || (sym.m_type == Type.Result))
                tpStack.Push(sym);
            else if (sym.m_type == Type.UnaryOperator)
            {
                tpSym1 = (Symbol)tpStack.Pop();
                tpResult = Evaluate(sym, tpSym1);
                if (tpResult.m_type == Type.Error)
                {
                    m_bError = true;
                    m_sErrorDescription = tpResult.m_name;
                    return;
                }
                tpStack.Push(tpResult);
            }
            else if (sym.m_type == Type.Operator)
            {
                tpSym1 = (Symbol)tpStack.Pop();
                tpSym2 = (Symbol)tpStack.Pop();
                tpResult = Evaluate(tpSym2, sym, tpSym1);
                if (tpResult.m_type == Type.Error)
                {
                    m_bError = true;
                    m_sErrorDescription = tpResult.m_name;
                    return;
                }
                tpStack.Push(tpResult);
            }
            else if (sym.m_type == Type.Comma)
            {
                tpStack.Push(sym);
            }
            else if (sym.m_type == Type.Function)
            {
                fnParam.Clear();
                tpSym1 = (Symbol)tpStack.Pop();
                if ((tpSym1.m_type == Type.Value) || (tpSym1.m_type == Type.Variable) || (tpSym1.m_type == Type.Result))
                {
                    tpResult = EvaluateFunction(sym.m_name, tpSym1);
                    if (tpResult.m_type == Type.Error)
                    {
                        m_bError = true;
                        m_sErrorDescription = tpResult.m_name;
                        return;
                    }
                    tpStack.Push(tpResult);
                }
                else if (tpSym1.m_type == Type.Comma)
                {
                    while (tpSym1.m_type == Type.Comma)
                    {
                        tpSym1 = (Symbol)tpStack.Pop();
                        fnParam.Add(tpSym1);
                        tpSym1 = (Symbol)tpStack.Pop();
                    }
                    fnParam.Add(tpSym1);
                    tpResult = EvaluateFunction(sym.m_name, fnParam.ToArray());
                    if (tpResult.m_type == Type.Error)
                    {
                        m_bError = true;
                        m_sErrorDescription = tpResult.m_name;
                        return;
                    }
                    tpStack.Push(tpResult);
                }
                else
                {
                    tpStack.Push(tpSym1);
                    tpResult = EvaluateFunction(sym.m_name);
                    if (tpResult.m_type == Type.Error)
                    {
                        m_bError = true;
                        m_sErrorDescription = tpResult.m_name;
                        return;
                    }
                    tpStack.Push(tpResult);
                }
            }
        }
        if (tpStack.Count == 1)
        {
            tpResult = (Symbol)tpStack.Pop();
            m_result = tpResult.m_value;
        }
    }

    protected int Precedence(Symbol sym)
    {
        switch (sym.m_type)
        {
            case Type.Bracket:
                return 7;
            case Type.Function:
                return 5;
            case Type.UnaryOperator:
                return 4;
            case Type.Comma:
                return 0;
        }
        switch (sym.m_name)
        {
            case "^":
                return 3;
            case "/":
            case "*":
            case "%":
                return 2;
            case "+":
            case "-":
                return 1;
        }
        return -1;
    }

    protected Symbol Evaluate(Symbol opr, Symbol sym1)
    {
        Symbol result;
        result.m_name = opr.m_name + sym1.m_name;
        result.m_type = Type.Result;
        result.m_value = 0;
        switch (opr.m_name)
        {
            case "+":
                result.m_value = sym1.m_value; ;
                break;
            case "-":
                result.m_value = -1 * sym1.m_value;
                break;
            default:
                result.m_type = Type.Error;
                result.m_name = "Undefine unary operator: " + opr.m_name + ".";
                break;
        }
        return result;
    }

    protected Symbol Evaluate(Symbol sym1, Symbol opr, Symbol sym2)
    {
        Symbol result;
        result.m_name = sym1.m_name + opr.m_name + sym2.m_name;
        result.m_type = Type.Result;
        result.m_value = 0;
        switch (opr.m_name)
        {
            case "^":
                result.m_value = DecimalMath.Pow(sym1.m_value, sym2.m_value);
                break;
            case "/":
                {
                    if (sym2.m_value != 0)
                        result.m_value = sym1.m_value / sym2.m_value;
                    else
                    {
                        result.m_name = "Divide by Zero.";
                        result.m_type = Type.Error;
                    }
                    break;
                }
            case "*":
                result.m_value = sym1.m_value * sym2.m_value;
                break;
            case "%":
                result.m_value = sym1.m_value % sym2.m_value;
                break;
            case "+":
                result.m_value = sym1.m_value + sym2.m_value;
                break;
            case "-":
                result.m_value = sym1.m_value - sym2.m_value;
                break;
            default:
                result.m_type = Type.Error;
                result.m_name = "Undefine operator: " + opr.m_name + ".";
                break;
        }
        return result;
    }

    protected Symbol EvaluateFunction(string name, params Object[] args)
    {
        Symbol result;
        result.m_name = "";
        result.m_type = Type.Result;
        result.m_value = 0;
        switch (name)
        {
            case "cos":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Cos(((Symbol)args[0]).m_value * DecimalMath.PI / 180);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "sin":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Sin(((Symbol)args[0]).m_value * DecimalMath.PI / 180);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "tan":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Tan(((Symbol)args[0]).m_value * DecimalMath.PI / 180);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "cosh":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Cosh(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "sinh":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Sinh(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "tanh":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Tanh(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "log":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Log10(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "ln":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Log(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "logn":
                if (args.Length == 2)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + "'" + ((Symbol)args[1]).m_value.ToString() + ")";
                    // log_b(x) = ln(x) / ln(b)
                    result.m_value = DecimalMath.Log(((Symbol)args[0]).m_value) / DecimalMath.Log(((Symbol)args[1]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "sqrt":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Sqrt(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "abs":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Abs(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "acos":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Acos(((Symbol)args[0]).m_value) * 180 / DecimalMath.PI;
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "asin":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Asin(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "atan":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Atan(((Symbol)args[0]).m_value) * 180 / DecimalMath.PI;
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            case "exp":
                if (args.Length == 1)
                {
                    result.m_name = name + "(" + ((Symbol)args[0]).m_value.ToString() + ")";
                    result.m_value = DecimalMath.Exp(((Symbol)args[0]).m_value);
                }
                else
                {
                    result.m_name = "Invalid number of parameters in: " + name + ".";
                    result.m_type = Type.Error;
                }
                break;
            default:
                if (m_defaultFunctionEvaluation != null)
                    result = m_defaultFunctionEvaluation(name, args);
                else
                {
                    result.m_name = "Function: " + name + ", not found.";
                    result.m_type = Type.Error;
                }
                break;
        }
        return result;
    }

    protected bool m_bError = false;
    protected string m_sErrorDescription = "None";
    public decimal m_result = 0;
    public ArrayList m_equation = new ArrayList();
    public ArrayList m_postfix = new ArrayList();
    protected EvaluateFunctionDelegate m_defaultFunctionEvaluation;
}
