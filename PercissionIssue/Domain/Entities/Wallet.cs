using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvgRate.Reconciliation.Demo.Domain.Entities;

public class Wallet
{
    private Wallet(BigInteger amount, int scaleAmount, BigInteger rate, int scaleRate)
    {
        Amount = amount;
        ScaleAmount = scaleAmount;
        Rate = rate;
        ScaleRate = scaleRate;
    }

    public int Id { get; private set; }
    public BigInteger Amount { get; private set; }
    public int ScaleAmount { get; private set; }

    public BigInteger Rate { get; private set; }
    public int ScaleRate { get; private set; }


    public static Wallet Create(decimal amount, decimal rate)
    {
        CustomDecimal amountDecimal = amount.ConvertFromDecimal();

        CustomDecimal rateDecimal = rate.ConvertFromDecimal();
        
        return new(amountDecimal.Number, amountDecimal.Scale, rateDecimal.Number, rateDecimal.Scale);
    }

    internal void UpdateRate((BigInteger number, int scale) rate)
    {
        Rate = rate.number;
        ScaleRate = rate.scale;
    }

    internal void UpdateUsd((BigInteger number, int scale) usd)
    {
        Amount = usd.number;
        ScaleAmount = usd.scale;
    }
}


