using System;
using UnityEngine;
using Random = System.Random;

public static class Extensions
{
    public static T ToNewInstance<T>(this BaseUnitModifikator from)
            where T : BaseUnitModifikator
    {
        var newInstance = ScriptableObject.CreateInstance<T>();
        newInstance.displayname = from.displayname;
        newInstance.name        = from.name;

        newInstance.actionsModifier = from.actionsModifier;

        newInstance.charismaMultiplier     = from.charismaMultiplier;
        newInstance.constitutionMultiplier = from.constitutionMultiplier;
        newInstance.dexterityMultiplier    = from.dexterityMultiplier;
        newInstance.intuitionMultiplier    = from.intuitionMultiplier;
        newInstance.logicMultiplier        = from.logicMultiplier;
        newInstance.willpowerMultiplier    = from.willpowerMultiplier;
        newInstance.wisdomMultiplier       = from.wisdomMultiplier;
        newInstance.charismaMultiplier     = from.charismaMultiplier;

        return newInstance;
    }

    public static float ApplyOperation(this float attributeValue, string op, float modifier) => op switch
    {
        "+" => attributeValue + modifier,
        "-" => attributeValue - modifier,
        "*" => attributeValue * modifier,
        "/" => attributeValue / modifier,
        _ => throw new Exception("Ins Operatorfield kommen nur +, -, *, / rein >:C")
    };

    public static float InfuseRandomness(this float luckyBoyWhoIsAboutToBeRandomized)
    {
        var          random   = new Random();
        const double minValue = 0.5;
        const double maxValue = 1.5;

        var modifier = random.NextDouble() * (maxValue - minValue) + minValue;

        return (float)(modifier * luckyBoyWhoIsAboutToBeRandomized);
    }
}