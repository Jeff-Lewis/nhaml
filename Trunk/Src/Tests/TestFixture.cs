using System;
using System.IO;

using NUnit.Framework;

namespace Mindscape.NHaml.Tests
{
  [TestFixture]
  public abstract class TestFixture
  {
    private TemplateCompiler _templateCompiler;

    [SetUp]
    public void SetUp()
    {
      _templateCompiler = new TemplateCompiler();
    }

    protected void AssertRender(string template)
    {
      AssertRender(template, _templateCompiler);
    }

    protected void AssertRender(string template, string layout)
    {
      Type viewType = _templateCompiler.Compile(
        @"..\..\Templates\" + template + ".haml",
        @"..\..\Templates\" + layout + ".haml");

      ICompiledView view = (ICompiledView)Activator.CreateInstance(viewType);

      string output = view.Render();

      Console.WriteLine(output);

      Assert.AreEqual(File.ReadAllText(@"..\..\Results\" + layout + ".xhtml"), output);
    }

    protected static void AssertRender(string template, TemplateCompiler templateCompiler, params string[] genericArguments)
    {
      Type viewType = templateCompiler.Compile(
        @"..\..\Templates\" + template + ".haml", genericArguments);

      ICompiledView view = (ICompiledView)Activator.CreateInstance(viewType);

      string output = view.Render();

      Console.WriteLine(output);

      Assert.AreEqual(File.ReadAllText(@"..\..\Results\" + template + ".xhtml"), output);
    }
  }
}