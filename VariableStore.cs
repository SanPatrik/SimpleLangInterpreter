namespace SimpleLangInterpreter;

public class VariableStore {
    private Dictionary<string, int> store = new Dictionary<string, int>();

    public int ResolveValue(string operand) {
        // Check if the operand is an integer literal
        if (int.TryParse(operand, out int value)) {
            return value;
        }

        return GetValue(operand);
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
