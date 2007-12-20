using Mindscape.NHaml.ViewEngine.Configuration;

using NUnit.Framework;

namespace Mindscape.NHaml.Tests.Configuration
{
  [TestFixture]
  public class ConfigurationTests
  {
    [Test]
    public void Read()
    {
      NHamlViewEngineSection section = NHamlViewEngineSection.Read();

      Assert.IsNotNull(section);
      Assert.IsTrue(section.Production);
    }
  }
}