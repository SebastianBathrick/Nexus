using Chow.Values;
namespace Chow.Tests.Execution;

[TestFixture]
public class ChowNumberTests
{
    [Test]
    public void ChowNumber_AddTwoNumbers_ReturnsSum()
    {
        var a = new ChowNumber(2);
        var b = new ChowNumber(3);
        var sum = a + b;
        Assert.That(sum, Is.InstanceOf<ChowNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void ChowNumber_SubtractTwoNumbers_ReturnsDifference()
    {
        var a = new ChowNumber(10);
        var b = new ChowNumber(3);
        var diff = a - b;
        Assert.That(diff.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void ChowNumber_MultiplyTwoNumbers_ReturnsProduct()
    {
        var a = new ChowNumber(4);
        var b = new ChowNumber(5);
        var product = a * b;
        Assert.That(product.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void ChowNumber_DivideTwoNumbers_ReturnsQuotient()
    {
        var a = new ChowNumber(15);
        var b = new ChowNumber(3);
        var quotient = a / b;
        Assert.That(quotient.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void ChowNumber_EqualToSameValue_ReturnsTrue()
    {
        var a = new ChowNumber(1);
        var b = new ChowNumber(1);
        Assert.That(a == b, Is.True);
        Assert.That(a != b, Is.False);
        Assert.That(b != a, Is.False);
    }

    [Test]
    public void ChowNumber_EqualToDifferentValue_ReturnsFalse()
    {
        var a = new ChowNumber(1);
        var b = new ChowNumber(2);
        Assert.That(a == b, Is.False);
        Assert.That(a != b, Is.True);
    }

    [Test]
    public void ChowNumber_EqualToDouble_ReturnsTrueWhenEqual()
    {
        var a = new ChowNumber(1.5);
        Assert.That(a == 1.5, Is.True);
        Assert.That(1.5 == a, Is.True);
        Assert.That(a != 2.0, Is.True);
    }

    [Test]
    public void ChowNumber_LessThanGreaterThan_ReturnExpected()
    {
        var one = new ChowNumber(1);
        var two = new ChowNumber(2);
        Assert.That(one < two, Is.True);
        Assert.That(two > one, Is.True);
        var one2 = new ChowNumber(1);
        Assert.That(one <= one2, Is.True);
        var two2 = new ChowNumber(2);
        Assert.That(two >= two2, Is.True);
    }

    [Test]
    public void ChowNumber_ToString_ReturnsInvariantString()
    {
        Assert.That(new ChowNumber(42).ToString(), Is.EqualTo("42"));
        Assert.That(new ChowNumber(3.14).ToString(), Is.EqualTo("3.14"));
    }

    [Test]
    public void ChowNumber_EqualsNull_ReturnsFalse()
    {
        var n = new ChowNumber(1);
#pragma warning disable CS8602 // n is never null; Equals(null) is the case under test
        Assert.That(n.Equals(null), Is.False);
        Assert.That(n.Equals("not a value"), Is.False);
#pragma warning restore CS8602
    }

    [Test]
    public void ChowNumber_EqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new ChowNumber(7);
        var b = new ChowNumber(7);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void ChowNumber_AddWithChowBool_CoercesBoolAndReturnsSum()
    {
        var num = new ChowNumber(1);
        var b = new ChowBool(true);
        var sum = num + b;
        Assert.That(sum, Is.InstanceOf<ChowNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("2"));
    }
}

[TestFixture]
public class ChowBoolTests
{
    [Test]
    public void ChowBool_EqualToSameBool_ReturnsTrue()
    {
        var t1 = new ChowBool(true);
        var t2 = new ChowBool(true);
        var f1 = new ChowBool(false);
        var f2 = new ChowBool(false);
        Assert.That(t1 == t2, Is.True);
        Assert.That(f1 == f2, Is.True);
        Assert.That(t1 != f1, Is.True);
    }

    [Test]
    public void ChowBool_EqualToDouble_CoercesCorrectly()
    {
        Assert.That(new ChowBool(true) == 1.0, Is.True);
        Assert.That(new ChowBool(false) == 0.0, Is.True);
    }

    [Test]
    public void ChowBool_AddWithNumber_ReturnsChowNumber()
    {
        var b = new ChowBool(true);
        var n = new ChowNumber(2);
        var sum = b + n;
        Assert.That(sum, Is.InstanceOf<ChowNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("3"));
    }

    [Test]
    public void ChowBool_LessThanGreaterThan_WithNumber_ReturnsExpected()
    {
        var t = new ChowBool(true);
        var one = new ChowNumber(1);
        Assert.That(t < new ChowNumber(2), Is.True);
        Assert.That(t > new ChowNumber(0), Is.True);
        Assert.That(new ChowBool(false) < one, Is.True);
    }

    [Test]
    public void ChowBool_ToString_ReturnsTrueOrFalse()
    {
        Assert.That(new ChowBool(true).ToString(), Is.EqualTo("true"));
        Assert.That(new ChowBool(false).ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void ChowBool_EqualsNull_ReturnsFalse()
    {
        var b = new ChowBool(true);
        Assert.That(b.Equals(null), Is.False);
    }

    [Test]
    public void ChowBool_EqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new ChowBool(true);
        var b = new ChowBool(true);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }
}
