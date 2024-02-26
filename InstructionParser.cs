namespace SimpleLangInterpreter;

public class InstructionParser {
    private readonly Dictionary<string, InstructionType> operationMap = new Dictionary<string, InstructionType> {
        { "+", InstructionType.ADD },
        { "-", InstructionType.SUBTRACT },
        { "*", InstructionType.MULTIPLY },
        { "<", InstructionType.LESS_THAN },
        { ">", InstructionType.GREATER_THAN },
        { "<=", InstructionType.LESS_EQUAL },
        { ">=", InstructionType.GREATER_EQUAL },
        { "==", InstructionType.EQUALS },
        { "=", InstructionType.ASSIGN },
        // Add other mappings as necessary
    };

    public Instruction Parse(string line) {
        string[] parts = line.Split(',');
        if (parts.Length == 0) {
            throw new Exception("Empty instruction");
        }

        string operationToken = parts[0];
        InstructionType operation;

        // Check if the operation token is a key in the operationMap; if so, use the mapped value
        if (operationMap.ContainsKey(operationToken)) {
            operation = operationMap[operationToken];
        } else if (Enum.IsDefined(typeof(InstructionType), operationToken.ToUpper())) {
            // Additionally, check if it's a valid enum name for cases like READ, WRITE, etc.
            operation = (InstructionType)Enum.Parse(typeof(InstructionType), operationToken.ToUpper(), ignoreCase: true);
        } else {
            // If it's neither in the map nor a valid enum, set to UNKNOWN
            operation = InstructionType.UNKNOWN;
        }

        List<string> operands = parts.Skip(1).ToList();
        return new Instruction(operation, operands);
    }
}
