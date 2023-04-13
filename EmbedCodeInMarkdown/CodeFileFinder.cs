using System;
using System.Collections.Generic;
using System.IO;

namespace EmbedCode
{
    internal class CodeFileFinder
    {
        private Dictionary<string, CodeFile> fileContentsCache = new Dictionary<string, CodeFile>();
        private string _codeRoot;

        public string[] FindCodeFiles(string directoryPath)
        {
            _codeRoot = directoryPath;

            string[] codeFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

            foreach (string codeFile in codeFiles)
            {
                fileContentsCache[codeFile] = new CodeFile { Path = codeFile };
            }

            return codeFiles;
        }

        public CodeFile GetCodeFile(string filePath)
        {
            if (!Path.IsPathFullyQualified(filePath))
            {
                filePath = Path.Combine(_codeRoot, filePath);
            }

            if (!fileContentsCache.TryGetValue(filePath, out CodeFile codeFile))
            {
                throw new ArgumentException($"File path not found {filePath}");
            }

            codeFile.Load();
            return codeFile;
        }
    }
}