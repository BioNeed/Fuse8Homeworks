using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class ExceptionHandler
{
	private const string UnknownErrorMessage = "Произошла непредвиденная ошибка";
	private const string NotValidKopekCountErrorMessage = "Количество копеек должно быть больше 0 и меньше 99";
	private const string NegativeRubleCountErrorMessage = "Число рублей не может быть отрицательным";
	private const string NotFoundErrorMessage = "Ресурс не райден";

    /// <summary>
    /// Обрабатывает исключение, которое может возникнуть при выполнении <paramref name="action"/>
    /// </summary>
    /// <param name="action">Действие, которое может породить исключение</param>
    /// <returns>Сообщение об ошибке</returns>
    public static string? Handle(Action action)
	{
		// TODO Реализовать обработку исключений
		try
		{
			action();
		}
		catch(NotValidKopekCountException)
		{
			return NotValidKopekCountErrorMessage;
        }
		catch(NegativeRubleCountException)
		{
			return NegativeRubleCountErrorMessage;
        }
		catch(MoneyException ex)
		{
			return ex.Message;
		}
        catch (HttpRequestException ex) when (ex.StatusCode == (HttpStatusCode)404)
        {
			return NotFoundErrorMessage;
        }
        catch (HttpRequestException ex)
        {
			return ex.StatusCode.ToString();
        }
		catch(Exception)
		{
			return UnknownErrorMessage;
        }

		return null;
	}
}

public class MoneyException : Exception
{
	public MoneyException()
	{
	}

	public MoneyException(string? message)
		: base(message)
	{
	}
}

public class NotValidKopekCountException : MoneyException
{
	public NotValidKopekCountException()
	{
	}
}

public class NegativeRubleCountException : MoneyException
{
	public NegativeRubleCountException()
	{
	}
}