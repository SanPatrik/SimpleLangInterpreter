namespace SimpleLangInterpreter;

public class Interpreter {
    private List<Instruction> instructions = new List<Instruction>();
    private VariableStore variables = new VariableStore();
    private InstructionParser parser = new InstructionParser();
    private int currentLine = 0; // Updated to track the current line number for error handling

    public void LoadInstructions(string filePath) {
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++) {
            try {
                instructions.Add(parser.Parse(lines[i]));
            } catch (Exception e) {
                ErrorHandler.ReportError(e.Message, i + 1); // Line numbers start at 1
            }
        }
    }

    public void Execute() {
        while (currentLine < instructions.Count) {
            Instruction instruction = instructions[currentLine];
            try {
                ExecuteInstruction(instruction);
            } catch (Exception e) {
                ErrorHandler.ReportError(e.Message, currentLine + 1); // Reporting errors with the correct line number
                return; // Exit execution on error
            }
            currentLine++;  // Increment line number unless modified by a jump instruction
        }
    }

    private void ExecuteInstruction(Instruction instruction) {
        switch (instruction.Operation) {
            case InstructionType.READ:
                Console.Write($"Enter value for {instruction.Operands[0]}: "); // Prompt user for input
                int readValue = int.Parse(Console.ReadLine()); // Convert input to integer
                variables.SetValue(instruction.Operands[0], readValue); // Store in variable
                break;
            case InstructionType.WRITE:
                int writeValue = variables.GetValue(instruction.Operands[0]); // Retrieve value from variable
                Console.WriteLine($"{instruction.Operands[0]}: {writeValue}"); // Print value
                break;
            case InstructionType.ADD:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) + variables.GetValue(instruction.Operands[1])); // Add values and store result
                break;
            case InstructionType.SUBTRACT:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) - variables.GetValue(instruction.Operands[1])); // Subtract values and store result
                break;
            case InstructionType.MULTIPLY:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) * variables.GetValue(instruction.Operands[1])); // Multiply values and store result
                break;
            case InstructionType.LESS_THAN:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) < variables.GetValue(instruction.Operands[1]) ? 1 : 0); // Check if less than and store boolean result as integer
                break;
            case InstructionType.GREATER_THAN:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) > variables.GetValue(instruction.Operands[1]) ? 1 : 0); // Check if greater than and store boolean result as integer
                break;
            case InstructionType.LESS_EQUAL:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) <= variables.GetValue(instruction.Operands[1]) ? 1 : 0); // Check if less than or equal and store boolean result as integer
                break;
            case InstructionType.GREATER_EQUAL:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) >= variables.GetValue(instruction.Operands[1]) ? 1 : 0); // Check if greater than or equal and store boolean result as integer
                break;
            case InstructionType.EQUALS:
                variables.SetValue(instruction.Operands[2], variables.GetValue(instruction.Operands[0]) == variables.GetValue(instruction.Operands[1]) ? 1 : 0); // Check equality and store boolean result as integer
                break;
            case InstructionType.ASSIGN:
                variables.SetValue(instruction.Operands[0], variables.GetValue(instruction.Operands[1])); // Assign value from one variable to another
                break;
            case InstructionType.JUMP:
                currentLine = int.Parse(instruction.Operands[0]) - 2; // Adjust for zero-based index and upcoming increment
                break;
            case InstructionType.JUMPT:
                if (variables.GetValue(instruction.Operands[0]) != 0) {
                    currentLine = int.Parse(instruction.Operands[1]) - 2; // Jump if true
                }
                break;
            case InstructionType.JUMPF:
                if (variables.GetValue(instruction.Operands[0]) == 0) {
                    currentLine = int.Parse(instruction.Operands[1]) - 2; // Jump if false
                }
                break;
            case InstructionType.NOP:
                // No operation, continue to next instruction
                break;
            default:
                Console.WriteLine($"Unknown instruction: {instruction.Operation}");
                break;
        }
    }
}
