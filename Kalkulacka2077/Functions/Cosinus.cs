namespace Kalkulacka2077.Functions;

public class Cosinus: IFunction
{
    public double Evaluate(params double[] operands)
    {
        return Math.Cos(operands[0]);
    }
}