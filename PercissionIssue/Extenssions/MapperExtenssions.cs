using System.Numerics;

public static class MapperExtenssions
{
    public static CustomDecimal ConvertFromDecimal(this decimal amount)
    {
        int[] bits = decimal.GetBits(amount);

        // استخراج الـ scale (عدد الأرقام بعد الفاصلة)
        int scale = (bits[3] >> 16) & 0x7F;

        // استخراج الإشارة
        bool isNegative = (bits[3] & unchecked((int)0x80000000)) != 0;

        // بناء الرقم الكامل (96-bit integer)
        BigInteger integer =
            ((BigInteger)(uint)bits[2] << 64) |
            ((BigInteger)(uint)bits[1] << 32) |
            (uint)bits[0];

        if (isNegative)
            integer = -integer;

        return new (integer, scale);
    }

    public static decimal ConvertFromBigInteger(this BigInteger amount, int scale)
    {
        if (scale < 0 || scale > 28)
            throw new ArgumentOutOfRangeException(nameof(scale),
                "Scale must be between 0 and 28 for decimal.");

        bool isNegative = amount < 0;
        BigInteger absValue = BigInteger.Abs(amount);

        // decimal يدعم فقط 96-bit
        if (absValue.GetBitLength() > 96)
            throw new OverflowException("Value too large to fit into decimal.");

        // استخراج low/mid/high
        uint low = (uint)(absValue & 0xFFFFFFFF);
        uint mid = (uint)((absValue >> 32) & 0xFFFFFFFF);
        uint high = (uint)((absValue >> 64) & 0xFFFFFFFF);

        return new decimal((int)low, (int)mid, (int)high, isNegative, (byte)scale);
    }
}


