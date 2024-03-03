namespace SimpleLangInterpreter;

public class Interpreter
{
    private List<Instruction> instructions = new List<Instruction>();
    private VariableStore variables = new VariableStore();
    private InstructionParser parser = new InstructionParser();
    private int currentLine = 0;
    private bool jumpExecuted = false;

    public void LoadInstructions(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++)
        {
            try
            {
                instructions.Add(parser.Parse(lines[i]));
            }
            catch (Exception e)
            {
                ErrorHandler.ReportError(e.Message, i + 1);
            }
        }
    }

    public void Execute()
    {
        while (currentLine < instructions.Count)
        {
            Instruction instruction = instructions[currentLine];
            try
            {
                jumpExecuted = false;
                ExecuteInstruction(instruction);
            }
            catch (Exception e)
            {
                ErrorHandler.ReportError(e.Message, currentLine + 1);
                return;
            }

            if (!jumpExecuted)
            {
                currentLine++;
            }
        }
    }


    private void ExecuteInstruction(Instruction instruction)
    {
        switch (instruction.Operation)
        {
            case InstructionType.READ:
                Console.Write($"Enter value for {instruction.Operands[0]}: ");
                int readValue = int.Parse(Console.ReadLine()); // Convert input to integer
                variables.SetValue(instruction.Operands[0], readValue);
                Console.WriteLine($"[READ] Variable '{instruction.Operands[0]}' set to {readValue}.");
                break;
            case InstructionType.WRITE:
                int writeValue = variables.GetValue(instruction.Operands[0]);
                Console.WriteLine($"[WRITE] Value of '{instruction.Operands[0]}': {writeValue}.");
                break;
            case InstructionType.ADD:
                int addResult = variables.ResolveValue(instruction.Operands[0]) +
                                variables.ResolveValue(instruction.Operands[1]);
                variables.SetValue(instruction.Operands[2], addResult);
                Console.WriteLine(
                    $"[ADD] {instruction.Operands[0]} + {instruction.Operands[1]} = {addResult}, stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.SUBTRACT:
                int subtractResult = variables.ResolveValue(instruction.Operands[0]) -
                                     variables.ResolveValue(instruction.Operands[1]);
                variables.SetValue(instruction.Operands[2], subtractResult);
                Console.WriteLine(
                    $"[SUBTRACT] {instruction.Operands[0]} - {instruction.Operands[1]} = {subtractResult}, stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.MULTIPLY:
                int multiplyResult = variables.ResolveValue(instruction.Operands[0]) *
                                     variables.ResolveValue(instruction.Operands[1]);
                variables.SetValue(instruction.Operands[2], multiplyResult);
                Console.WriteLine(
                    $"[MULTIPLY] {instruction.Operands[0]} * {instruction.Operands[1]} = {multiplyResult}, stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.LESS_THAN:
                int lessThanResult = variables.ResolveValue(instruction.Operands[0]) <
                                     variables.ResolveValue(instruction.Operands[1])
                    ? 1
                    : 0;
                variables.SetValue(instruction.Operands[2], lessThanResult);
                Console.WriteLine(
                    $"[LESS THAN] {instruction.Operands[0]} < {instruction.Operands[1]} = {lessThanResult} (1: true, 0: false), stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.GREATER_THAN:
                int greaterThanResult = variables.ResolveValue(instruction.Operands[0]) >
                                        variables.ResolveValue(instruction.Operands[1])
                    ? 1
                    : 0;
                variables.SetValue(instruction.Operands[2], greaterThanResult);
                Console.WriteLine(
                    $"[GREATER THAN] {instruction.Operands[0]} > {instruction.Operands[1]} = {greaterThanResult} (1: true, 0: false), stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.LESS_EQUAL:
                int lessEqualResult = variables.ResolveValue(instruction.Operands[0]) <=
                                      variables.ResolveValue(instruction.Operands[1])
                    ? 1
                    : 0;
                variables.SetValue(instruction.Operands[2], lessEqualResult);
                Console.WriteLine(
                    $"[LESS EQUAL] {instruction.Operands[0]} <= {instruction.Operands[1]} = {lessEqualResult} (1: true, 0: false), stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.GREATER_EQUAL:
                int greaterEqualResult = variables.ResolveValue(instruction.Operands[0]) >=
                                         variables.ResolveValue(instruction.Operands[1])
                    ? 1
                    : 0;
                variables.SetValue(instruction.Operands[2], greaterEqualResult);
                Console.WriteLine(
                    $"[GREATER EQUAL] {instruction.Operands[0]} >= {instruction.Operands[1]} = {greaterEqualResult} (1: true, 0: false), stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.EQUALS:
                int equalsResult = variables.ResolveValue(instruction.Operands[0]) ==
                                   variables.ResolveValue(instruction.Operands[1])
                    ? 1
                    : 0;
                variables.SetValue(instruction.Operands[2], equalsResult);
                Console.WriteLine(
                    $"[EQUALS] {instruction.Operands[0]} == {instruction.Operands[1]} = {equalsResult} (1: true, 0: false), stored in {instruction.Operands[2]}.");
                break;
            case InstructionType.ASSIGN:
                variables.SetValue(instruction.Operands[0],
                    variables.ResolveValue(instruction.Operands[1]));
                break;
            case InstructionType.JUMP:
                int jumpTarget = variables.ResolveValue(instruction.Operands[0]) - 1;
                if (jumpTarget >= 0 && jumpTarget < instructions.Count) {
                    currentLine = jumpTarget;
                    jumpExecuted = true;
                } else {
                    ErrorHandler.ReportError($"Invalid jump target '{instruction.Operands[0]}'.", currentLine + 1);
                }
                break;
            case InstructionType.JUMPT:
                if (variables.ResolveValue(instruction.Operands[0]) != 0) {
                    int jumptTarget = variables.ResolveValue(instruction.Operands[1]) - 1;
                    if (jumptTarget >= 0 && jumptTarget < instructions.Count) {
                        currentLine = jumptTarget;
                        jumpExecuted = true;
                    } else {
                        ErrorHandler.ReportError($"Invalid jump target '{instruction.Operands[1]}'.", currentLine + 1);
                    }
                }
                break;
            case InstructionType.JUMPF:
                if (variables.ResolveValue(instruction.Operands[0]) == 0) {
                    int jumpfTarget = variables.ResolveValue(instruction.Operands[1]) - 1;
                    if (jumpfTarget >= 0 && jumpfTarget < instructions.Count) {
                        currentLine = jumpfTarget;
                        jumpExecuted = true;
                    } else {
                        ErrorHandler.ReportError($"Invalid jump target '{instruction.Operands[1]}'.", currentLine + 1);
                    }
                }
                break;
            case InstructionType.NOP:
                break;
            default:
                Console.WriteLine($"Unknown instruction: {instruction.Operation}");
                break;
        }
    }
}