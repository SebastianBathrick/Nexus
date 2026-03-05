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
    public void Run_TrueOrTrueAndFalse_AndBindsTighter_ReturnsTrue()
    {
        // true or (true and false) => true or false => true (and has higher precedence than or)
        AssertRunBool("return true or true and false", true);
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

    // is / is not keyword operators
    [Test]
    public void Run_IsKeyword_EqualNumbers_ReturnsTrue()
    {
        AssertRunBool("return 5 is 5", true);
    }

    [Test]
    public void Run_IsKeyword_UnequalNumbers_ReturnsFalse()
    {
        AssertRunBool("return 5 is 6", false);
    }

    [Test]
    public void Run_IsNotKeyword_UnequalNumbers_ReturnsTrue()
    {
        AssertRunBool("return 5 is not 6", true);
    }

    [Test]
    public void Run_IsNotKeyword_EqualNumbers_ReturnsFalse()
    {
        AssertRunBool("return 3 is not 3", false);
    }

    [Test]
    public void Run_IsKeyword_BoolAndCoercedNumber_ReturnsTrue()
    {
        AssertRunBool("return true is 1", true);
    }

    // Negation mixed with comparisons and logic
    [Test]
    public void Run_NotOnGreaterThanComparison_ReturnsExpected()
    {
        // not (3 > 5) => not false => true
        AssertRunBool("return not (3 > 5)", true);
    }

    [Test]
    public void Run_NotOnLessThanOrEqualComparison_ReturnsExpected()
    {
        // not (4 <= 3) => not false => true
        AssertRunBool("return not (4 <= 3)", true);
    }

    [Test]
    public void Run_NotOnInequalityComparison_ReturnsExpected()
    {
        // not (1 != 1) => not false => true
        AssertRunBool("return not (1 != 1)", true);
    }

    [Test]
    public void Run_NotTrueAndNotFalse_BothFlipped_ReturnsFalse()
    {
        // not true and not false => false and true => false
        AssertRunBool("return not true and not false", false);
    }

    [Test]
    public void Run_NotTrueOrNotFalse_ReturnsTrue()
    {
        // not true or not false => false or true => true
        AssertRunBool("return not true or not false", true);
    }

    [Test]
    public void Run_TripleNot_ReturnsInverted()
    {
        AssertRunBool("return not not not true", false);
    }

    // Negative numbers in comparisons
    [Test]
    public void Run_NegativeNumberLessThanZero_ReturnsTrue()
    {
        AssertRunBool("return -5 < 0", true);
    }

    [Test]
    public void Run_NegativeNumberGreaterThanNegative_ReturnsTrue()
    {
        AssertRunBool("return -1 > -2", true);
    }

    [Test]
    public void Run_NegativeEqualsNegative_ReturnsTrue()
    {
        AssertRunBool("return -3 == -3", true);
    }

    [Test]
    public void Run_NegativeNumberInArithmeticThenCompare_ReturnsTrue()
    {
        // -2 * -3 = 6 > 5 => true
        AssertRunBool("return -2 * -3 > 5", true);
    }

    // Mixed logic + comparison + arithmetic — deeper nesting
    [Test]
    public void Run_ArithmeticComparisonAndLogicChain_ReturnsTrue()
    {
        // (2 + 3 == 5) and (10 / 2 > 4) => true and true => true
        AssertRunBool("return (2 + 3 == 5) and (10 / 2 > 4)", true);
    }

    [Test]
    public void Run_OrOfTwoFalseComparisons_ReturnsFalse()
    {
        AssertRunBool("return (1 > 2) or (3 < 1)", false);
    }

    [Test]
    public void Run_AndPrecedenceInChain_CorrectGrouping()
    {
        // false or true and false or true
        // => false or (true and false) or true
        // left-assoc or: ((false or false) or true) => true
        AssertRunBool("return false or true and false or true", true);
    }

    [Test]
    public void Run_ParensOverrideAndPrecedence_ReturnsFalse()
    {
        // (false or true) and false => true and false => false
        AssertRunBool("return (false or true) and false", false);
    }

    [Test]
    public void Run_NotOfAndExpression_ReturnsFalse()
    {
        // not (true and true) => not true => false
        AssertRunBool("return not (true and true)", false);
    }

    [Test]
    public void Run_NotOfOrExpression_ReturnsFalse()
    {
        // not (false or true) => not true => false
        AssertRunBool("return not (false or true)", false);
    }

    [Test]
    public void Run_ComplexMixedExpression_ReturnsTrue()
    {
        // (1 + 2) * 3 == 9 and not (4 > 5) or false
        // => (9 == 9) and (not false) or false
        // => true and true or false
        // => true or false => true
        AssertRunBool("return (1 + 2) * 3 == 9 and not (4 > 5) or false", true);
    }

    [Test]
    public void Run_NegatedArithmeticInLogicChain_ReturnsTrue()
    {
        // -(2 * 3) + 10 > 0 and true => 4 > 0 and true => true and true => true
        AssertRunBool("return -(2 * 3) + 10 > 0 and true", true);
    }

    [Test]
    public void Run_DecimalComparisonInLogicChain_ReturnsTrue()
    {
        AssertRunBool("return 1.5 + 1.5 == 3.0 and 0.5 < 1.0", true);
    }

    [Test]
    public void Run_BoolCoercionInComparisonAndLogic_ReturnsTrue()
    {
        // (true + true == 2) and (false + 1 == 1)
        AssertRunBool("return (true + true == 2) and (false + 1 == 1)", true);
    }

    [Test]
    public void Run_NegatedBoolInArithmeticComparison_ReturnsExpected()
    {
        // -true is -1, -1 < 0 => true
        AssertRunBool("return -true < 0", true);
    }

    [Test]
    public void Run_NotEqualOnBools_ReturnsTrue()
    {
        AssertRunBool("return true != false", true);
    }

    [Test]
    public void Run_NotEqualOnBools_ReturnsFalseWhenSame()
    {
        AssertRunBool("return false != false", false);
    }

    // Deep nesting with varied operators
    [TestCase("return ((1 + 2) * (3 + 4)) == 21", true)]
    [TestCase("return ((10 - 3) * 2) > (6 + 7)", true)]
    [TestCase("return ((4 * 4) - (3 * 3)) == (2 * 2 + 3)", true)]
    [TestCase("return (((1 + 1) * 2) + ((3 - 1) * 3)) == 10", true)]
    [TestCase("return ((8 / (2 * 2)) + (3 * (1 + 1))) == 8", true)]
    public void Run_DeeplyNestedArithmeticInComparison(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [TestCase("return (1 < 2 and 3 < 4) and (5 < 6 and 7 < 8)", true)]
    [TestCase("return (1 > 2 or 3 > 4) or (5 > 6 or 0 < 1)", true)]
    [TestCase("return (1 < 2 and 3 > 4) or (5 < 6 and 7 > 6)", true)]
    [TestCase("return (1 > 2 or 3 < 4) and (5 < 6 or 7 > 8)", true)]
    [TestCase("return (1 > 2 and 3 > 4) or (5 > 6 and 7 > 8)", false)]
    public void Run_NestedLogicWithComparisons(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [TestCase("return not (1 < 2 and 3 < 4)", false)]
    [TestCase("return not (1 > 2 or 3 > 4)", true)]
    [TestCase("return not (not true and not false)", true)]
    [TestCase("return not (not (1 == 1))", true)]
    [TestCase("return not ((1 + 1 == 2) and (2 + 2 == 5))", true)]
    public void Run_NotOverNestedExpressions(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [TestCase("return ((2 + 3) * 2 == 10) and ((8 - 3) > 4)", true)]
    [TestCase("return ((3 * 3) >= 9) or ((4 / 2) > 3)", true)]
    [TestCase("return ((10 / 2) == 5) and ((3 + 4) != 8)", true)]
    [TestCase("return ((2 * 2 + 1) > 4) and ((3 - 1) < 3)", true)]
    [TestCase("return ((1 + 1) > 3) or ((2 * 3) <= 6)", true)]
    public void Run_ArithmeticInComparisonInLogic(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [TestCase("return not (2 + 3 > 6) and not (1 == 2)", true)]
    [TestCase("return not (4 * 2 < 7) or not (3 + 3 == 5)", true)]
    [TestCase("return not ((3 > 2) and (2 > 1)) or (1 == 1)", true)]
    [TestCase("return (not (1 > 2)) and (not (3 < 2))", true)]
    [TestCase("return not (not (2 > 1) or not (3 > 2))", true)]
    public void Run_NotMixedWithArithmeticAndLogic(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [TestCase("return (-(3 * 2) + 10 > 0) and (-(1 + 1) < 0)", true)]
    [TestCase("return -(2 + 3) == -5", true)]
    [TestCase("return (-2 * -3) == (-(- 6))", true)]
    [TestCase("return (-(4 - 6)) > 0", true)]
    [TestCase("return (-1 + -2) == -3", true)]
    public void Run_NegationInNestedArithmeticAndComparison(string source, bool expected)
    {
        AssertRunBool(source, expected);
    }

    [Test]
    public void Run_FourLevelNesting_AllOperatorTypes_ReturnsTrue()
    {
        // ((1 + 2) * 3 == 9 and not (4 > 5)) and ((6 / 2 >= 3) or (not false and 1 < 2))
        // => (9 == 9 and not false) and (3 >= 3 or true and true)
        // => (true and true) and (true or true)
        // => true and true => true
        AssertRunBool(
            "return ((1 + 2) * 3 == 9 and not (4 > 5)) and ((6 / 2 >= 3) or (not false and 1 < 2))",
            true);
    }

    [Test]
    public void Run_FourLevelNesting_AllOperatorTypes_ReturnsFalse()
    {
        // ((2 * 3 == 7) or (not (1 < 2))) and ((4 + 1 > 5) or not true)
        // => (6 == 7 or not true) and (5 > 5 or false)
        // => (false or false) and (false or false)
        // => false and false => false
        AssertRunBool(
            "return ((2 * 3 == 7) or (not (1 < 2))) and ((4 + 1 > 5) or not true)",
            false);
    }

    [Test]
    public void Run_NotOverDeepArithmeticComparison_ReturnsTrue()
    {
        // not ((2 + 3) * 2 > 11) => not (10 > 11) => not false => true
        AssertRunBool("return not ((2 + 3) * 2 > 11)", true);
    }

    [Test]
    public void Run_OrOfThreeAndClauses_CorrectPrecedence()
    {
        // (1 > 2 and 3 > 4) or (5 > 4 and 3 > 2) or (1 == 2 and true)
        // => false or true or false => true
        AssertRunBool("return (1 > 2 and 3 > 4) or (5 > 4 and 3 > 2) or (1 == 2 and true)", true);
    }

    [Test]
    public void Run_CoercedBoolInDeepNesting_ReturnsTrue()
    {
        // ((true + true) * 2 == 4) and ((false + 1) >= 1)
        // => (4 == 4) and (1 >= 1) => true
        AssertRunBool("return ((true + true) * 2 == 4) and ((false + 1) >= 1)", true);
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
