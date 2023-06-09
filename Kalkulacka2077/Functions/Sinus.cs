using Kalkulacka2077.Operations;

namespace Kalkulacka2077.Functions;

public class Sinus: IFunction
{

    public double Evaluate(params double[] operands)
    {
        return Math.Sin(operands[0]);
    }
}