using System.Text;
using Clua.Values;
namespace Clua.CodeGeneration;

class CodeObject
{
    readonly Instruction[] _instructions;
    readonly CluaValue[] _constsCache;
    
    public int Length => _instructions.Length;

    public Instruction this[int index] => _instructions[index];

    public CluaValue GetConstant(int index) => _constsCache[index];
    
    public CodeObject(IList<Instruction> instructions,  IList<CluaValue> constsCache)
    {
        _instructions = instructions.ToArray();
        _constsCache = constsCache.ToArray();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("[CONSTANTS]:\n");
        
        for (var i = 0; i < _constsCache.Length; i++)
            sb.AppendLine($"[{i}] {_constsCache[i]}");
        
        sb.AppendLine("\n[INSTRUCTIONS]:\n");
        
        for(var i = 0; i < _instructions.Length; i++)
            sb.AppendLine($"[{i}] {_instructions[i]}");
        
        return sb.ToString();
    }
}
