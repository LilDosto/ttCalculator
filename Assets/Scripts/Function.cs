using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public enum Type { Variable, Value, UnaryOperator, Operator, Function, Result, Bracket, Comma, Error }

public struct Symbol
{
    public string m_name;
    public decimal m_value;
    public Type m_type;
    public int m_argCount;
    public override string ToString()
    {
        return m_name;
    }
}

public delegate Symbol EvaluateFunctionDelegate(string name, params Symbol[] args);

public class Function
{
    private static readonly HashSet<string> KnownFunctions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "sin", "cos", "tan", "cot", "cosh", "sinh", "tanh",
        "log", "ln", "logn", "sqrt", "cbrt", "abs",
        "acos", "asin", "atan", "atan2", "exp",
        "max", "min", "floor", "ceil", "round", "sign"
    };

    public static bool UseRadians { get; set; } = true;

    public decimal Result => m_result;
    public ArrayList Equation => (ArrayList)m_equation.Clone();
    public ArrayList Postfix => (ArrayList)m_postfix.Clone();

    public EvaluateFunctionDelegate DefaultFunctionEvaluation
    {
        set => m_defaultFunctionEvaluation = value;
    }

    public bool Error => m_bError;
    public string ErrorDescription => m_sErrorDescription;

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

    public void SetVariable(string name, decimal value)
    {
        for (int i = 0; i < m_postfix.Count; i++)
        {
            Symbol s = (Symbol)m_postfix[i];
            if (s.m_type == Type.Variable && string.Equals(s.m_name, name, StringComparison.OrdinalIgnoreCase))
            {
                s.m_value = value;
                m_postfix[i] = s;
            }
        }
    }

    public void SetVariables(Dictionary<string, decimal> values)
    {
        for (int i = 0; i < m_postfix.Count; i++)
        {
            Symbol s = (Symbol)m_postfix[i];
            if (s.m_type == Type.Variable && values.TryGetValue(s.m_name, out decimal val))
            {
                s.m_value = val;
                m_postfix[i] = s;
            }
        }
    }

    public Function()
    {
    }

    private void AddSymbolWithImplicitMultiplication(Symbol sym)
    {
        if (m_equation.Count > 0)
        {
            Symbol prev = (Symbol)m_equation[m_equation.Count - 1];
            bool prevIsOperandOrClose = prev.m_type == Type.Value ||
                                        prev.m_type == Type.Variable ||
                                        prev.m_name == ")" || prev.m_name == "]" || prev.m_name == "}";

            bool currIsOperandOrOpen = sym.m_type == Type.Value ||
                                       sym.m_type == Type.Variable ||
                                       sym.m_type == Type.Function ||
                                       sym.m_name == "(" || sym.m_name == "[" || sym.m_name == "{";

            if (prevIsOperandOrClose && currIsOperandOrOpen)
            {
                Symbol mulSym = new Symbol
                {
                    m_name = "*",
                    m_value = 0,
                    m_type = Type.Operator
                };
                m_equation.Add(mulSym);
            }
        }
        m_equation.Add(sym);
    }

    public void Parse(string equation)
    {
        int state = 1;
        string temp = "";
        Symbol ctSymbol = new Symbol();

        m_bError = false;
        m_sErrorDescription = "None";

        m_equation.Clear();
        m_postfix.Clear();

        if (string.IsNullOrWhiteSpace(equation))
        {
            m_bError = true;
            m_sErrorDescription = "Denklem boş olamaz.";
            return;
        }

        // We process whitespace as delimiter without deleting it globally
        for (int i = 0; i < equation.Length; i++)
        {
            char ch = equation[i];
            switch (state)
            {
                case 1: // Neutral
                    if (char.IsWhiteSpace(ch))
                    {
                        continue;
                    }
                    if (char.IsDigit(ch) || ch == '.')
                    {
                        state = 2;
                        temp += ch;
                    }
                    else if (char.IsLetter(ch))
                    {
                        state = 3;
                        temp += ch;
                    }
                    else
                    {
                        ctSymbol = ClassifyNonAlphanumeric(ch.ToString());
                        AddSymbolWithImplicitMultiplication(ctSymbol);
                    }
                    break;

                case 2: // Reading Number
                    if (char.IsDigit(ch) || ch == '.')
                    {
                        temp += ch;
                    }
                    else if (char.IsWhiteSpace(ch))
                    {
                        if (!EmitNumberSymbol(temp)) return;
                        temp = "";
                        state = 1;
                    }
                    else
                    {
                        if (!EmitNumberSymbol(temp)) return;
                        temp = "";

                        if (char.IsLetter(ch))
                        {
                            state = 3;
                            temp += ch;
                        }
                        else
                        {
                            state = 1;
                            ctSymbol = ClassifyNonAlphanumeric(ch.ToString());
                            AddSymbolWithImplicitMultiplication(ctSymbol);
                        }
                    }
                    break;

                case 3: // Reading Identifier
                    if (char.IsLetterOrDigit(ch))
                    {
                        temp += ch;
                    }
                    else if (char.IsWhiteSpace(ch))
                    {
                        // Lookahead to see if next non-whitespace is '('
                        bool nextIsOpen = false;
                        for (int k = i + 1; k < equation.Length; k++)
                        {
                            if (!char.IsWhiteSpace(equation[k]))
                            {
                                nextIsOpen = (equation[k] == '(' || equation[k] == '[' || equation[k] == '{');
                                break;
                            }
                        }
                        EmitIdentifierSymbol(temp, nextIsOpen);
                        temp = "";
                        state = 1;
                    }
                    else
                    {
                        state = 1;
                        EmitIdentifierSymbol(temp, ch == '(' || ch == '[' || ch == '{');
                        temp = "";

                        ctSymbol = ClassifyNonAlphanumeric(ch.ToString());
                        AddSymbolWithImplicitMultiplication(ctSymbol);
                    }
                    break;
            }
        }

        if (temp != "")
        {
            if (state == 2)
            {
                if (!EmitNumberSymbol(temp)) return;
            }
            else
            {
                EmitIdentifierSymbol(temp, false);
            }
        }
    }

    private bool EmitNumberSymbol(string temp)
    {
        if (!decimal.TryParse(temp, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
        {
            m_bError = true;
            m_sErrorDescription = "Geçersiz sayı formatı: " + temp;
            return false;
        }

        Symbol sym = new Symbol
        {
            m_name = temp,
            m_value = val,
            m_type = Type.Value
        };
        AddSymbolWithImplicitMultiplication(sym);
        return true;
    }

    private void EmitIdentifierSymbol(string temp, bool isNextOpenParen)
    {
        Symbol sym = new Symbol
        {
            m_name = temp,
            m_value = 0
        };

        string lower = temp.ToLowerInvariant();
        if (lower == "pi")
        {
            sym.m_value = DecimalMath.PI;
            sym.m_type = Type.Value;
        }
        else if (lower == "e")
        {
            sym.m_value = DecimalMath.E;
            sym.m_type = Type.Value;
        }
        else if (isNextOpenParen)
        {
            if (temp.Length == 1 && (temp == "x" || temp == "y" || temp == "z" || temp == "t" || temp == "u" || temp == "v"))
            {
                sym.m_type = Type.Variable;
            }
            else
            {
                sym.m_type = Type.Function;
            }
        }
        else
        {
            sym.m_type = Type.Variable;
        }
        AddSymbolWithImplicitMultiplication(sym);
    }

    private Symbol ClassifyNonAlphanumeric(string token)
    {
        Symbol ctSymbol = new Symbol
        {
            m_name = token,
            m_value = 0
        };

        switch (token)
        {
            case ",":
            case ";":
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
                {
                    ctSymbol.m_type = Type.UnaryOperator;
                }
                else
                {
                    Symbol prev = (Symbol)m_equation[m_equation.Count - 1];
                    if (prev.m_type == Type.Value || prev.m_type == Type.Variable ||
                        prev.m_name == ")" || prev.m_name == "]" || prev.m_name == "}")
                    {
                        ctSymbol.m_type = Type.Operator;
                    }
                    else
                    {
                        ctSymbol.m_type = Type.UnaryOperator;
                    }
                }
                break;
        }
        return ctSymbol;
    }

    public void Infix2Postfix()
    {
        m_postfix.Clear();
        Symbol tpSym;
        Stack tpStack = new Stack();
        Stack<int> fnArgCounts = new Stack<int>();

        foreach (Symbol sym in m_equation)
        {
            if ((sym.m_type == Type.Value) || (sym.m_type == Type.Variable))
                m_postfix.Add(sym);
            else if (sym.m_type == Type.Function)
            {
                tpStack.Push(sym);
                fnArgCounts.Push(1);
            }
            else if ((sym.m_name == "(") || (sym.m_name == "[") || (sym.m_name == "{"))
                tpStack.Push(sym);
            else if (sym.m_type == Type.Comma)
            {
                while (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Peek();
                    if (tpSym.m_name == "(" || tpSym.m_name == "[" || tpSym.m_name == "{")
                        break;
                    m_postfix.Add(tpStack.Pop());
                }
                if (fnArgCounts.Count > 0)
                {
                    int c = fnArgCounts.Pop();
                    fnArgCounts.Push(c + 1);
                }
            }
            else if ((sym.m_name == ")") || (sym.m_name == "]") || (sym.m_name == "}"))
            {
                if (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Pop();
                    while ((tpSym.m_name != "(") && (tpSym.m_name != "[") && (tpSym.m_name != "{"))
                    {
                        m_postfix.Add(tpSym);
                        if (tpStack.Count == 0) break;
                        tpSym = (Symbol)tpStack.Pop();
                    }

                    if (tpStack.Count > 0 && ((Symbol)tpStack.Peek()).m_type == Type.Function)
                    {
                        Symbol fn = (Symbol)tpStack.Pop();
                        fn.m_argCount = fnArgCounts.Count > 0 ? fnArgCounts.Pop() : 1;
                        m_postfix.Add(fn);
                    }
                }
            }
            else if (sym.m_type == Type.UnaryOperator)
            {
                // Unary operators (like unary - or +) modify the operand to their right.
                // They should never pop binary operators (^, *, /, +, -) from the stack.
                tpStack.Push(sym);
            }
            else if (sym.m_name == "^")
            {
                // Right-associative operator ^
                while (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Peek();
                    if ((tpSym.m_type == Type.Operator || tpSym.m_type == Type.Function) &&
                        Precedence(tpSym) > Precedence(sym))
                    {
                        m_postfix.Add(tpStack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                tpStack.Push(sym);
            }
            else
            {
                while (tpStack.Count > 0)
                {
                    tpSym = (Symbol)tpStack.Peek();
                    if (((tpSym.m_type == Type.UnaryOperator) || (tpSym.m_type == Type.Operator) || (tpSym.m_type == Type.Function)) &&
                        (Precedence(tpSym) >= Precedence(sym)))
                    {
                        m_postfix.Add(tpStack.Pop());
                    }
                    else
                    {
                        break;
                    }
                }
                tpStack.Push(sym);
            }
        }
        while (tpStack.Count > 0)
        {
            tpSym = (Symbol)tpStack.Pop();
            if (tpSym.m_type == Type.Function && tpSym.m_argCount == 0)
            {
                tpSym.m_argCount = fnArgCounts.Count > 0 ? fnArgCounts.Pop() : 1;
            }
            m_postfix.Add(tpSym);
        }
    }

    public void EvaluatePostfix()
    {
        Symbol tpSym1, tpSym2, tpResult;
        Stack tpStack = new Stack();
        m_bError = false;
        foreach (Symbol sym in m_postfix)
        {
            if ((sym.m_type == Type.Value) || (sym.m_type == Type.Variable) || (sym.m_type == Type.Result))
                tpStack.Push(sym);
            else if (sym.m_type == Type.UnaryOperator)
            {
                if (tpStack.Count < 1)
                {
                    m_bError = true;
                    m_sErrorDescription = "Sözdizimi hatası (eksik operand).";
                    return;
                }
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
                if (tpStack.Count < 2)
                {
                    m_bError = true;
                    m_sErrorDescription = "Sözdizimi hatası (eksik operand).";
                    return;
                }
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
            else if (sym.m_type == Type.Function)
            {
                int count = sym.m_argCount > 0 ? sym.m_argCount : 1;
                if (tpStack.Count < count)
                {
                    m_bError = true;
                    m_sErrorDescription = $"Fonksiyon '{sym.m_name}' için argüman eksik.";
                    return;
                }

                Symbol[] fnParam = new Symbol[count];
                for (int i = count - 1; i >= 0; i--)
                {
                    fnParam[i] = (Symbol)tpStack.Pop();
                }

                tpResult = EvaluateFunction(sym.m_name, fnParam);
                if (tpResult.m_type == Type.Error)
                {
                    m_bError = true;
                    m_sErrorDescription = tpResult.m_name;
                    return;
                }
                tpStack.Push(tpResult);
            }
        }
        if (tpStack.Count == 1)
        {
            tpResult = (Symbol)tpStack.Pop();
            m_result = tpResult.m_value;
        }
        else if (tpStack.Count > 1)
        {
            m_bError = true;
            m_sErrorDescription = "İfade tam çözümlenemedi (fazla terim).";
        }
    }

    protected int Precedence(Symbol sym)
    {
        switch (sym.m_type)
        {
            case Type.Bracket:
                return 7;
            case Type.Function:
                return 6;
            case Type.UnaryOperator:
                return 3;
            case Type.Comma:
                return 0;
        }
        switch (sym.m_name)
        {
            case "^":
                return 5;
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
        Symbol result = default;
        result.m_name = opr.m_name + sym1.m_name;
        result.m_type = Type.Result;
        result.m_value = 0;
        switch (opr.m_name)
        {
            case "+":
                result.m_value = sym1.m_value;
                break;
            case "-":
                result.m_value = -1 * sym1.m_value;
                break;
            default:
                result.m_type = Type.Error;
                result.m_name = "Tanımsız tekli operatör: " + opr.m_name;
                break;
        }
        return result;
    }

    protected Symbol Evaluate(Symbol sym1, Symbol opr, Symbol sym2)
    {
        Symbol result = default;
        result.m_name = sym1.m_name + opr.m_name + sym2.m_name;
        result.m_type = Type.Result;
        result.m_value = 0;
        try
        {
            switch (opr.m_name)
            {
                case "^":
                    result.m_value = DecimalMath.Pow(sym1.m_value, sym2.m_value);
                    break;
                case "/":
                    if (sym2.m_value != 0)
                        result.m_value = sym1.m_value / sym2.m_value;
                    else
                    {
                        result.m_name = "Sıfıra bölme hatası (Divide by Zero).";
                        result.m_type = Type.Error;
                    }
                    break;
                case "*":
                    result.m_value = sym1.m_value * sym2.m_value;
                    break;
                case "%":
                    if (sym2.m_value != 0)
                        result.m_value = sym1.m_value % sym2.m_value;
                    else
                    {
                        result.m_name = "Sıfıra göre mod alınamaz.";
                        result.m_type = Type.Error;
                    }
                    break;
                case "+":
                    result.m_value = sym1.m_value + sym2.m_value;
                    break;
                case "-":
                    result.m_value = sym1.m_value - sym2.m_value;
                    break;
                default:
                    result.m_type = Type.Error;
                    result.m_name = "Tanımsız operatör: " + opr.m_name;
                    break;
            }
        }
        catch (Exception ex)
        {
            result.m_type = Type.Error;
            result.m_name = ex.Message;
        }
        return result;
    }

    protected Symbol EvaluateFunction(string name, params Symbol[] args)
    {
        Symbol result = default;
        result.m_name = "";
        result.m_type = Type.Result;
        result.m_value = 0;

        string lowerName = name.ToLowerInvariant();
        decimal angleFactor = UseRadians ? 1.0m : (DecimalMath.PI / 180.0m);
        decimal invAngleFactor = UseRadians ? 1.0m : (180.0m / DecimalMath.PI);

        try
        {
            switch (lowerName)
            {
                case "cos":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Cos(v * angleFactor);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "sin":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Sin(v * angleFactor);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "tan":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Tan(v * angleFactor);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "cot":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        decimal sinVal = DecimalMath.Sin(v * angleFactor);
                        if (DecimalMath.Abs(sinVal) <= 1e-25m)
                        {
                            result.m_name = "Cot(x) bu noktada tanımsızdır (sin(x) = 0).";
                            result.m_type = Type.Error;
                        }
                        else
                        {
                            result.m_value = DecimalMath.Cos(v * angleFactor) / sinVal;
                        }
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "cosh":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Cosh(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "sinh":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Sinh(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "tanh":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Tanh(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "log":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Log10(v);
                    }
                    else if (args.Length == 2)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        decimal b = ((Symbol)args[1]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + "," + b.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Log(v) / DecimalMath.Log(b);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "ln":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Log(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "logn":
                    if (args.Length == 2)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        decimal b = ((Symbol)args[1]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + "," + b.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Log(v) / DecimalMath.Log(b);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "sqrt":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Sqrt(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "cbrt":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = v < 0 ? -DecimalMath.Pow(-v, 1m / 3m) : DecimalMath.Pow(v, 1m / 3m);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "abs":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Abs(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "acos":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Acos(v) * invAngleFactor;
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "asin":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Asin(v) * invAngleFactor;
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "atan":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Atan(v) * invAngleFactor;
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "atan2":
                    if (args.Length == 2)
                    {
                        decimal y = ((Symbol)args[0]).m_value;
                        decimal x = ((Symbol)args[1]).m_value;
                        result.m_name = name + "(" + y.ToString(CultureInfo.InvariantCulture) + "," + x.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Atan2(y, x) * invAngleFactor;
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: atan2(y, x)";
                        result.m_type = Type.Error;
                    }
                    break;
                case "exp":
                    if (args.Length == 1)
                    {
                        decimal v = ((Symbol)args[0]).m_value;
                        result.m_name = name + "(" + v.ToString(CultureInfo.InvariantCulture) + ")";
                        result.m_value = DecimalMath.Exp(v);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
                case "max":
                    if (args.Length == 2)
                    {
                        decimal a = ((Symbol)args[0]).m_value;
                        decimal b = ((Symbol)args[1]).m_value;
                        result.m_value = Math.Max(a, b);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: max(a, b)";
                        result.m_type = Type.Error;
                    }
                    break;
                case "min":
                    if (args.Length == 2)
                    {
                        decimal a = ((Symbol)args[0]).m_value;
                        decimal b = ((Symbol)args[1]).m_value;
                        result.m_value = Math.Min(a, b);
                    }
                    else
                    {
                        result.m_name = "Geçersiz parametre sayısı: min(a, b)";
                        result.m_type = Type.Error;
                    }
                    break;
                case "floor":
                    if (args.Length == 1)
                    {
                        result.m_value = decimal.Floor(((Symbol)args[0]).m_value);
                    }
                    break;
                case "ceil":
                    if (args.Length == 1)
                    {
                        result.m_value = decimal.Ceiling(((Symbol)args[0]).m_value);
                    }
                    break;
                case "round":
                    if (args.Length == 1)
                    {
                        result.m_value = decimal.Round(((Symbol)args[0]).m_value);
                    }
                    break;
                case "sign":
                    if (args.Length == 1)
                    {
                        result.m_value = DecimalMath.Sign(((Symbol)args[0]).m_value);
                    }
                    break;
                default:
                    if (m_defaultFunctionEvaluation != null)
                        result = m_defaultFunctionEvaluation(name, args);
                    else
                    {
                        result.m_name = "Fonksiyon bulunamadı: " + name;
                        result.m_type = Type.Error;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            result.m_name = ex.Message;
            result.m_type = Type.Error;
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
