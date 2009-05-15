using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.FSharp.Compiler.CodeDom;

namespace NHaml.Compilers.FSharp
{
    internal class FSharpTemplateTypeBuilder: ITemplateTypeBuilder
    {
        private readonly CompilerParameters _compilerParameters
            = new CompilerParameters();


        [SuppressMessage( "Microsoft.Security", "CA2122" )]
        public FSharpTemplateTypeBuilder(  )
        {
            ProviderOptions = new Dictionary<string, string>();

            ProviderOptions.Add( "CompilerVersion", "v2.0" );

            _compilerParameters.GenerateInMemory = true;
            _compilerParameters.IncludeDebugInformation = false;
        }

        public string Source { get; private set; }

        public CompilerResults CompilerResults { get; private set; }

        protected Dictionary<string, string> ProviderOptions { get; private set; }

        [SuppressMessage( "Microsoft.Security", "CA2122" )]
        [SuppressMessage( "Microsoft.Portability", "CA1903" )]
        public Type Build( string source, string typeName )
        {
            BuildSource( source );

            Trace.WriteLine( Source );

            AddReferences();

            var codeProvider = new FSharpCodeProvider(  );

            CompilerResults = codeProvider
                .CompileAssemblyFromSource( _compilerParameters, Source );

            if( CompilerResults.Errors.Count == 0 )
            {
                var assembly = CompilerResults.CompiledAssembly;
                var fullTypeName = "TempNHamlNamespace." + typeName;
                return assembly.GetType(fullTypeName, true, true);
            }

            return null;
        }

        [SuppressMessage( "Microsoft.Security", "CA2122" )]
        private void AddReferences()
        {
            _compilerParameters.ReferencedAssemblies.Clear();
            //_compilerParameters.ReferencedAssemblies.Add()

            foreach( var assembly in References )
            {
                _compilerParameters.ReferencedAssemblies.Add( assembly );
            }
        }

        private void BuildSource( string source )
        {
            var sourceBuilder = new StringBuilder();

            sourceBuilder.AppendLine("#light ");
            sourceBuilder.AppendLine("namespace TempNHamlNamespace");
            foreach( var usingStatement in Usings )
            {
                sourceBuilder.AppendLine( "open " + usingStatement);
            }

            sourceBuilder.AppendLine( source);

            Source = sourceBuilder.ToString();
        }

        public IList<string> Usings { get; set; }
        public IList<string> References { get; set; }
    }
}