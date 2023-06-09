namespace Kalkulacka2077;

public class Calculator
{
    public double EvaluateExpression(string expression)
    {
        return new Expression(expression).Evaluate();
    }
}