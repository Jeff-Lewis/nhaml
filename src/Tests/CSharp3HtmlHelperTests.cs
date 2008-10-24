using System;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Moq;

using NHaml.Backends.CSharp3;
using NHaml.Samples.Mvc.Models;
using NHaml.Web.Mvc;

using NUnit.Framework;

namespace NHaml.Tests
{
  [TestFixture]
  public class CSharp3HtmlHelperTests : TestFixtureBase
  {
    public override void SetUp()
    {
      base.SetUp();

      _templateCompiler.CompilerBackend = new CSharp3CompilerBackend();
      _templateCompiler.ViewBaseType = typeof(MockView);

      _templateCompiler.AddReferences(typeof(IViewDataContainer));
      _templateCompiler.AddReference(typeof(Expression).Assembly.Location);
      _templateCompiler.AddReference(typeof(Route).Assembly.Location);
      _templateCompiler.AddReference(typeof(NHamlHtmlHelper).Assembly.Location);
      _templateCompiler.AddReference(typeof(Product).Assembly.Location);

      _templateCompiler.AddUsing("NHaml.Samples.Mvc.Models");
      _templateCompiler.AddUsing("NHaml.Samples.Mvc.Controllers");
    }

    [Test]
    public void FormHelper()
    {
      AssertRender("FormHelperCS3", "FormHelperCS3", _templateCompiler);
    }

    public abstract class MockView : NHamlMvcView<object>
    {
      private NHamlHtmlHelper _html;

      public override void Render(TextWriter output)
      {
        // lol!

        var mockViewDataContainer = new Mock<IViewDataContainer>();
        mockViewDataContainer.ExpectGet(v => v.ViewData).Returns(new ViewDataDictionary());

        _html = new NHamlHtmlHelper(
          Output,
          new ViewContext(
            new Mock<HttpContextBase>().Object,
            new RouteData(),
            new Mock<ControllerBase>().Object,
            this,
            new ViewDataDictionary(),
            new TempDataDictionary()),
          mockViewDataContainer.Object);
      }

      public new NHamlHtmlHelper Html
      {
        get { return _html; }
      }
    }
  }
}