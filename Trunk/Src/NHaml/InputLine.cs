using System.Text.RegularExpressions;

using Mindscape.NHaml.Exceptions;
using Mindscape.NHaml.Properties;

namespace Mindscape.NHaml
{
  public sealed class InputLine
  {
    private static readonly Regex _indentRegex
      = new Regex(@"^\s*", RegexOptions.Compiled | RegexOptions.Singleline);

    private static readonly Regex _multiLineRegex
      = new Regex(@"^.+\s+(\|\s*)$", RegexOptions.Compiled | RegexOptions.Singleline);

    private string _text;
    private string _normalizedText;

    private readonly char _signifier;
    private readonly int _lineNumber;
    private readonly string _indent;
    private readonly int _indentSize;
    private readonly bool _IsMultiline;

    public InputLine(string text, int lineNumber)
    {
      _text = text;
      _lineNumber = lineNumber;

      Match match = _multiLineRegex.Match(_text);

      _IsMultiline = match.Success;

      if (_IsMultiline)
      {
        _text = _text.Remove(match.Groups[1].Index);
      }

      _normalizedText = _text.Trim();

      if (!string.IsNullOrEmpty(_normalizedText))
      {
        _signifier = _normalizedText[0];
        _normalizedText = _normalizedText.Remove(0, 1);
      }

      _indent = _indentRegex.Match(_text).Groups[0].Value;

      ValidateIndentation();

      _indentSize = _indent.Length / 2;
    }

    public char Signifier
    {
      get { return _signifier; }
    }

    public string Text
    {
      get { return _text; }
    }

    public string NormalizedText
    {
      get { return _normalizedText; }
    }

    public string Indent
    {
      get { return _indent; }
    }

    public int IndentSize
    {
      get { return _indentSize; }
    }

    public int LineNumber
    {
      get { return _lineNumber; }
    }

    public bool IsMultiline
    {
      get { return _IsMultiline; }
    }

    public void Merge(InputLine nextInputLine)
    {
      _text += nextInputLine.Text;
      _normalizedText += nextInputLine.Text;
    }

    public override string ToString()
    {
      return LineNumber + ": " + Text;
    }

    private void ValidateIndentation()
    {
      if (_indent.Contains("\t") || ((_indent.Length % 2) != 0))
      {
        SyntaxException.Throw(this, Resources.IllegalIndentation);
      }
    }
  }
}