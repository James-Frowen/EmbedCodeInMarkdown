using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal class CodeFile
    {
        public string Path { get; set; }
        public string FileContents { get; private set; }
        public Dictionary<string, CodeBlock> CodeBlocks { get; private set; }

        public void Load()
        {
            FileContents = File.ReadAllText(Path);
            CodeBlocks = new Dictionary<string, CodeBlock>();

            Regex startRegex = new Regex(@"// CodeEmbed-Start: (?<name>.+)");
            MatchCollection startMatches = startRegex.Matches(FileContents);

            Regex endRegex = new Regex(@"// CodeEmbed-End: (?<name>.+)");
            MatchCollection endMatches = endRegex.Matches(FileContents);

            foreach (Match startMatch in startMatches)
            {
                string name = startMatch.Groups["name"].Value;
                bool endFound = false;

                foreach (Match endMatch in endMatches)
                {
                    if (endMatch.Groups["name"].Value == name)
                    {
                        int startIndex = FileContents.IndexOf('\n', startMatch.Index) + 1;
                        int endIndex = FileContents.LastIndexOf('\n', endMatch.Index);

                        CodeBlock codeBlock = new CodeBlock
                        {
                            StartIndex = startIndex,
                            EndIndex = endIndex
                        };

                        CodeBlocks[name] = codeBlock;
                        endFound = true;
                        break;
                    }
                }

                if (!endFound)
                {
                    throw new Exception($"End comment not found for start comment with name '{name}'");
                }
            }
        }
    }
}