using Nexus.Execution.Values;

namespace Nexus.Tests;

[TestFixture]
public class NexusNumberTests
{
    [Test]
    public void NexusNumber_AddTwoNumbers_ReturnsSum()
    {
        var a = new NexusNumber(2);
        var b = new NexusNumber(3);
        var sum = a + b;
        Assert.That(sum, Is.InstanceOf<NexusNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void NexusNumber_SubtractTwoNumbers_ReturnsDifference()
    {
        var a = new NexusNumber(10);
        var b = new NexusNumber(3);
        var diff = a - b;
        Assert.That(diff.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void NexusNumber_MultiplyTwoNumbers_ReturnsProduct()
    {
        var a = new NexusNumber(4);
        var b = new NexusNumber(5);
        var product = a * b;
        Assert.That(product.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void NexusNumber_DivideTwoNumbers_ReturnsQuotient()
    {
        var a = new NexusNumber(15);
        var b = new NexusNumber(3);
        var quotient = a / b;
        Assert.That(quotient.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void NexusNumber_EqualToSameValue_ReturnsTrue()
    {
        var a = new NexusNumber(1);
        var b = new NexusNumber(1);
        Assert.That(a == b, Is.True);
        Assert.That(a != b, Is.False);
        Assert.That(b != a, Is.False);
    }

    [Test]
    public void NexusNumber_EqualToDifferentValue_ReturnsFalse()
    {
        var a = new NexusNumber(1);
        var b = new NexusNumber(2);
        Assert.That(a == b, Is.False);
        Assert.That(a != b, Is.True);
    }

    [Test]
    public void NexusNumber_EqualToDouble_ReturnsTrueWhenEqual()
    {
        var a = new NexusNumber(1.5);
        Assert.That(a == 1.5, Is.True);
        Assert.That(1.5 == a, Is.True);
        Assert.That(a != 2.0, Is.True);
    }

    [Test]
    public void NexusNumber_LessThanGreaterThan_ReturnExpected()
    {
        var one = new NexusNumber(1);
        var two = new NexusNumber(2);
        Assert.That(one < two, Is.True);
        Assert.That(two > one, Is.True);
        var one2 = new NexusNumber(1);
        Assert.That(one <= one2, Is.True);
        var two2 = new NexusNumber(2);
        Assert.That(two >= two2, Is.True);
    }

    [Test]
    public void NexusNumber_ToString_ReturnsInvariantString()
    {
        Assert.That(new NexusNumber(42).ToString(), Is.EqualTo("42"));
        Assert.That(new NexusNumber(3.14).ToString(), Is.EqualTo("3.14"));
    }

    [Test]
    public void NexusNumber_EqualsNull_ReturnsFalse()
    {
        var n = new NexusNumber(1);
#pragma warning disable CS8602 // n is never null; Equals(null) is the case under test
        Assert.That(n.Equals(null), Is.False);
        Assert.That(n.Equals("not a value"), Is.False);
#pragma warning restore CS8602
    }

    [Test]
    public void NexusNumber_EqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new NexusNumber(7);
        var b = new NexusNumber(7);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void NexusNumber_AddWithNexusBool_CoercesBoolAndReturnsSum()
    {
        var num = new NexusNumber(1);
        var b = new NexusBool(true);
        var sum = num + b;
        Assert.That(sum, Is.InstanceOf<NexusNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("2"));
    }
}

[TestFixture]
public class NexusBoolTests
{
    [Test]
    public void NexusBool_EqualToSameBool_ReturnsTrue()
    {
        var t1 = new NexusBool(true);
        var t2 = new NexusBool(true);
        var f1 = new NexusBool(false);
        var f2 = new NexusBool(false);
        Assert.That(t1 == t2, Is.True);
        Assert.That(f1 == f2, Is.True);
        Assert.That(t1 != f1, Is.True);
    }

    [Test]
    public void NexusBool_EqualToDouble_CoercesCorrectly()
    {
        Assert.That(new NexusBool(true) == 1.0, Is.True);
        Assert.That(new NexusBool(false) == 0.0, Is.True);
    }

    [Test]
    public void NexusBool_AddWithNumber_ReturnsNexusNumber()
    {
        var b = new NexusBool(true);
        var n = new NexusNumber(2);
        var sum = b + n;
        Assert.That(sum, Is.InstanceOf<NexusNumber>());
        Assert.That(sum.ToString(), Is.EqualTo("3"));
    }

    [Test]
    public void NexusBool_LessThanGreaterThan_WithNumber_ReturnsExpected()
    {
        var t = new NexusBool(true);
        var one = new NexusNumber(1);
        Assert.That(t < new NexusNumber(2), Is.True);
        Assert.That(t > new NexusNumber(0), Is.True);
        Assert.That(new NexusBool(false) < one, Is.True);
    }

    [Test]
    public void NexusBool_ToString_ReturnsTrueOrFalse()
    {
        Assert.That(new NexusBool(true).ToString(), Is.EqualTo("true"));
        Assert.That(new NexusBool(false).ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void NexusBool_EqualsNull_ReturnsFalse()
    {
        var b = new NexusBool(true);
        Assert.That(b.Equals(null), Is.False);
    }

    [Test]
    public void NexusBool_EqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new NexusBool(true);
        var b = new NexusBool(true);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }
}
