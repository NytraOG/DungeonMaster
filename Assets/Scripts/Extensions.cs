using System;

public static class Extensions
{
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
        var modifier = new Random().NextDouble() * 2.0;
        var result   = (float)(modifier * luckyBoyWhoIsAboutToBeRandomized);

        return result;
    }
}