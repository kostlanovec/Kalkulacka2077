namespace Kalkulacka2077.Operations;

public class Subtraction : IOperation
{
    public double Evaluate(params double[] operands)
    {
        return operands.Aggregate((a, b) => a - b);
    }
}