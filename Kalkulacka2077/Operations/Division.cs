namespace Kalkulacka2077.Operations;

public class Division : IOperation
{
    public double Evaluate(params double[] operands)
    {
        return operands.Aggregate((a, b) => a / b);
    }
}