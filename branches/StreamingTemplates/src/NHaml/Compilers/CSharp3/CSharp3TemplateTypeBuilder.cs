using NHaml.Compilers.CSharp2;

namespace NHaml.Compilers.CSharp3
{
    internal sealed class CSharp3TemplateTypeBuilder : CSharp2TemplateTypeBuilder
    {
        public CSharp3TemplateTypeBuilder( )
        {
            ProviderOptions["CompilerVersion"] = "v3.5";
        }
    }
}