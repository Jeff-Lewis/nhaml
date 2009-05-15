using System;
using System.Collections.Generic;

namespace NHaml.Compilers
{
    public interface ITemplateCompiler
    {
        TemplateFactory Compile( TemplateParser templateParser );
        BlockClosingAction RenderSilentEval( TemplateParser templateParser );
        TemplateClassBuilder CreateTemplateClassBuilder( string className, Type templateBaseType );

    }
    public interface ITemplateTypeBuilder
    {
        IList<string> Usings { get; set; }
        IList<string> References { get; set; }
    }
}