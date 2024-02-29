namespace SimpleLangInterpreter;

public class VariableStore {
    private Dictionary<string, int> store = new Dictionary<string, int>();

    public int ResolveValue(string operand) {
        // Check if the operand is an integer literal
        if (int.TryParse(operand, out int value)) {
            return value; // Return the integer value directly
        }

        // Otherwise, treat it as a variable name and get its value
        return GetValue(operand); // This method should already exist in your VariableStore class
    }
    
    public void SetValue(string name, int value) {
        store[name] = value;
    }

    public int GetValue(string name) {
        if (!store.ContainsKey(name)) {
            throw new Exception($"Variable {name} not defined.");
        }
        return store[name];
    }

    public bool Exists(string name) {
        return store.ContainsKey(name);
    }
}
