using Nexus;
using Nexus.Runtime.Values;
using NUnit.Framework;

namespace Nexus.Tests;

[TestFixture]
public class NovaInterpreterTests
{
    static void AssertRunResult(string source, string expectedToString)
    {
        var result = NovaInterpreter.Run(source);
        Assert.That(result.ToString(), Is.EqualTo(expectedToString));
    }

    static void AssertRunBool(string source, bool expected)
    {
        var result = NovaInterpreter.Run(source);
        Assert.That(result, Is.InstanceOf<NexusBool>());
        Assert.That(((NexusBool)result).ToString(), Is.EqualTo(expected ? "true" : "false"));
    }

    // Arithmetic
    [Test]
    public void Run_ReturnOnePlusTwo_ReturnsThree()
    {
        AssertRunResult("return 1 + 2", "3");
    }

    [Test]
    public void Run_ReturnTwoTimesThreePlusFour_ReturnsTen()
    {
        AssertRunResult("return 2 * 3 + 4", "10");
    }

    [Test]
    public void Run_ReturnParenthesizedArithmetic_ReturnsNine()
    {
        AssertRunResult("return (1 + 2) * 3", "9");
    }

    [Test]
    public void Run_ReturnUnaryMinusTimesFive_ReturnsNegativeFive()
    {
        AssertRunResult("return -1 * 5", "-5");
    }

    [Test]
    public void Run_ReturnDivision_ReturnsQuotient()
    {
        AssertRunResult("return 10 / 2", "5");
    }

    [Test]
    public void Run_ReturnMixedArithmetic_ReturnsExpectedValue()
    {
        AssertRunResult("return 1 + 2 * 3 - 4 / 2", "5");
    }

    // Literals
    [Test]
    public void Run_ReturnZero_ReturnsZero()
    {
        AssertRunResult("return 0", "0");
    }

    [Test]
    public void Run_ReturnDecimal_ReturnsDecimal()
    {
        AssertRunResult("return 3.14", "3.14");
    }

    [Test]
    public void Run_ReturnTrue_ReturnsTrue()
    {
        AssertRunResult("return true", "true");
    }

    [Test]
    public void Run_ReturnFalse_ReturnsFalse()
    {
        AssertRunResult("return false", "false");
    }

    // Comparison
    [Test]
    public void Run_ReturnOneEqualsOne_ReturnsTrue()
    {
        AssertRunBool("return 1 == 1", true);
    }

    [Test]
    public void Run_ReturnOneNotEqualsTwo_ReturnsTrue()
    {
        AssertRunBool("return 1 != 2", true);
    }

    [Test]
    public void Run_ReturnOneLessThanTwo_ReturnsTrue()
    {
        AssertRunBool("return 1 < 2", true);
    }

    [Test]
    public void Run_ReturnTwoGreaterThanOne_ReturnsTrue()
    {
        AssertRunBool("return 2 > 1", true);
    }

    [Test]
    public void Run_ReturnOneLessThanOrEqualOne_ReturnsTrue()
    {
        AssertRunBool("return 1 <= 1", true);
    }

    [Test]
    public void Run_ReturnTwoGreaterThanOrEqualTwo_ReturnsTrue()
    {
        AssertRunBool("return 2 >= 2", true);
    }

    // Logic
    [Test]
    public void Run_ReturnTrueAndFalse_ReturnsFalse()
    {
        AssertRunBool("return true and false", false);
    }

    [Test]
    public void Run_ReturnTrueOrFalse_ReturnsTrue()
    {
        AssertRunBool("return true or false", true);
    }

    [Test]
    public void Run_ReturnNotFalse_ReturnsTrue()
    {
        AssertRunBool("return not false", true);
    }

    [Test]
    public void Run_ReturnTrueAndFalseOrTrue_ReturnsTrue()
    {
        AssertRunBool("return true and false or true", true);
    }

    // Coercion: bool and number equality (language allows)
    [Test]
    public void Run_ReturnTrueEqualsOne_ReturnsTrueViaCoercion()
    {
        AssertRunBool("return true == 1", true);
    }

    // --- Complex and nested expressions (many cases) ---

    [Test]
    public void Run_DeeplyNestedArithmetic_ReturnsCorrectValue()
    {
        AssertRunResult("return (1 + (2 + (3 + (4 + 5))))", "15");
    }

    [Test]
    public void Run_NestedParenthesesMultiplicationAndAddition_ReturnsCorrectValue()
    {
        AssertRunResult("return ((2 * 3) + (4 * 5))", "26");
    }

