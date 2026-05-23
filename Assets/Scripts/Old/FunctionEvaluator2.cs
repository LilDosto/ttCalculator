using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionEvaluator2 : MonoBehaviour
{
    public List<string> Chars = new List<string>();
    List<char> Operators = new List<char> { '+', '-', '*', '/', '^' };

    string Evaluate(string eq)
    {
        int deepestP = FindDeepestParenthesis(eq);
        int closeDeepestP = (deepestP == -1) ? -1 : FindClosedParenthesis(eq, deepestP);

        if (closeDeepestP != -1)
        {
            string newEq = eq.Substring(deepestP + 1, closeDeepestP - deepestP - 1);
            return Evaluate(newEq);
        }
        else
        {
            Chars.Clear();
            string newChar = "";
            bool containsOperator = true;

            while (containsOperator)
            {
                for (int i = 0; i < eq.Length; i++)
                {
                    if (Operators.Contains(eq[i]))
                    {
                        Chars.Add(newChar);
                        newChar = eq[i].ToString();
                        Chars.Add(newChar);
                        newChar = "";
                    }
                    else
                    {
                        if (eq[i] != '(' && eq[i] != ')')
                        {
                            newChar += eq[i];
                        }
                        if (i == eq.Length - 1)
                        {
                            Chars.Add(newChar);
                        }
                    }
                }

                if (Chars.Contains("^"))
                {
                    int pow = (int)Mathf.Pow(GetLeftNumber(eq, FindClosestPower(eq)), GetRightNumber(eq, FindClosestPower(eq)));

                    eq = ReplaceWith(eq, pow.ToString(), GetLeftNumberIndex(eq, FindClosestPower(eq)), GetRightNumberIndex(eq, FindClosestPower(eq)));
                }
                else if (Chars.Contains("*"))
                {
                    int mult = GetLeftNumber(eq, FindClosestMultiplication(eq)) * GetRightNumber(eq, FindClosestMultiplication(eq));

                    eq = ReplaceWith(eq, mult.ToString(), GetLeftNumberIndex(eq, FindClosestMultiplication(eq)), GetRightNumberIndex(eq, FindClosestMultiplication(eq)));
                }
                else if (Chars.Contains("/"))
                {
                    int div = (int)(GetLeftNumber(eq, FindClosestDivision(eq)) / GetRightNumber(eq, FindClosestDivision(eq)));

                    eq = ReplaceWith(eq, div.ToString(), GetLeftNumberIndex(eq, FindClosestDivision(eq)), GetRightNumberIndex(eq, FindClosestDivision(eq)));
                }
                else if (Chars.Contains("+"))
                {
                    int sum = GetLeftNumber(eq, FindClosestAddition(eq)) + GetRightNumber(eq, FindClosestAddition(eq));

                    eq = ReplaceWith(eq, sum.ToString(), GetLeftNumberIndex(eq, FindClosestAddition(eq)), GetRightNumberIndex(eq, FindClosestAddition(eq)));
                }
                else if (Chars.Contains("-"))
                {
                    int sub = GetLeftNumber(eq, FindClosestSubtraction(eq)) - GetRightNumber(eq, FindClosestSubtraction(eq));

                    eq = ReplaceWith(eq, sub.ToString(), GetLeftNumberIndex(eq, FindClosestSubtraction(eq)), GetRightNumberIndex(eq, FindClosestSubtraction(eq)));
                }


                bool contains = false;
                for (int i = 0; i < eq.Length; i++)
                {
                    if (Operators.Contains(eq[i]))
                    {
                        contains = true;
                    }
                }

                containsOperator = contains;
            }


            return eq;
        }
    }

    string ReplaceWith(string replacedStr, string replacingStr, int start, int end)
    {
        string ans = replacedStr;
        ans = ans.Remove(start, end - start);

        string newStr = "";
        for (int i = start; i < ans.Length; i++)
        {
            newStr += ans[i];
        }
        ans = ans.Remove(start);

        ans += replacingStr;
        ans += newStr;

        return ans;
    }

    int GetLeftNumber(string eq, int index)
    {
        string ans = "";
        for (int i = 0; i < index; i++)
        {
            ans += eq[i];
            if (Operators.Contains(eq[i]))
            {
                ans = "";
            }
            Debug.Log(ans + " / " + index + " / " + i);
        }
        Debug.Log(ans + " / " + index);

        return int.Parse(ans);
    }
    int GetLeftNumberIndex(string eq, int index)
    {
        int ans = -1;
        for (int i = index - 1; i >= 0; i--)
        {
            if (Operators.Contains(eq[i]))
            {
                ans = i + 1;
                break;
            }
            if (i == 0)
                ans = 0;
        }
        return ans;
    }
    int GetRightNumber(string eq, int index)
    {
        string ans = "";
        for (int i = index + 1; i < eq.Length; i++)
        {
            if (Operators.Contains(eq[i]))
            {
                break;
            }
            else
            {
                ans += eq[i];
            }
        }
        return int.Parse(ans);
    }
    int GetRightNumberIndex(string eq, int index)
    {
        int ans = -1;
        for (int i = index + 1; i < eq.Length; i++)
        {
            if (Operators.Contains(eq[i]))
            {
                ans = i - 1;
                break;
            }
        }
        return ans;
    }

    int FindDeepestParenthesis(string eq)
    {
        int openedCount = 0, deepestIndex = -1, maxOpenedCount = 0;
        if (eq.Contains("("))
        {
            for (int i = 0; i < eq.Length; i++)
            {
                if (eq[i] == '(')
                {
                    openedCount++;
                }
                else if (eq[i] == ')')
                {
                    openedCount--;
                }

                if (openedCount > maxOpenedCount)
                {
                    maxOpenedCount = openedCount;
                    deepestIndex = i;
                }
            }
        }
        return deepestIndex;
    }

    int FindClosedParenthesis(string eq, int index)
    {
        if (eq[index] == '(')
        {
            int openedCount = 1, result = -1;
            bool counted = false;
            for (int i = index + 1; i < eq.Length; i++)
            {
                if (eq[i] == '(')
                {
                    if (!counted)
                        counted = true;
                    openedCount++;
                }

                else if (eq[i] == ')')
                {
                    if (!counted)
                        counted = true;
                    openedCount--;
                }

                if (counted && openedCount == 0)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        else
        {
            return -1;
        }
    }

    int FindClosestPower(string eq)
    {
        int ans = -1;
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '^')
            {
                ans = i;
                break;
            }
        }
        return ans;
    }

    int FindClosestMultiplication(string eq)
    {
        int ans = -1;
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '*')
            {
                ans = i;
                break;
            }
        }
        return ans;
    }
    int FindClosestDivision(string eq)
    {
        int ans = -1;
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '/')
            {
                ans = i;
                break;
            }
        }
        return ans;
    }
    int FindClosestAddition(string eq)
    {
        int ans = -1;
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '+')
            {
                ans = i;
                break;
            }
        }
        return ans;
    }
    int FindClosestSubtraction(string eq)
    {
        int ans = -1;
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i] == '-')
            {
                ans = i;
                break;
            }
        }
        return ans;
    }
}
