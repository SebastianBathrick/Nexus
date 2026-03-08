using Chow.Interpretation;
namespace Chow.Tests.Execution;

[TestFixture]
public class TaggedUnionNumberTests
{
    [Test]
    public void TaggedUnion_AddTwoNumbers_ReturnsSum()
    {
        var a = new TaggedUnion(2.0);
        var b = new TaggedUnion(3.0);
        var sum = a + b;
        Assert.That(sum.Tag, Is.EqualTo(TagType.Number));
        Assert.That(sum.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void TaggedUnion_SubtractTwoNumbers_ReturnsDifference()
    {
        var a = new TaggedUnion(10.0);
        var b = new TaggedUnion(3.0);
        var diff = a - b;
        Assert.That(diff.ToString(), Is.EqualTo("7"));
    }

    [Test]
    public void TaggedUnion_MultiplyTwoNumbers_ReturnsProduct()
    {
        var a = new TaggedUnion(4.0);
        var b = new TaggedUnion(5.0);
        var product = a * b;
        Assert.That(product.ToString(), Is.EqualTo("20"));
    }

    [Test]
    public void TaggedUnion_DivideTwoNumbers_ReturnsQuotient()
    {
        var a = new TaggedUnion(15.0);
        var b = new TaggedUnion(3.0);
        var quotient = a / b;
        Assert.That(quotient.ToString(), Is.EqualTo("5"));
    }

    [Test]
    public void TaggedUnion_EqualToSameValue_ReturnsTrue()
    {
        var a = new TaggedUnion(1.0);
        var b = new TaggedUnion(1.0);
        Assert.That((a == b).BoolValue, Is.True);
        Assert.That((a != b).BoolValue, Is.False);
        Assert.That((b != a).BoolValue, Is.False);
    }

    [Test]
    public void TaggedUnion_EqualToDifferentValue_ReturnsFalse()
    {
        var a = new TaggedUnion(1.0);
        var b = new TaggedUnion(2.0);
        Assert.That((a == b).BoolValue, Is.False);
        Assert.That((a != b).BoolValue, Is.True);
    }

    [Test]
    public void TaggedUnion_LessThanGreaterThan_ReturnExpected()
    {
        var one = new TaggedUnion(1.0);
        var two = new TaggedUnion(2.0);
        Assert.That((one < two).BoolValue, Is.True);
        Assert.That((two > one).BoolValue, Is.True);
        var one2 = new TaggedUnion(1.0);
        Assert.That((one <= one2).BoolValue, Is.True);
        var two2 = new TaggedUnion(2.0);
        Assert.That((two >= two2).BoolValue, Is.True);
    }

    [Test]
    public void TaggedUnion_ToString_ReturnsInvariantString()
    {
        Assert.That(new TaggedUnion(42.0).ToString(), Is.EqualTo("42"));
        Assert.That(new TaggedUnion(3.14).ToString(), Is.EqualTo("3.14"));
    }

    [Test]
    public void TaggedUnion_EqualsNull_ReturnsFalse()
    {
        var n = new TaggedUnion(1.0);
        Assert.That(n.Equals(null), Is.False);
        Assert.That(n.Equals("not a value"), Is.False);
    }

    [Test]
    public void TaggedUnion_EqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new TaggedUnion(7.0);
        var b = new TaggedUnion(7.0);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void TaggedUnion_AddNumberWithBool_CoercesBoolAndReturnsSum()
    {
        var num = new TaggedUnion(1.0);
        var b = new TaggedUnion(true);
        var sum = num + b;
        Assert.That(sum.Tag, Is.EqualTo(TagType.Number));
        Assert.That(sum.ToString(), Is.EqualTo("2"));
    }
}

[TestFixture]
public class TaggedUnionBoolTests
{
    [Test]
    public void TaggedUnion_EqualToSameBool_ReturnsTrue()
    {
        var t1 = new TaggedUnion(true);
        var t2 = new TaggedUnion(true);
        var f1 = new TaggedUnion(false);
        var f2 = new TaggedUnion(false);
        Assert.That((t1 == t2).BoolValue, Is.True);
        Assert.That((f1 == f2).BoolValue, Is.True);
        Assert.That((t1 != f1).BoolValue, Is.True);
    }

    [Test]
    public void TaggedUnion_BoolEqualToNumber_CoercesCorrectly()
    {
        Assert.That((new TaggedUnion(true) == new TaggedUnion(1.0)).BoolValue, Is.True);
        Assert.That((new TaggedUnion(false) == new TaggedUnion(0.0)).BoolValue, Is.True);
    }

    [Test]
    public void TaggedUnion_BoolAddWithNumber_ReturnsNumber()
    {
        var b = new TaggedUnion(true);
        var n = new TaggedUnion(2.0);
        var sum = b + n;
        Assert.That(sum.Tag, Is.EqualTo(TagType.Number));
        Assert.That(sum.ToString(), Is.EqualTo("3"));
    }

    [Test]
    public void TaggedUnion_BoolLessThanGreaterThan_WithNumber_ReturnsExpected()
    {
        var t = new TaggedUnion(true);
        var one = new TaggedUnion(1.0);
        Assert.That((t < new TaggedUnion(2.0)).BoolValue, Is.True);
        Assert.That((t > new TaggedUnion(0.0)).BoolValue, Is.True);
        Assert.That((new TaggedUnion(false) < one).BoolValue, Is.True);
    }

    [Test]
    public void TaggedUnion_BoolToString_ReturnsTrueOrFalse()
    {
        Assert.That(new TaggedUnion(true).ToString(), Is.EqualTo("true"));
        Assert.That(new TaggedUnion(false).ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void TaggedUnion_BoolEqualsNull_ReturnsFalse()
    {
        var b = new TaggedUnion(true);
        Assert.That(b.Equals(null), Is.False);
    }

    [Test]
    public void TaggedUnion_BoolEqualsSameValue_ReturnsTrueAndMatchingHashCode()
    {
        var a = new TaggedUnion(true);
        var b = new TaggedUnion(true);
        Assert.That(a.Equals(b), Is.True);
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }
}