    [Test]
    public void Run_ComplexArithmeticWithMultipleParenLevels_ReturnsCorrectValue()
    {
        AssertRunResult("return (1 + 2) * (3 + 4) - (5 / 1)", "16");
    }

    [Test]
    public void Run_ArithmeticWithNestedNegation_ReturnsCorrectValue()
    {
        AssertRunResult("return -(-1) * 10", "10");
    }

    [Test]
    public void Run_NestedDivisionAndMultiplication_ReturnsCorrectValue()
    {
        AssertRunResult("return 100 / (2 * 5)", "10");
    }

    [Test]
    public void Run_ChainedSubtractionAndAddition_ReturnsCorrectValue()
    {
        AssertRunResult("return 10 - 2 - 3 + 1", "6");
    }

    [Test]
    public void Run_ArithmeticExpressionComparedToNumber_ReturnsTrue()
    {
        AssertRunBool("return (1 + 2) == 3", true);
    }

    [Test]
    public void Run_ArithmeticExpressionComparedToNumber_ReturnsFalseWhenUnequal()
    {
        AssertRunBool("return (1 + 2) == 4", false);
    }

    [Test]
    public void Run_ArithmeticResultLessThan_ReturnsBool()
    {
        AssertRunBool("return (1 + 2 * 3) < 10", true);
    }

    [Test]
    public void Run_ArithmeticResultGreaterThanOrEqual_ReturnsBool()
    {
        AssertRunBool("return (2 * 5) >= 10", true);
    }

    [Test]
    public void Run_NestedComparisonInsideParens_ReturnsBool()
    {
        AssertRunBool("return (1 < 2) == true", true);
    }

    [Test]
    public void Run_ComparisonOfArithmeticBothSides_ReturnsBool()
    {
        AssertRunBool("return (1 + 1) < (2 + 2)", true);
    }

    [Test]
    public void Run_NotEqualToWithArithmetic_ReturnsBool()
    {
        AssertRunBool("return (3 * 3) != 8", true);
    }

    [Test]
    public void Run_LogicalAndWithComparisons_ReturnsBool()
    {
        AssertRunBool("return 1 < 2 and 3 > 2", true);
    }

    [Test]
    public void Run_LogicalOrWithComparisons_ReturnsBool()
    {
        AssertRunBool("return 1 > 2 or 2 < 3", true);
    }

    [Test]
    public void Run_FalseAndFalseOrTrue_AndBindsTighter_ReturnsTrue()
    {
        // (false and false) or true => false or true => true
        AssertRunBool("return false and false or true", true);
    }

    [Test]
    public void Run_TrueOrTrueAndFalse_LeftAssociative_ReturnsFalse()
    {
        // (true or true) and false => true and false => false (and/or same precedence, left-associative)
        AssertRunBool("return true or true and false", false);
    }

    [Test]
    public void Run_NotAppliedToComparison_ReturnsBool()
    {
        AssertRunBool("return not (1 == 2)", true);
    }

    [Test]
    public void Run_NotAppliedToLogicalAnd_ReturnsBool()
    {
        AssertRunBool("return not (true and false)", true);
    }

    [Test]
    public void Run_NestedLogic_ParensOverridePrecedence()
    {
        AssertRunBool("return (true or false) and false", false);
    }

    [Test]
    public void Run_ComplexLogicChain_ReturnsExpected()
    {
        AssertRunBool("return true and (false or true) and not false", true);
    }

    [Test]
    public void Run_NotNotTrue_ReturnsTrue()
    {
        AssertRunBool("return not not true", true);
    }

    [Test]
    public void Run_NotNotFalse_ReturnsFalse()
    {
        AssertRunBool("return not not false", false);
    }

    // Coercion: bool as number in arithmetic
    [Test]
    public void Run_TruePlusOne_ReturnsTwoViaCoercion()
    {
        AssertRunResult("return true + 1", "2");
    }

    [Test]
    public void Run_FalsePlusZero_ReturnsZero()
    {
        AssertRunResult("return false + 0", "0");
    }

    [Test]
    public void Run_TrueTimesFive_ReturnsFive()
    {
        AssertRunResult("return true * 5", "5");
    }

    [Test]
    public void Run_FalseTimesTen_ReturnsZero()
    {
        AssertRunResult("return false * 10", "0");
    }

    [Test]
    public void Run_TrueMinusOne_ReturnsZero()
    {
        AssertRunResult("return true - 1", "0");
    }

