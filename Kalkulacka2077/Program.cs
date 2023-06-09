using System;
using Kalkulacka2077;

var calculator = new Calculator();
Console.WriteLine("Kalkulačka 2077");

while (true)
{ 
    Console.Write("Zadejte výraz (nebo 'exit' pro ukončení): ");
    string input = Console.ReadLine();

    if (input.ToLower() == "exit")
    {
        break;
    }

    try
    {
        double result = calculator.EvaluateExpression(input);
        Console.WriteLine($"Výsledek: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Chyba: {ex.Message}");
    }

    Console.WriteLine();
}

Console.WriteLine("Konec programu");