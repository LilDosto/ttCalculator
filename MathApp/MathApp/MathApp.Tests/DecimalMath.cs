using System;

/// <summary>
/// Analogy of System.Math class for decimal types with high precision.
/// </summary>
public static class DecimalMath
{
    /// <summary>
    /// represents PI
    /// </summary>
    public const decimal PI = 3.14159265358979323846264338327950288419716939937510M;

    /// <summary>
    /// represents machine epsilon for decimal comparisons
    /// </summary>
    public const decimal Epsilon = 0.0000000000000000001M;

    /// <summary>
    /// represents 2*PI
    /// </summary>
    public const decimal PIx2 = 6.28318530717958647692528676655900576839433879875021M;

    /// <summary>
    /// represents E
    /// </summary>
    public const decimal E = 2.7182818284590452353602874713526624977572470936999595749M;

    /// <summary>
    /// represents PI/2
    /// </summary>
    public const decimal PIdiv2 = 1.570796326794896619231321691639751442098584699687552910487M;

    /// <summary>
    /// represents PI/4
    /// </summary>
    public const decimal PIdiv4 = 0.785398163397448309615660845819875721049292349843776455243M;

    /// <summary>
    /// represents 1.0/E
    /// </summary>
    private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678M;

    /// <summary>
    /// log(10,E) factor
    /// </summary>
    private const decimal Log10Inv = 0.434294481903251827651128918916605082294397005803666566114M;

    /// <summary>
    /// Zero
    /// </summary>
    public const decimal Zero = 0.0M;

    /// <summary>
    /// One
    /// </summary>
    public const decimal One = 1.0M;

    /// <summary>
    /// Represents 0.5M
    /// </summary>
    private const decimal Half = 0.5M;

    /// <summary>
    /// Max iterations count in Taylor series
    /// </summary>
    private const int MaxIteration = 100;

    /// <summary>
    /// Analogy of Math.Exp method with full precision across all ranges
    /// </summary>
    public static decimal Exp(decimal x)
    {
        if (x == Zero) return One;
        if (x > 66.5m)
        {
            throw new OverflowException("Exp argümanı çok büyük (decimal taşması).");
        }
        if (x < -70m)
        {
            return Zero;
        }
        if (x < Zero)
        {
            return One / Exp(-x);
        }

        int count = decimal.ToInt32(decimal.Truncate(x));
        x -= count;

        int iteration = 1;
        decimal result = One;
        decimal factorial = One;
        decimal cachedResult;
        do
        {
            cachedResult = result;
            factorial *= x / iteration++;
            result += factorial;
        } while (cachedResult != result && iteration < MaxIteration);

        if (count == 0) return result;
        return result * PowerN(E, count);
    }

    /// <summary>
    /// Analogy of Math.Pow method
    /// </summary>
    public static decimal Pow(decimal value, decimal pow)
    {
        if (pow == Zero) return One;
        if (pow == One) return value;
        if (value == One) return One;

        if (value == Zero)
        {
            if (pow > Zero) return Zero;
            throw new DivideByZeroException("Geçersiz İşlem: Sıfır tabanının negatif kuvveti alınamaz.");
        }

        if (pow == -One) return One / value;

        bool isPowerInteger = IsInteger(pow);
        if (value < Zero && !isPowerInteger)
        {
            // Check if power is reciprocal of an odd integer, e.g. 1/3, 1/5
            decimal invPow = One / pow;
            if (IsInteger(invPow))
            {
                int q = (int)decimal.Round(invPow);
                if (q % 2 != 0)
                {
                    return -Exp(pow * Log(-value));
                }
            }
            throw new ArgumentException("Geçersiz İşlem: Negatif tabanın çift kökü reel sayılarda tanımsızdır.");
        }

        if (isPowerInteger)
        {
            int powerInt = (int)decimal.Round(pow);
            return PowerN(value, powerInt);
        }

        return Exp(pow * Log(value));
    }

    private static bool IsInteger(decimal value)
    {
        var rounded = decimal.Round(value);
        return Abs(value - rounded) <= Epsilon;
    }

