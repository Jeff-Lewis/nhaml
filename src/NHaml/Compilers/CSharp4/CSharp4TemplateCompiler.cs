using System;
using System.Text.RegularExpressions;

using NHaml.Compilers.CSharp2;

namespace NHaml.Compilers.CSharp4
{
    public sealed class CSharp4TemplateCompiler : CSharp2TemplateCompiler
    {
        public override string TranslateLambda( string codeLine, Match lambdaMatch )
        {
            var groups = lambdaMatch.Groups;
            var part2 = groups[2].Captures[0].Value;
            var part0 = codeLine.Substring( 0, groups[1].Length - 2 );
            var part1 = (groups[1].Captures[0].Value.Trim().EndsWith( "()", StringComparison.OrdinalIgnoreCase ) ? null : ", ");
            return string.Format("{0}{1}{2} => {{", part0, part1, part2);
        }


        public override CodeDomTemplateTypeBuilder CreateTemplateTypeBuilder(TemplateOptions options)
        {
            return new CSharp4TemplateTypeBuilder( options );
        }

    }
}