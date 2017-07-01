using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProportuionValue<T>
{
    public float Proportion { get; set; }
    public T Value { get; set; }
}

public static class ProportionValue
{
    public static ProportuionValue<T> Create<T>(float proportion, T value)
    {
        return new ProportuionValue<T> { Proportion = proportion, Value = value };
    }

    static System.Random random = new System.Random();
    public static T ChoseByRandom<T>(this IEnumerable<ProportuionValue<T>> collection)
    {
        var rnd = random.NextDouble();
        foreach (var num in collection)
        {
            if (rnd < num.Proportion)
                return num.Value;
            rnd -= num.Proportion;
        }
        throw new InvalidOperationException("The proportions in the collection do not add up to 1."); // Соотношения в коллекции является больше одного или меньше одного (то есть мы привысили процентаж в 100% или <100%).
    }
}
