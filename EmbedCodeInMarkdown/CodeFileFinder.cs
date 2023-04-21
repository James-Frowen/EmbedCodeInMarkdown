using System;
using System.Collections.Generic;
using System.IO;

namespace EmbedCode
{
    internal class CodeFileFinder
    {
        private Dictionary<string, CodeFile> fileContentsCache = new Dictionary<string, CodeFile>();
        private string _codeRoot;

        public void FindCodeFiles(string directoryPath)
        {
            _codeRoot = directoryPath;

            string[] codeFiles = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);

            foreach (string codeFile in codeFiles)
            {
                string normalizedPath = Path.GetFullPath(codeFile);
                fileContentsCache[normalizedPath] = new CodeFile { Path = normalizedPath };
            }
        }

        public CodeFile GetCodeFile(string filePath)
        {
            if (!Path.IsPathFullyQualified(filePath))
            {
                filePath = Path.Combine(_codeRoot, filePath);
            }

            string normalizedPath = Path.GetFullPath(filePath);


            if (!fileContentsCache.TryGetValue(normalizedPath, out CodeFile codeFile))
            {
                throw new ArgumentException($"File path not found {normalizedPath}");
            }

            codeFile.Load();
            return codeFile;
        }
    }
}