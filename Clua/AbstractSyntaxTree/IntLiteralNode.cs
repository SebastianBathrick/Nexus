namespace Clua.AbstractSyntaxTree;

public class IntLiteralNode(int val) : LiteralNode
{
    public override int GetIntValue() => val;
    
    public override float GetFloatValue() => val;
    
    public override string ToString() => val.ToString();
}
