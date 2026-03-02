using System;
using Clua.SyntaxAnalysis;
using Clua.SyntaxAnalysis.Expressions;
using Clua.Values;

namespace Clua.CodeGeneration;

class CodeObjectGenerator
{
    List<CluaValue> _constsCache = [];
    
    public CodeObject Generate(Node node)
    {
        var instructions = GetExpressionInstructions(node);
        var retObj = new CodeObject(instructions, _constsCache);
        Reset();
        return retObj;
    }

    List<Instruction> GetExpressionInstructions(Node node)
    {
        if (node is not ExpressionNode expression)
            return [GetPushInstruction(node)];
        
        var lByteCode = GetExpressionInstructions(expression.Left);
        var rByteCode = GetExpressionInstructions(expression.Right);
        var opInstr = new Instruction(GetOperatorOperation(expression.Operator));
        
        return [..lByteCode, ..rByteCode, opInstr];
    }
    
    Instruction GetPushInstruction(Node node)
    {
        if (node is not NumberLiteralNode numNode)
            throw new ArgumentException("node is not a NumberLiteralNode");
        
        var numVal = numNode.GetNumberValue();
        var constIndex = _constsCache.FindIndex(x => x == numVal);

        if (constIndex == -1)
        {
            _constsCache.Add(new CluaNumber(numVal));
            constIndex = _constsCache.Count - 1;
        }

        return new Instruction(Operation.PushConstant, CacheType.Constant, constIndex);
    }

    // Reset the generator's state so it can be reused if needed
    void Reset()
    {
        _constsCache.Clear();
    }
    
    static Operation GetOperatorOperation(Operator op)
    {
        switch (op)
        {
            case Operator.Addition: return Operation.Add;
            case Operator.Subtraction: return Operation.Subtract;
            case Operator.Multiplication: return Operation.Multiply;
            case Operator.Division: return Operation.Divide;
            default:
                throw new InvalidOperationException($"Unsupported operator: {op}");
        }
    }
}