    /// <summary>
    /// Power to the integer value
    /// </summary>
    public static decimal PowerN(decimal value, int power)
    {
        if (power == 0) return One;
        if (power < 0)
        {
            value = One / value;
            power = -power;
        }

        int q = power;
        decimal prod = One;
        decimal current = value;
        while (q > 0)
        {
            if (q % 2 == 1)
            {
                prod = current * prod;
                q--;
            }
            current *= current;
            q >>= 1;
        }
        return prod;
    }

    /// <summary>
    /// Analogy of Math.Log10
    /// </summary>
    public static decimal Log10(decimal x)
    {
        return Log(x) * Log10Inv;
    }

    /// <summary>
    /// Analogy of Math.Log
    /// </summary>
    public static decimal Log(decimal x)
    {
        if (x <= Zero)
        {
            throw new ArgumentException("Logaritma argümanı sıfırdan büyük olmalıdır.");
        }
        int count = 0;
        while (x >= One)
        {
            x *= Einv;
            count++;
        }
        while (x <= Einv)
        {
            x *= E;
            count--;
        }
        x--;
        if (x == Zero) return count;

        decimal result = Zero;
        int iteration = 0;
        decimal y = One;
        decimal cacheResult = result - One;
        while (cacheResult != result && iteration < MaxIteration)
        {
            iteration++;
            cacheResult = result;
            y *= -x;
            result += y / iteration;
        }
        return count - result;
    }

    /// <summary>
    /// Analogy of Math.Sin with direct Taylor series (accurate to 28 decimal digits even for small x)
    /// </summary>
    public static decimal Sin(decimal x)
    {
        TruncateToPeriodicInterval(ref x);

        if (x > PI) x -= PIx2;
        else if (x < -PI) x += PIx2;

        if (x > PIdiv2) x = PI - x;
        else if (x < -PIdiv2) x = -PI - x;

        decimal x2 = x * x;
        decimal term = x;
        decimal sum = x;
        decimal cachedSum;
        for (int i = 1; i < MaxIteration; i++)
        {
            cachedSum = sum;
            term *= -x2 / ((2 * i) * (2 * i + 1));
            sum += term;
            if (sum == cachedSum) break;
        }
        return sum;
    }

    /// <summary>
    /// Analogy of Math.Cos
    /// </summary>
    public static decimal Cos(decimal x)
    {
        TruncateToPeriodicInterval(ref x);

        if (x > PI) x -= PIx2;
        else if (x < -PI) x += PIx2;

        if (x > PIdiv2) return -Cos(PI - x);
        if (x < -PIdiv2) return -Cos(-PI - x);

        decimal x2 = x * x;
        decimal term = One;
        decimal sum = One;
        decimal cachedSum;
        for (int i = 1; i < MaxIteration; i++)
        {
            cachedSum = sum;
            term *= -x2 / ((2 * i - 1) * (2 * i));
            sum += term;
            if (sum == cachedSum) break;
        }
        return sum;
    }

    /// <summary>
    /// Analogy of Math.Tan
    /// </summary>
    public static decimal Tan(decimal x)
    {
        decimal cos = Cos(x);
        if (Abs(cos) <= 1e-25m) throw new ArgumentException("Tan(x) bu noktada tanımsızdır (cos(x) = 0).");
        return Sin(x) / cos;
    }

    private static void TruncateToPeriodicInterval(ref decimal x)
    {
        if (x >= PIx2 || x <= -PIx2)
        {
            x = x % PIx2;
        }
    }

    /// <summary>
    /// Analogy of Math.Sqrt (Safe against infinite 1-ULP oscillation)
    /// </summary>
    public static decimal Sqrt(decimal x, decimal epsilon = Zero)
    {
        if (x < Zero) throw new ArgumentException("Negatif sayının karekökü reel sayılarda alınamaz.");
        if (x == Zero) return Zero;

        decimal current = (decimal)Math.Sqrt((double)x);
        if (current == Zero) current = x * Half;

        decimal previous = Zero;
        int maxSteps = 25;
        for (int step = 0; step < maxSteps; step++)
        {
            decimal next = (current + x / current) * Half;
            if (next == current || next == previous || Abs(next - current) <= epsilon)
            {
                return next;
            }
            previous = current;
            current = next;
        }
        return current;
    }

