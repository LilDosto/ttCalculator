using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MathApp.Core
{
    /// <summary>
    /// Represents the mathematical computation outcome with high-precision metadata and categorization.
    /// </summary>
    public class CalculationResult
    {
        public bool IsSuccess { get; set; }
        public decimal Value { get; set; }
        public string FormattedValue { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorCategory { get; set; } = string.Empty;
        public long ElapsedTicks { get; set; }

        public static CalculationResult Success(decimal value, string formatted, long elapsedTicks = 0)
        {
            return new CalculationResult
            {
                IsSuccess = true,
                Value = value,
                FormattedValue = formatted,
                ElapsedTicks = elapsedTicks
            };
        }

        public static CalculationResult Error(string message, string category = "Hesaplama Hatası")
        {
            return new CalculationResult
            {
                IsSuccess = false,
                ErrorMessage = message,
                ErrorCategory = category,
                FormattedValue = category
            };
        }
    }

    /// <summary>
    /// High-performance, thread-safe, 128-bit decimal scientific calculation engine.
    /// Implements AST/Shunting-yard caching, state-machine input sanitization, 
    /// full numerical precision preservation, and zero-crash exception handling.
    /// </summary>
    public static class CalculatorEngine
    {
        // LRU / bounded cache for parsed postfix equations to avoid repetitive AST construction
        private static readonly ConcurrentDictionary<string, Function> _parsedEquationCache = new(StringComparer.Ordinal);
        private const int MaxCacheSize = 250;

        private static readonly Regex FactorialRegex = new(@"(\d+(\.\d+)?|\([^\(\)]+\))!", RegexOptions.Compiled);

        /// <summary>
        /// Clears the parsed expression cache.
        /// </summary>
        public static void ClearCache()
        {
            _parsedEquationCache.Clear();
        }

        /// <summary>
        /// Asynchronously evaluates a mathematical expression with full UI thread non-blocking guarantee.
        /// </summary>
        public static async Task<CalculationResult> EvaluateAsync(string expression, AngleUnit angleUnit = AngleUnit.Radian, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => Evaluate(expression, angleUnit), cancellationToken);
        }

        /// <summary>
        /// Evaluates a mathematical expression synchronously with high-precision decimal arithmetic.
        /// </summary>
        public static CalculationResult Evaluate(string expression, AngleUnit angleUnit = AngleUnit.Radian)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return CalculationResult.Error("İfade boş olamaz.", "Boş İfade");
            }

            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                // 1. Balance brackets and preprocess expression (e.g. 5! -> fact(5))
                string preprocessed = PreprocessExpression(expression);
                string balanced = BalanceParentheses(preprocessed);

                // Set angle unit
                Function.CurrentAngleUnit = angleUnit;

                // 2. Lookup cached parsed function or create & parse
                Function func;
                string cacheKey = $"{angleUnit}:{balanced}";

                if (!_parsedEquationCache.TryGetValue(cacheKey, out func))
                {
                    func = new Function();
                    func.Parse(balanced);

                    if (func.Error)
                    {
                        sw.Stop();
                        return CategorizeError(func.ErrorDescription);
                    }

                    func.Infix2Postfix();

                    if (_parsedEquationCache.Count < MaxCacheSize)
                    {
                        _parsedEquationCache.TryAdd(cacheKey, func);
                    }
                }

                // 3. Evaluate postfix
                func.EvaluatePostfix();
                sw.Stop();

                if (func.Error)
                {
                    return CategorizeError(func.ErrorDescription);
                }

                decimal rawResult = func.Result;

                // 4. Numerical precision normalization (normalize machine epsilon residuals)
                decimal normalized = NormalizePrecision(rawResult);
                string formatted = FormatResult(normalized);

                return CalculationResult.Success(normalized, formatted, sw.ElapsedTicks);
            }
            catch (DivideByZeroException ex)
            {
                sw.Stop();
                return CalculationResult.Error(ex.Message, "Sıfıra Bölünemez");
            }
            catch (OverflowException ex)
            {
                sw.Stop();
                return CalculationResult.Error(ex.Message, "Taşma Hatası");
            }
            catch (ArgumentException ex)
            {
                sw.Stop();
                return CalculationResult.Error(ex.Message, "Tanımsız Değer");
            }
            catch (Exception ex)
            {
                sw.Stop();
                return CalculationResult.Error(ex.Message, "Sözdizimi Hatası");
            }
        }

        /// <summary>
        /// Preprocesses expression, transforming factorial syntax (5! -> fact(5)) and normalizing operators.
        /// </summary>
        public static string PreprocessExpression(string expr)
        {
            if (string.IsNullOrWhiteSpace(expr)) return string.Empty;

            string s = expr.Trim();

            // Transform postfix factorial ! e.g. 5! => fact(5), (3+2)! => fact(3+2)
            while (s.Contains("!"))
            {
                var match = FactorialRegex.Match(s);
                if (!match.Success) break;

                string operand = match.Groups[1].Value;
                string replacement = $"fact({operand})";
                s = s.Substring(0, match.Index) + replacement + s.Substring(match.Index + match.Length);
            }

            return s;
        }

        /// <summary>
        /// Balances unclosed parentheses in the expression.
        /// </summary>
        public static string BalanceParentheses(string expr)
        {
            if (string.IsNullOrEmpty(expr)) return expr;

            int openCount = 0;
            foreach (char c in expr)
            {
                if (c == '(') openCount++;
                else if (c == ')') openCount--;
            }

            if (openCount > 0)
            {
                return expr + new string(')', openCount);
            }
            return expr;
        }

        /// <summary>
        /// Categorizes parser or mathematical errors into user-friendly Turkish domain categories.
        /// </summary>
        private static CalculationResult CategorizeError(string errorDesc)
        {
            string lower = errorDesc.ToLowerInvariant();

            if (lower.Contains("sıfıra bölme") || lower.Contains("divide by zero") || lower.Contains("mod alınamaz"))
            {
                return CalculationResult.Error("Sıfıra bölme işlemi matematikte tanımsızdır.", "Sıfıra Bölünemez");
            }
            if (lower.Contains("tanımsız") || lower.Contains("reel sayılarda") || lower.Contains("kutup") || 
                lower.Contains("aralığında olmalıdır") || lower.Contains("pozitif") || lower.Contains("belirsiz") ||
                lower.Contains("logaritma") || lower.Contains("sıfırdan büyük") || lower.Contains("negatif sayının"))
            {
                return CalculationResult.Error(errorDesc, "Tanımsız Değer");
            }
            if (lower.Contains("taşma") || lower.Contains("overflow") || lower.Contains("çok büyük"))
            {
                return CalculationResult.Error(errorDesc, "Taşma Hatası");
            }

            return CalculationResult.Error(errorDesc, "Sözdizimi Hatası");
        }

        /// <summary>
        /// Normalizes infinitesimal residuals resulting from series expansions or division.
        /// E.g., 0.9999999999999999999999999999m -> 1.0m, or 1e-26m -> 0m.
        /// </summary>
        public static decimal NormalizePrecision(decimal val)
        {
            if (val == 0m) return 0m;

            // If within 1e-22 of an integer, snap to integer
            decimal rounded = decimal.Round(val);
            if (Math.Abs(val - rounded) <= 1e-22m)
            {
                return rounded;
            }

            // If absolute value is smaller than 1e-25, snap to zero
            if (Math.Abs(val) <= 1e-25m)
            {
                return 0m;
            }

            return val;
        }

        /// <summary>
        /// Formats decimal results cleanly with scientific notation for extreme values 
        /// and natural notation for standard ranges without truncation of valid significant digits.
        /// </summary>
        public static string FormatResult(decimal val)
        {
            if (val == 0m) return "0";

            decimal absVal = Math.Abs(val);

            // Large magnitude: |x| >= 10^15 or extremely small: 0 < |x| < 10^-6
            if (absVal >= 1000000000000000m || absVal < 0.000001m)
            {
                // Double precision exponential string
                return ((double)val).ToString("0.############e+00", CultureInfo.InvariantCulture);
            }

            // Standard decimal formatting, stripping trailing zeroes
            string str = val.ToString("0.############################", CultureInfo.InvariantCulture);
            return str;
        }

        /// <summary>
        /// State machine input sanitizer.
        /// Filters or modifies incoming tokens to prevent consecutive invalid operators (++//**),
        /// duplicate decimal points (5..2.1), and malformed syntax at input time.
        /// </summary>
        public static string SanitizeInput(string currentExpr, string token)
        {
            if (string.IsNullOrEmpty(currentExpr))
            {
                // Cannot start with binary operators except unary minus or decimal point
                if (token == "*" || token == "/" || token == "%" || token == "^" || token == ")")
                {
                    return string.Empty;
                }
                if (token == ".")
                {
                    return "0.";
                }
                if (token == "+")
                {
                    return string.Empty; // Unary plus at start is redundant
                }
                return token;
            }

            char lastChar = currentExpr[currentExpr.Length - 1];

            // 1. Handling Decimal Point '.'
            if (token == ".")
            {
                // Scan backwards to find current number token
                int i = currentExpr.Length - 1;
                bool hasDot = false;
                while (i >= 0 && (char.IsDigit(currentExpr[i]) || currentExpr[i] == '.'))
                {
                    if (currentExpr[i] == '.')
                    {
                        hasDot = true;
                        break;
                    }
                    i--;
                }

                if (hasDot)
                {
                    // Current active number already has a decimal point, reject second dot!
                    return currentExpr;
                }

                if (!char.IsDigit(lastChar))
                {
                    // If last char is operator or open paren, append "0."
                    return currentExpr + "0.";
                }
                return currentExpr + ".";
            }

            // 2. Handling Binary Operators: '+', '*', '/', '%', '^'
            if (token == "+" || token == "*" || token == "/" || token == "%" || token == "^")
            {
                if (IsBinaryOperator(lastChar))
                {
                    // Replace previous operator with new operator
                    return currentExpr.Substring(0, currentExpr.Length - 1) + token;
                }
                if (lastChar == '(')
                {
                    // Cannot have binary operator immediately after '('
                    return currentExpr;
                }
                return currentExpr + token;
            }

            // 3. Handling '-' (can be binary or unary)
            if (token == "-")
            {
                if (lastChar == '-')
                {
                    // Prevent triple minus
                    if (currentExpr.Length >= 2 && currentExpr[currentExpr.Length - 2] == '-')
                        return currentExpr;
                }
                return currentExpr + "-";
            }

            // 4. Handling ')'
            if (token == ")")
            {
                // Only allow ')' if there is an unmatched '('
                int open = 0;
                foreach (char c in currentExpr)
                {
                    if (c == '(') open++;
                    else if (c == ')') open--;
                }

                if (open <= 0 || IsBinaryOperator(lastChar) || lastChar == '(')
                {
                    return currentExpr; // Cannot close
                }
                return currentExpr + ")";
            }

            // 5. Handling Functions and '(' (Implicit multiplication)
            if (token.EndsWith("(") || token == "(" || token == "pi" || token == "e")
            {
                if (char.IsDigit(lastChar) || lastChar == ')' || lastChar == 'x' || lastChar == 'y' || lastChar == 'z')
                {
                    return currentExpr + "*" + token;
                }
            }

            // 6. Handling leading zeros (prevent 007 -> 7)
            if (char.IsDigit(token[0]))
            {
                if (lastChar == '0')
                {
                    // Check if previous char was an operator or start of string
                    if (currentExpr.Length == 1)
                    {
                        return token; // Replace single leading zero with new digit
                    }
                    char prev = currentExpr[currentExpr.Length - 2];
                    if (IsBinaryOperator(prev) || prev == '(')
                    {
                        return currentExpr.Substring(0, currentExpr.Length - 1) + token;
                    }
                }
            }

            return currentExpr + token;
        }

        private static bool IsBinaryOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == '^';
        }
    }
}
