namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Контейнер для значения, с отложенным получением
/// </summary>
public class Lazy<TValue>
{
    private TValue? value;
    private Func<TValue> acion;
    private bool isInitializedProperty;

    public Lazy(Func<TValue> func)
    {
        acion = func;
    }

    // TODO Реализовать ленивое получение значение при первом обращении к Value

    public TValue? Value 
    {
        get
        {
            if (isInitializedProperty == false)
            {
                value = acion();
                isInitializedProperty = true;
            }

            return value;
        }
    }
}