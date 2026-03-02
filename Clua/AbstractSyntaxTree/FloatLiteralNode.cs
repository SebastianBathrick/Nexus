namespace Clua.AbstractSyntaxTree;

public class FloatLiteralNode(float val) : LiteralNode
{
    public override int GetIntValue() => (int)val;
    
    public override float GetFloatValue() => val;
    
    public override string ToString() => val.ToString();
}
