using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    internal class CodeFileFinder
    {
        private Dictionary<string, CodeFile> fileContentsCache = new Dictionary<string, CodeFile>();

        public string[] FindCodeFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
        }

        public CodeFile GetCodeFile(string filePath)
        {
            if (!fileContentsCache.ContainsKey(filePath))
            {
                CodeFile codeFile = new CodeFile { Path = filePath };
                codeFile.Load();
                fileContentsCache[filePath] = codeFile;
            }

            return fileContentsCache[filePath];
        }
    }
}