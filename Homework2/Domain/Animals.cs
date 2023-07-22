namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Животное
/// </summary>
public abstract class Animal
{
	/// <summary>
	/// true - если животное является другом человека
	/// </summary>
	public virtual bool IsHumanFriend => false;

	/// <summary>
	/// true - если у животного большой вес
	/// </summary>
	public abstract bool HasBigWeight { get; }

	/// <summary>
	/// Как говорит животное
	/// </summary>
	/// <returns>Возвращает звук, который говорит животное</returns>
	public abstract string WhatDoesSay();
}
// TODO В наследниках реализовать метод WhatDoesSay и свойство HasBigWeight, а также переопределить IsHumanFriend там, где это нужно

/// <summary>
/// Собака
/// </summary>
public abstract class Dog : Animal
{
	public override bool IsHumanFriend => true;

    public override bool HasBigWeight => false;

    public override string WhatDoesSay()
    {
        return AnimalSounds.DogSound;
    }
}

/// <summary>
/// Лиса
/// </summary>
public sealed class Fox : Animal
{
    public override bool HasBigWeight => false;

    public override string WhatDoesSay()
    {
        return AnimalSounds.FoxSound;
    }
}

/// <summary>
/// Чихуахуа
/// </summary>
public sealed class Chihuahua : Dog
{
}

/// <summary>
/// Хаски
/// </summary>
public sealed class Husky : Dog
{
    public override bool HasBigWeight => true;

    public new string WhatDoesSay()
    {
        return AnimalSounds.HuskySound;
    }
}