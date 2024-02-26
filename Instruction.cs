namespace SimpleLangInterpreter;

public class Instruction {
    public InstructionType Operation { get; set; }
    public List<string> Operands { get; set; }

    public Instruction(InstructionType operation, List<string> operands) {
        Operation = operation;
        Operands = operands;
    }
}

