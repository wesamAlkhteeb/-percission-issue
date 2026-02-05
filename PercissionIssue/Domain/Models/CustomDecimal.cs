using System.Numerics;

public class CustomDecimal
{
    public BigInteger Number { get; }
    public int Scale { get; }

    public CustomDecimal(BigInteger number, int scale)
    {
        Number = number;
        Scale = scale;
    }

    // --- عملية الجمع ---
    public static CustomDecimal operator +(CustomDecimal a, CustomDecimal b)
    {
        int maxScale = Math.Max(a.Scale, b.Scale);
        BigInteger n1 = a.Number * BigInteger.Pow(10, maxScale - a.Scale);
        BigInteger n2 = b.Number * BigInteger.Pow(10, maxScale - b.Scale);

        return new CustomDecimal(n1 + n2, maxScale);
    }

    // --- عملية الطرح ---
    public static CustomDecimal operator -(CustomDecimal a, CustomDecimal b)
    {
        int maxScale = Math.Max(a.Scale, b.Scale);
        BigInteger n1 = a.Number * BigInteger.Pow(10, maxScale - a.Scale);
        BigInteger n2 = b.Number * BigInteger.Pow(10, maxScale - b.Scale);

        return new CustomDecimal(n1 - n2, maxScale);
    }

    // --- عملية الضرب ---
    public static CustomDecimal operator *(CustomDecimal a, CustomDecimal b)
    {
        // في الضرب نجمع الـ Scales
        return new CustomDecimal(a.Number * b.Number, a.Scale + b.Scale);
    }

    // --- عملية القسمة ---
    public static CustomDecimal operator /(CustomDecimal a, CustomDecimal b)
    {
        // نحدد دقة افتراضية للقسمة (مثلاً 18 خانة) لتجنب الأرقام الدورية اللانهائية
        int targetPrecision = 100;
        int scaleAdjustment = targetPrecision + b.Scale - a.Scale;

        BigInteger numerator = a.Number * BigInteger.Pow(10, scaleAdjustment);
        BigInteger result = numerator / b.Number;

        return new CustomDecimal(result, targetPrecision);
    }

    // ميثود مساعدة للعرض
    public override string ToString()
    {
        string s = Number.ToString();
        if (Scale <= 0) return s;

        if (s.Length <= Scale)
            s = s.PadLeft(Scale + 1, '0');

        return s.Insert(s.Length - Scale, ".");
    }
}
