using System;
using System.IO;

namespace NHaml
{
    public interface ITemplateContentProvider
    {
        string Key { get; }
        DateTime GetLastWriteTime();
        StreamReader GetContentReader();
    }
}