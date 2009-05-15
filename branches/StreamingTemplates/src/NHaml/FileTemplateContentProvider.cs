using System;
using System.IO;
using NHaml.Utils;

namespace NHaml
{
    public class FileTemplateContentProvider : ITemplateContentProvider
    {
        public FileTemplateContentProvider(string key)
        {
            Invariant.ArgumentNotEmpty(key, "templatePath");
            Invariant.FileExists(key);

            Key = key;
        }

        public string Key{get; private set;}
        public DateTime GetLastWriteTime()
        {
            return File.GetLastWriteTime(Key);
        }


        public StreamReader GetContentReader()
        {
            return File.OpenText(Key);
        }
    }
}