namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Модель для хранения денег
/// </summary>
public class Money
{
    private const int RublesToKopeksFactor = 100;
    private const string MinusSign = "-";

    public Money(int rubles, int kopeks)
        : this(false, rubles, kopeks)
    {
    }

    public Money(bool isNegative, int rubles, int kopeks)
    {
        if (rubles < 0)
        {
            throw new ArgumentException("Rubles count must be >= 0", nameof(rubles));
        }

        if (kopeks < 0 || kopeks > 99)
        {
            throw new ArgumentException("Kopeks count must be >= 0 and <= 99", nameof(kopeks));
        }

        if (rubles == 0 && kopeks == 0 && isNegative == true)
        {
            throw new ArgumentException("Zero balance cannot be negative", nameof(isNegative));
        }

        IsNegative = isNegative;
        Rubles = rubles;
        Kopeks = kopeks;
    }

    /// <summary>
    /// Отрицательное значение
    /// </summary>
    public bool IsNegative { get; }

    /// <summary>
    /// Число рублей
    /// </summary>
    public int Rubles { get; }

    /// <summary>
    /// Количество копеек
    /// </summary>
    public int Kopeks { get; }

    public static Money operator +(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        int resultTotalKopeks = leftTotalKopeks + rightTotalKopeks;

        return CreateMoneyFromTotalKopeks(resultTotalKopeks);
    }

    public static Money operator -(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        int resultTotalKopeks = leftTotalKopeks - rightTotalKopeks;

        return CreateMoneyFromTotalKopeks(resultTotalKopeks);
    }

    public static bool operator >(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        return leftTotalKopeks > rightTotalKopeks;
    }

    public static bool operator <(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        return leftTotalKopeks < rightTotalKopeks;
    }

    public static bool operator >=(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        return leftTotalKopeks >= rightTotalKopeks;
    }

    public static bool operator <=(Money left, Money right)
    {
        int leftTotalKopeks = left.CalculateTotalKopeks();
        int rightTotalKopeks = right.CalculateTotalKopeks();

        return leftTotalKopeks <= rightTotalKopeks;
    }

    public override string ToString()
    {
        string sign = string.Empty;
        if (IsNegative == true)
        {
            sign = MinusSign;
        }

        return $"Account balance: {sign}{Rubles.ToString()} rub. {Kopeks.ToString()} kop.";
    }

    public override bool Equals(object? obj)
    {
        return obj is Money other
               && IsNegative == other.IsNegative
               && Rubles == other.Rubles
               && Kopeks == other.Kopeks;
    }

    public override int GetHashCode()
    {
        return CalculateTotalKopeks();
    }

    private static Money CreateMoneyFromTotalKopeks(int totalKopeks)
    {
        int rubles = Math.Abs(totalKopeks / RublesToKopeksFactor);
        int kopeks = Math.Abs(totalKopeks % RublesToKopeksFactor);
        bool isNegative = totalKopeks < 0;

        return new Money(isNegative: isNegative, rubles, kopeks);
    }

    private int CalculateTotalKopeks()
    {
        int sign = IsNegative ? -1 : 1;
        return sign * ((Rubles * RublesToKopeksFactor) + Kopeks);
    }
}