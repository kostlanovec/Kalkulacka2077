using System.Reflection;
using Kalkulacka2077.Operations;
using Kalkulacka2077.Functions;

namespace Kalkulacka2077
{
    public class Expression
    {
        private readonly Dictionary<string, IOperation> _operations = new ();
        private readonly Dictionary<string, IFunction> _functions = new ();

        private string _content;

        public string Content
        {
            get { return _content; }
            set { _content = value?.TrimEnd('='); }
        }

        public Expression(string content)
        {
            Content = content;
            RegisterDefaultOperations();
            RegisterDefaultFunctions();
        }

        private void RegisterDefaultOperations()
        {
            
            //přidávání operací
            RegisterOperation("+", new Addition());
            RegisterOperation("-", new Subtraction());
            RegisterOperation("*", new Multiplication());
            RegisterOperation("/", new Division());
        }

        private void RegisterDefaultFunctions()
        {
            //přidávání funkcí
            RegisterFunction("Sin", new Sinus());
            RegisterFunction("Cos", new Cosinus());
        }

        public void RegisterOperation(string symbol, IOperation operation)
        {
            _operations[symbol] = operation;
        }

        public void RegisterFunction(string name, IFunction function)
        {
            _functions[name] = function;
        }

        public double Evaluate()
        {
            var tokens = Tokenize();
            var postfix = ConvertToPostfix(tokens);
            return EvaluatePostfix(postfix);
        }

        private List<string> Tokenize()
        {
            var tokens = new List<string>();
            var currentToken = "";
            var isValidNumber = false;
            var parenCount = 0;

        for (var i = 0; i < Content.Length; i++)
        {
            var character = Content[i];

            if (char.IsDigit(character) || character == '.')
            {
                currentToken += character;
                isValidNumber = true;
            }
        else if (char.IsLetter(character))
        {
            currentToken += character;
        }
        else if (IsOperator(character))
        {
            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                tokens.Add(currentToken);
                currentToken = "";
            }

            tokens.Add(character.ToString());
            isValidNumber = false;
        }
        else if (character == '(')
        {
            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                tokens.Add(currentToken);
                currentToken = "";
            }

            tokens.Add(character.ToString());
            isValidNumber = false;
            parenCount++;
        }
        else if (character == ')')
        {
            if (!string.IsNullOrWhiteSpace(currentToken))
            {
                tokens.Add(currentToken);
                currentToken = "";
            }

            tokens.Add(character.ToString());
            isValidNumber = false;
            parenCount--;
        }
        else if (!char.IsWhiteSpace(character))
        {
            throw new InvalidOperationException($"Unknown token: {character}");
        }

        if (isValidNumber && (i == Content.Length - 1 || !char.IsDigit(Content[i + 1])))
        {
            tokens.Add(currentToken);
            currentToken = "";
            isValidNumber = false;
        }

        }

        if (!string.IsNullOrWhiteSpace(currentToken))
        {
            tokens.Add(currentToken);
        }

        if (parenCount != 0)
        {
            throw new InvalidOperationException("Mismatched parentheses.");
        }
        return tokens;
}
        private bool IsOperator(char character)
        {
            return character == '+' || character == '-' || character == '*' || character == '/';
        }

        private List<string> ConvertToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var operatorStack = new Stack<string>();
            var functionStack = new Stack<string>();

            var precedence = new Dictionary<string, int>()
            {
                {"+", 1},
                {"-", 1},
                {"*", 2},
                {"/", 2}
            };

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    output.Add(token);
                }
                else if (IsOperator(token[0]))
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(" && precedence[token] <= precedence[operatorStack.Peek()])
                    {
                        output.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        output.Add(operatorStack.Pop());
                    }

                    if (operatorStack.Count == 0 || operatorStack.Peek() != "(")
                    {
                        throw new InvalidOperationException("Mismatched parentheses.");
                    }

                    operatorStack.Pop(); // odstranění "("

                    while (functionStack.Count > 0)
                    {
                        output.Add(functionStack.Pop());
                    }
                }
                else if (_functions.ContainsKey(token))
                {
                    functionStack.Push(token);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown token: {token}");
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "(" || operatorStack.Peek() == ")")
                {
                    throw new InvalidOperationException("Mismatched parentheses.");
                }
                output.Add(operatorStack.Pop());
            }

            return output;
        }
        private double EvaluatePostfix(List<string> postfix)
        {
            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (double.TryParse(token, out double number))
                { 
                    stack.Push(number);
                }
            else if (_operations.ContainsKey(token))
            {
                var operation = _operations[token];
                if (stack.Count < 2)
                {
                    throw new InvalidOperationException("Invalid expression.");
                }

                double operand2 = stack.Pop();
                double operand1 = stack.Pop(); 
                double result = operation.Evaluate(operand1, operand2); 
                stack.Push(result);
            }
            else if (_functions.ContainsKey(token))
            {
                var function = _functions[token];
                MethodInfo evaluateMethod = function.GetType().GetMethod("Evaluate");
                int operandCount = evaluateMethod.GetParameters().Length;
                double[] operands = new double[operandCount];
                for (int i = operandCount - 1; i >= 0; i--)
                {
                    if (stack.Count == 0)
                    {
                        throw new InvalidOperationException("Invalid expression.");
                    }
                    operands[i] = stack.Pop();
                }
                double result = (double)evaluateMethod.Invoke(function, new object[] { operands }); 
                stack.Push(result);
            }
            else
            {
                throw new InvalidOperationException($"Unknown token: {token}");
            }
            }

            if (stack.Count == 1)
            {
                return stack.Pop();
            }

            throw new InvalidOperationException("Invalid expression.");
        }
    }
}
