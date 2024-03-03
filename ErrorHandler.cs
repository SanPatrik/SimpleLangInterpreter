namespace SimpleLangInterpreter;

public class ErrorHandler {
    public static void ReportError(string message, int lineNum) {
        Console.WriteLine($"Error at line {lineNum}: {message}");
        Environment.Exit(1);
    }
}
