using NUnit.Framework;

namespace Mindscape.NHaml.Tests
{
  public class FunctionalTests : TestFixture
  {
    [Test]
    public void Partials2()
    {
      AssertRender("Partials2", "Partials2");
    }

    [Test]
    public void Partials()
    {
      AssertRender("Partials", "Partials");
    }

    [Test]
    public void Layout()
    {
      AssertRender("Welcome", "Application");
    }

    [Test]
    public void Welcome()
    {
      AssertRender("Welcome");
    }

    [Test]
    public void Doctype()
    {
      AssertRender("Doctype");
    }

    [Test]
    public void Escape()
    {
      AssertRender("Escape");
    }

    [Test]
    public void Empty()
    {
      AssertRender("Empty");
    }

    [Test]
    public void AutoClose()
    {
      AssertRender("AutoClose");
    }

    [Test]
    public void ReferenceExample1()
    {
      AssertRender("ReferenceExample1");
    }

    [Test]
    public void ReferenceExample2()
    {
      AssertRender("ReferenceExample2");
    }

    [Test]
    public void VeryBasic()
    {
      AssertRender("VeryBasic");
    }

    [Test]
    public void List()
    {
      AssertRender("List");
    }

    [Test]
    public void TagParsing()
    {
      AssertRender("TagParsing");
    }

    [Test]
    public void OriginalEngine()
    {
      AssertRender("OriginalEngine");
    }

    [Test]
    public void SimpleEval()
    {
      AssertRender("SimpleEval");
    }

    [Test]
    public void SilentEval()
    {
      AssertRender("SilentEval");
    }

    [Test]
    public void LoopEval()
    {
      AssertRender("LoopEval");
    }

    [Test]
    public void SwitchEval()
    {
      AssertRender("SwitchEval");
    }

    [Test]
    public void Conditionals()
    {
      AssertRender("Conditionals");
    }

    [Test]
    public void MultiLine()
    {
      AssertRender("MultiLine");
    }

    [Test]
    public void Comments()
    {
      AssertRender("Comments");
    }
  }
}