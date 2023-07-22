using System.Reflection;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class AssemblyHelpers
{
    /// <summary>
    /// Получает информацию о базовых типах классов из namespace "Fuse8_ByteMinds.SummerSchool.Domain", у которых есть наследники.
    /// </summary>
    /// <remarks>
    /// Информация возвращается только по самым базовым классам.
    /// Информация о промежуточных базовых классах не возвращается
    /// </remarks>
    /// <returns>Список типов с количеством наследников</returns>
    public static BaseTypeInfo[] GetTypesWithInheritors()
    {
        List<BaseTypeInfo> baseTypeInfos = new List<BaseTypeInfo>();

        Assembly currentAssembly = Assembly.GetAssembly(typeof(AssemblyHelpers));

        // Получаем все классы из текущей Assembly
        var assemblyClassTypes = currentAssembly
                                                !.DefinedTypes
                                                .Where(p => p.IsClass);

        foreach (Type type in assemblyClassTypes)
        {
            if (type.IsAbstract == true)
            {
                continue;
            }

            Type baseType = GetBaseType(type);
            if (baseType == null || baseType.Assembly != currentAssembly)
            {
                continue;
            }

            BaseTypeInfo foundBaseTypeInfo = FindTypeInBaseTypeInfos(baseTypeInfos, baseType);
            if (foundBaseTypeInfo == null)
            {
                baseTypeInfos.Add(new BaseTypeInfo(baseType.Name, 1));
            }
            else
            {
                foundBaseTypeInfo!.IncrementInheritorsCount();
            }
        }

        return baseTypeInfos.ToArray();
    }

    /// <summary>
    /// Получает базовый тип для класса
    /// </summary>
    /// <param name="type">Тип, для которого необходимо получить базовый тип</param>
    /// <returns>
    /// Первый тип в цепочке наследований. Если наследования нет, возвращает null
    /// </returns>
    /// <example>
    /// Класс A, наследуется от B, B наследуется от C
    /// При вызове GetBaseType(typeof(A)) вернется C
    /// При вызове GetBaseType(typeof(B)) вернется C
    /// При вызове GetBaseType(typeof(C)) вернется C
    /// </example>
    private static Type? GetBaseType(Type type)
    {
        var baseType = type;

        while (baseType.BaseType is not null && baseType.BaseType != typeof(object))
        {
            baseType = baseType.BaseType;
        }

        return baseType == type
            ? null
            : baseType;
    }

    private static BaseTypeInfo? FindTypeInBaseTypeInfos(IEnumerable<BaseTypeInfo> baseTypeInfos, Type type)
    {
        foreach (BaseTypeInfo baseTypeInfo in baseTypeInfos)
        {
            if (baseTypeInfo.Name == type.Name)
            {
                return baseTypeInfo;
            }
        }

        return null;
    }
}

public sealed class BaseTypeInfo
{
    public BaseTypeInfo(string baseTypeName, int inheritorCount)
    {
        Name = baseTypeName;
        InheritorCount = inheritorCount;
    }

    public string Name { get; }

    public int InheritorCount { get; private set; }

    public void Deconstruct(out string baseTypeName, out int inheritorCount)
    {
        baseTypeName = Name;
        inheritorCount = InheritorCount;
    }

    public void IncrementInheritorsCount()
    {
        InheritorCount++;
    }
}