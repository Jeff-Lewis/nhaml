using NUnit.Framework;

namespace NHaml.Tests
{
  [TestFixture]
  public class InputLineTests
  {
    [Test]
    public void DocType()
    {
      var inputLine = new InputLine("!!!", 1);

      Assert.AreEqual(0, inputLine.IndentCount);
      Assert.AreEqual(string.Empty, inputLine.Indent);
      Assert.AreEqual('!', inputLine.Signifier);
      Assert.AreEqual("!!", inputLine.NormalizedText);
    }

    [Test]
    public void Trimmed()
    {
      var inputLine = new InputLine("  %head ", 1);

      Assert.AreEqual("  %head ", inputLine.Text);
      Assert.AreEqual("head ", inputLine.NormalizedText);
    }

    [Test]
    public void Indent()
    {
      var inputLine = new InputLine("%head", 1);

      Assert.AreEqual(0, inputLine.IndentCount);
      Assert.AreEqual(string.Empty, inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("head", inputLine.NormalizedText);

      inputLine = new InputLine("  %head", 1);

      Assert.AreEqual(1, inputLine.IndentCount);
      Assert.AreEqual("  ", inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("head", inputLine.NormalizedText);

      inputLine = new InputLine("    %body", 1);

      Assert.AreEqual(2, inputLine.IndentCount);
      Assert.AreEqual("    ", inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("body", inputLine.NormalizedText);
    }
  }
}