    /// <summary>
    /// Analogy of Math.Sinh
    /// </summary>
    public static decimal Sinh(decimal x)
    {
        if (x < Zero) return -Sinh(-x);

        if (Abs(x) < 0.1m)
        {
            // Taylor series for small x to avoid loss of significance in (e^x - e^-x)/2
            decimal x2 = x * x;
            decimal term = x;
            decimal sum = x;
            decimal cachedSum;
            for (int i = 1; i < MaxIteration; i++)
            {
                cachedSum = sum;
                term *= x2 / ((2 * i) * (2 * i + 1));
                sum += term;
                if (sum == cachedSum) break;
            }
            return sum;
        }

        var y = Exp(x);
        var yy = One / y;
        return (y - yy) * Half;
    }

    /// <summary>
    /// Analogy of Math.Cosh
    /// </summary>
    public static decimal Cosh(decimal x)
    {
        if (x < Zero) x = -x;
        var y = Exp(x);
        var yy = One / y;
        return (y + yy) * Half;
    }

    /// <summary>
    /// Analogy of Math.Sign
    /// </summary>
    public static int Sign(decimal x)
    {
        return x < Zero ? -1 : (x > Zero ? 1 : 0);
    }

    /// <summary>
    /// Analogy of Math.Tanh
    /// </summary>
    public static decimal Tanh(decimal x)
    {
        if (x > 35m) return One;
        if (x < -35m) return -One;

        var y = Exp(2m * x);
        return (y - One) / (y + One);
    }

    /// <summary>
    /// Analogy of Math.Abs
    /// </summary>
    public static decimal Abs(decimal x)
    {
        return x <= Zero ? -x : x;
    }

    /// <summary>
    /// Analogy of Math.Asin
    /// </summary>
    public static decimal Asin(decimal x)
    {
        if (x > One || x < -One)
        {
            throw new ArgumentException("Asin argümanı [-1, 1] aralığında olmalıdır.");
        }
        if (x == Zero) return Zero;
        if (x == One) return PIdiv2;
        if (x == -One) return -PIdiv2;
        if (x < Zero) return -Asin(-x);

        var newX = One - 2 * x * x;
        if (Abs(x) > Abs(newX))
        {
            var t = Asin(newX);
            return Half * (PIdiv2 - t);
        }

        decimal y = Zero;
        decimal result = x;
        decimal cachedResult;
        int i = 1;
        y += result;
        decimal xx = x * x;
        do
        {
            cachedResult = result;
            result *= xx * (One - Half / i);
            y += result / ((i << 1) + 1);
            i++;
        } while (cachedResult != result && i < MaxIteration);
        return y;
    }

    /// <summary>
    /// Analogy of Math.Atan (safe against overflow for large x)
    /// </summary>
    public static decimal Atan(decimal x)
    {
        if (x == Zero) return Zero;
        if (x == One) return PIdiv4;
        if (x == -One) return -PIdiv4;

        if (x > 100000000000000m) return PIdiv2;
        if (x < -100000000000000m) return -PIdiv2;

        if (x > One)
        {
            return PIdiv2 - Atan(One / x);
        }
        if (x < -One)
        {
            return -PIdiv2 - Atan(One / x);
        }

        return Asin(x / Sqrt(One + x * x));
    }

    /// <summary>
    /// Analogy of Math.Acos
    /// </summary>
    public static decimal Acos(decimal x)
    {
        if (x > One || x < -One)
        {
            throw new ArgumentException("Acos argümanı [-1, 1] aralığında olmalıdır.");
        }
        if (x == Zero) return PIdiv2;
        if (x == One) return Zero;
        if (x == -One) return PI;
        if (x < Zero) return PI - Acos(-x);
        return PIdiv2 - Asin(x);
    }

    /// <summary>
    /// Analogy of Math.Atan2
    /// </summary>
    public static decimal Atan2(decimal y, decimal x)
    {
        if (x > Zero)
        {
            return Atan(y / x);
        }
        if (x < Zero && y >= Zero)
        {
            return Atan(y / x) + PI;
        }
        if (x < Zero && y < Zero)
        {
            return Atan(y / x) - PI;
        }
        if (x == Zero && y > Zero)
        {
            return PIdiv2;
        }
        if (x == Zero && y < Zero)
        {
            return -PIdiv2;
        }
        throw new ArgumentException("Geçersiz Atan2 argümanları (0, 0).");
    }
}
