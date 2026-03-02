namespace Clua.AbstractSyntaxTree;

public abstract class LiteralNode : Node
{
    public abstract int GetIntValue();
    
    public abstract float GetFloatValue();
}
