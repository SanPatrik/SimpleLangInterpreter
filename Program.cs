namespace SimpleLangInterpreter;

class Program
{
    static void Main(string[] args) {
        
        if (args.Length == 0) {
            Console.WriteLine("No file provided.");
            return;
        }
        string filePath = args[0];
        Interpreter interpreter = new Interpreter();
        interpreter.LoadInstructions(filePath);
        interpreter.Execute();
        
    }
}