    [Test]
    public void Run_NumberPlusTrue_ReturnsEight()
    {
        AssertRunResult("return 7 + true", "8");
    }

    [Test]
    public void Run_TruePlusTrue_ReturnsTwo()
    {
        AssertRunResult("return true + true", "2");
    }

    [Test]
    public void Run_FalsePlusFalse_ReturnsZero()
    {
        AssertRunResult("return false + false", "0");
    }

    // Coercion: bool and number in comparison
    [Test]
    public void Run_FalseEqualsZero_ReturnsTrue()
    {
        AssertRunBool("return false == 0", true);
    }

    [Test]
    public void Run_OneEqualsTrue_ReturnsTrue()
    {
        AssertRunBool("return 1 == true", true);
    }

    [Test]
    public void Run_ZeroEqualsFalse_ReturnsTrue()
    {
        AssertRunBool("return 0 == false", true);
    }

    [Test]
    public void Run_TrueNotEqualsZero_ReturnsTrue()
    {
        AssertRunBool("return true != 0", true);
    }

    [Test]
    public void Run_FalseLessThanOne_ReturnsTrue()
    {
        AssertRunBool("return false < 1", true);
    }

    [Test]
    public void Run_TrueGreaterThanZero_ReturnsTrue()
    {
        AssertRunBool("return true > 0", true);
    }

    [Test]
    public void Run_TrueGreaterThanOrEqualOne_ReturnsTrue()
    {
        AssertRunBool("return true >= 1", true);
    }

    [Test]
    public void Run_FalseLessThanOrEqualZero_ReturnsTrue()
    {
        AssertRunBool("return false <= 0", true);
    }

    // Coercion in nested expressions
    [Test]
    public void Run_ArithmeticWithBoolOperand_ReturnsNumber()
    {
        AssertRunResult("return (true + 1) * 2", "4");
    }

    [Test]
    public void Run_ComparisonOfCoercedArithmetic_BoolPlusBoolThenCompare_ReturnsBool()
    {
        // (true + false) is 1 (number); compare to 2 is false. Use (true + true) == 2.
        AssertRunBool("return (true + true) == 2", true);
    }

    [Test]
    public void Run_LogicWithCoercedComparison_ReturnsBool()
    {
        AssertRunBool("return (true == 1) and (false == 0)", true);
    }

    [Test]
    public void Run_NestedCoercionInComparison_ReturnsBool()
    {
        // Both sides numeric: (true + 0) is 1, and (0 + 1) is 1, so 1 >= 1.
        AssertRunBool("return (true + 0) >= (0 + 1)", true);
    }

    // More complex mixed
    [Test]
    public void Run_ExpressionWithAllOperatorTypes_ReturnsNumber()
    {
        AssertRunResult("return (1 + 2 * 3 - 4 / 2) + 0", "5");
    }

    [Test]
    public void Run_ComparisonThenLogic_ReturnsBool()
    {
        AssertRunBool("return (1 < 2 and 2 < 3) or false", true);
    }

    [Test]
    public void Run_DeeplyNestedComparisonAndLogic_ReturnsBool()
    {
        AssertRunBool("return ((1 == 1) and (2 != 3)) and (true or false)", true);
    }

    [Test]
    public void Run_UnaryMinusInNestedExpression_ReturnsCorrectValue()
    {
        AssertRunResult("return -(2 * 3) + 10", "4");
    }

    [Test]
    public void Run_MultipleUnaryMinus_ReturnsCorrectValue()
    {
        AssertRunResult("return -(-(-5))", "-5");
    }

    [Test]
    public void Run_DecimalArithmetic_ReturnsCorrectValue()
    {
        AssertRunResult("return 3.14 + 0.86", "4");
    }

    [Test]
    public void Run_DecimalComparison_ReturnsBool()
    {
        AssertRunBool("return 2.5 < 3.5", true);
    }

    [Test]
    public void Run_ZeroLessThanOne_ReturnsTrue()
    {
        AssertRunBool("return 0 < 1", true);
    }

    [Test]
    public void Run_ZeroEqualsZero_ReturnsTrue()
    {
        AssertRunBool("return 0 == 0", true);
    }

    // Error cases
    [Test]
    public void Run_InvalidSyntaxUnexpectedToken_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => NovaInterpreter.Run("return 1 2"));
    }

    [Test]
    public void Run_EmptyInput_Throws()
    {
        // Empty input yields no statements; Compiler accesses block.Statements[0] and throws.
        Assert.Throws<IndexOutOfRangeException>(() => NovaInterpreter.Run(""));
    }
}
