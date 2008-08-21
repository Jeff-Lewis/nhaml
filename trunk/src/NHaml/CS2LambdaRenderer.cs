using System;
using System.Text.RegularExpressions;

namespace NHaml
{
  internal class CS2LambdaRenderer : ILambdaRenderer
  {
    public string Render(string codeLine, Match lambdaMatch)
    {
      return codeLine.Substring(0, lambdaMatch.Groups[1].Length - 2)
        + (lambdaMatch.Groups[1].Captures[0].Value.Trim().EndsWith("()", StringComparison.OrdinalIgnoreCase) ? null : ", ")
          + "delegate" + lambdaMatch.Groups[2].Captures[0].Value + "{";
    }
  }
}