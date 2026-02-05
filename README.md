# 🪙 PrecisionIssue: High-Precision Financial Math in C#

This repository provides a solution for a common problem in financial systems: **Precision Loss**. When dealing with very small fractions or extremely large numbers, the standard `decimal` type in C# (limited to 28-29 digits) often falls short.

## 🚀 The Challenge
Standard financial types like `decimal` or `double` use floating-point or fixed-precision arithmetic, which can lead to rounding errors. In weighted average calculations or currency conversions, these tiny errors can accumulate into significant financial discrepancies.

### The Problematic Equation:
I tested a complex weighted average that standard calculators truncate:
$$\text{Result} = \frac{\sum (Weight \times Rate)}{\sum Weight}$$

## 🛠️ The Solution: `BigInteger` + `Scale`
To achieve **absolute precision**, I decompose numbers into two distinct components:
1.  **BigInteger (The Raw Number):** Stores all digits as a massive integer.
2.  **Scale (The Exponent):** An integer that tracks the position of the decimal point.

**Example:**
- Value: `1235125.123`
- Internal State: `Number = 1235125123`, `Scale = 3`

---

## 🧪 Comparison: Standard vs. BigInteger Strategy

We ran a stress test with rates up to 20 decimal places. Here is the comparison of the results:

### 📱 Standard Calculator Result
`7.2998852807335291107288063454036` (Truncated at 31 digits)

### 💎 Our BigInteger Strategy Result
`7.2998852807335291107288063454036450561248542246895854104038414785323034397300739650237881961845036272`
*(Full **100 digits** of mathematical perfection)*

---

## 📂 Repository Structure
- **Core Logic:** Implementation of `CustomDecimal` using `BigInteger`.
- **Operators:** Support for `+`, `-`, `*`, and `/` using operator overloading.
- **Scale Alignment:** Automatic adjustment of scales during addition and subtraction.

## 💻 How it Works (Snippet)
The core arithmetic ensures that no digit is lost during the process:

```csharp
public static CustomDecimal operator *(CustomDecimal a, CustomDecimal b)
{
    // In multiplication, we multiply raw numbers and add the scales
    return new CustomDecimal(a.Number * b.Number, a.Scale + b.Scale);
}
