namespace Kalkulacka2077.Operations;

public class Multiplication : IOperation
{
    public double Evaluate(params double[] operands)
    {
        return operands.Aggregate((a, b) => a * b);
    }
}