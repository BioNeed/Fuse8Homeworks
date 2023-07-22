using System.Reflection;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class BankCardHelpers
{
    /// <summary>
    /// Получает номер карты без маски
    /// </summary>
    /// <param name="card">Банковская карта</param>
    /// <returns>Номер карты без маски</returns>
    public static string GetUnmaskedCardNumber(BankCard card)
    {
        Type bankCardType = typeof(BankCard);
        FieldInfo numberField = bankCardType.GetField("_number", BindingFlags.Instance | BindingFlags.NonPublic);

        return numberField?.GetValue(card)?.ToString();
    }
}