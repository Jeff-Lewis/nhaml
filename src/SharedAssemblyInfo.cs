using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

[assembly: AssemblyProduct("NHaml")]
[assembly: AssemblyCopyright("MIT License")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.5.0.0")]
[assembly: AssemblyFileVersion("1.5.0.0")]
[assembly: NeutralResourcesLanguage("")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

#if !NOT_CLSCOMPLIANT
[assembly: System.CLSCompliant( true )]
#endif
