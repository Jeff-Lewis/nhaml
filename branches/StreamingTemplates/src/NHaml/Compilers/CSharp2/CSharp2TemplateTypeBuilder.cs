using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.CSharp;

namespace NHaml.Compilers.CSharp2
{
    internal class CSharp2TemplateTypeBuilder : ITemplateTypeBuilder
    {
        private readonly CompilerParameters _compilerParameters
          = new CompilerParameters();


        [SuppressMessage( "Microsoft.Security", "CA2122" )]
        public CSharp2TemplateTypeBuilder( )
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

            var codeProvider = new CSharpCodeProvider( ProviderOptions );

            CompilerResults = codeProvider
              .CompileAssemblyFromSource( _compilerParameters, Source );

            if( CompilerResults.Errors.Count == 0 )
            {
                return CompilerResults.CompiledAssembly.GetType( typeName );
            }

            return null;
        }

        [SuppressMessage( "Microsoft.Security", "CA2122" )]
        private void AddReferences()
        {
            _compilerParameters.ReferencedAssemblies.Clear();

            foreach( var assembly in References )
            {
                _compilerParameters.ReferencedAssemblies.Add( assembly );
            }
        }

        private void BuildSource( string source )
        {
            var sourceBuilder = new StringBuilder();

            foreach( var usingStatement in Usings )
            {
                sourceBuilder.AppendLine( string.Format("using {0};", usingStatement) );
            }

            sourceBuilder.AppendLine( source );

            Source = sourceBuilder.ToString();
        }

        public IList<string> Usings { get; set; }
        public IList<string> References { get; set; }
    }
}