using Mindscape.NHaml.Exceptions;

using NUnit.Framework;

namespace Mindscape.NHaml.Tests
{
  [TestFixture]
  public class InputLineTests
  {
    [Test]
    public void DocType()
    {
      InputLine inputLine = new InputLine("!!!", 1);

      Assert.AreEqual(0, inputLine.IndentSize);
      Assert.AreEqual(string.Empty, inputLine.Indent);
      Assert.AreEqual('!', inputLine.Signifier);
      Assert.AreEqual("!!", inputLine.NormalizedText);
    }

    [Test]
    public void RTrimmed()
    {
      InputLine inputLine = new InputLine("  %head ", 1);

      Assert.AreEqual("  %head ", inputLine.Text);
      Assert.AreEqual("head", inputLine.NormalizedText);
    }

    [Test]
    [ExpectedException(typeof(SyntaxException))]
    public void ContainsTab()
    {
      new InputLine("  \t%head", 1);
    }

    [Test]
    [ExpectedException(typeof(SyntaxException))]
    public void OddNumberOfSpaces()
    {
      new InputLine(" %head", 1);
    }

    [Test]
    public void Indent()
    {
      InputLine inputLine = new InputLine("%head", 1);

      Assert.AreEqual(0, inputLine.IndentSize);
      Assert.AreEqual(string.Empty, inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("head", inputLine.NormalizedText);

      inputLine = new InputLine("  %head", 1);

      Assert.AreEqual(1, inputLine.IndentSize);
      Assert.AreEqual("  ", inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("head", inputLine.NormalizedText);

      inputLine = new InputLine("    %body", 1);

      Assert.AreEqual(2, inputLine.IndentSize);
      Assert.AreEqual("    ", inputLine.Indent);
      Assert.AreEqual('%', inputLine.Signifier);
      Assert.AreEqual("body", inputLine.NormalizedText);
    }
  }
}