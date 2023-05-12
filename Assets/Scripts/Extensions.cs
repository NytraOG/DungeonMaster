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
        var          random   = new Random();
        const double minValue = 0.5;
        const double maxValue = 1.5;

        var modifier = random.NextDouble() * (maxValue - minValue) + minValue;

        return (float)(modifier * luckyBoyWhoIsAboutToBeRandomized);
    }
}