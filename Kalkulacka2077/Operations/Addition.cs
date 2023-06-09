namespace Kalkulacka2077.Operations;

public class Addition : IOperation
{
    public double Evaluate(params double[] operands)
    {
        return operands.Sum();
    }
}