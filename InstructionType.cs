namespace SimpleLangInterpreter;

public enum InstructionType {
    READ,
    WRITE,
    ADD,
    SUBTRACT,
    MULTIPLY,
    LESS_THAN,
    GREATER_THAN,
    LESS_EQUAL,
    GREATER_EQUAL,
    EQUALS,
    ASSIGN,
    JUMP,
    JUMPT,
    JUMPF,
    NOP,
    UNKNOWN  // Add this to handle unexpected instructions
}
