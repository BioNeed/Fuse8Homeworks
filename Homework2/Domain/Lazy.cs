namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Контейнер для значения, с отложенным получением
/// </summary>
public class Lazy<TValue>
{
    private readonly Func<TValue> acion;
    private TValue? value;
    private bool isInitializedProperty;

    public Lazy(Func<TValue> func)
    {
        acion = func;
    }

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