using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.CSharp;

namespace NHaml.Compilers.CSharp2
{
  internal class CSharp2TemplateTypeBuilder
  {
    private readonly CompilerParameters _compilerParameters
      = new CompilerParameters();

    private readonly Dictionary<string, string> _providerOptions
      = new Dictionary<string, string>();

    private readonly TemplateEngine _templateEngine;

    private CompilerResults _compilerResults;

    private string _source;

    [SuppressMessage("Microsoft.Security", "CA2122")]
    public CSharp2TemplateTypeBuilder(TemplateEngine templateEngine)
    {
      _templateEngine = templateEngine;

      _providerOptions.Add("CompilerVersion", "v2.0");

      _compilerParameters.GenerateInMemory = true;
      _compilerParameters.IncludeDebugInformation = false;
    }

    public string Source
    {
      get { return _source; }
    }

    public CompilerResults CompilerResults
    {
      get { return _compilerResults; }
    }

    protected Dictionary<string, string> ProviderOptions
    {
      get { return _providerOptions; }
    }

    [SuppressMessage("Microsoft.Security", "CA2122")]
    [SuppressMessage("Microsoft.Portability", "CA1903")]
    public Type Build(string source, string typeName)
    {
      BuildSource(source);

      Trace.WriteLine(_source);

      AddReferences();

      var codeProvider = new CSharpCodeProvider(_providerOptions);

      _compilerResults = codeProvider
        .CompileAssemblyFromSource(_compilerParameters, _source);

      if (_compilerResults.Errors.Count == 0)
      {
        return _compilerResults.CompiledAssembly.GetType(typeName);
      }

      return null;
    }

    [SuppressMessage("Microsoft.Security", "CA2122")]
    private void AddReferences()
    {
      _compilerParameters.ReferencedAssemblies.Clear();

      foreach (var assembly in _templateEngine.References)
      {
        _compilerParameters.ReferencedAssemblies.Add(assembly);
      }
    }

    private void BuildSource(string source)
    {
      var sourceBuilder = new StringBuilder();

      foreach (var usingStatement in _templateEngine.Usings)
      {
        sourceBuilder.AppendLine("using " + usingStatement + ";");
      }

      sourceBuilder.AppendLine(source);

      _source = sourceBuilder.ToString();
    }
  }
}