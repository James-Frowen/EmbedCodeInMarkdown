using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace EmbedCode
{
    internal class CodeFile
    {
        public string Path { get; set; }
        public bool HasLoaded { get; private set; }
        public string FileContents { get; private set; }
        public Dictionary<string, CodeBlock> CodeBlocks { get; private set; }

        public void Load()
        {
            if (HasLoaded)
                return;

            HasLoaded = true;

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
                        int startIndex = FileContents.IndexOf(Environment.NewLine, startMatch.Index) + 2;
                        int endIndex = FileContents.LastIndexOf(Environment.NewLine, endMatch.Index);

                        if (name.EndsWith("\r"))
                            name = name.Substring(0, name.Length - 1);

                        CodeBlock codeBlock = new CodeBlock
                        {
                            Name = name,
                            StartIndex = startIndex,
                            EndIndex = endIndex
                        };

                        //LogCodeBlock(name, codeBlock);


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

        private void LogCodeBlock(string name, CodeBlock codeBlock)
        {
            string codeText = GetCode(codeBlock);
            string[] lines = codeText.Split(Environment.NewLine);

            Console.WriteLine($"---start-{name}---");
            foreach (string line in lines)
                Console.WriteLine(line);

            Console.WriteLine($"---end-{name}---");
        }

        public string GetCode(CodeBlock block)
        {
            int count = block.EndIndex - block.StartIndex;
            if (count <= 0)
            {
                Console.WriteLine($"WARNING: empty code block. File:{Path} BlockName:{block.Name}");
                return string.Empty;
            }

            return FileContents.Substring(block.StartIndex, count);
        }
    }
}