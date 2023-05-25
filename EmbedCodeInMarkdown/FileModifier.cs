using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmbedCode
{
    internal class FileModifier
    {
        public bool Fail { get; private set; }
        public void ModifyFile(string filePath, string fromRoot, string outputRoot, CodeFileFinder codeFinder)
        {
            string fileContents = File.ReadAllText(filePath);
            List<MarkdownBlock> blocks = BlockFinder.FindBlocks(fileContents);

            // skip writing if there are no code blocks
            if (blocks.Count == 0)
                return;

            StringBuilder sb = new StringBuilder();
            int currentIndex = 0;
            foreach (MarkdownBlock block in blocks)
            {
                // Add the text before the block
                sb.Append(fileContents.Substring(currentIndex, block.StartIndex - currentIndex));

                // Add the code result
                string codeResult = GetCodeResult(block, codeFinder);
                sb.Append(codeResult);

                currentIndex = block.EndIndex;
            }
            // Add the remaining text after the last block
            sb.Append(fileContents.Substring(currentIndex));

            // Save the modified file contents
            string outPath = OutFilePath(filePath, fromRoot, outputRoot);
            File.WriteAllText(outPath, sb.ToString());
        }

        private static string OutFilePath(string fullPath, string fromRoot, string outputRoot)
        {
            if (string.IsNullOrEmpty(outputRoot))
                return fullPath;

            string filePath = Path.GetRelativePath(fromRoot, fullPath);
            return Path.Combine(outputRoot, filePath);
        }


        private string GetCodeResult(MarkdownBlock block, CodeFileFinder codeFinder)
        {
            CodeFile codeFile = codeFinder.GetCodeFile(block.Path);

            if (codeFile.CodeBlocks.TryGetValue(block.Name, out CodeBlock codeBlock))
            {
                string code = codeFile.GetCode(codeBlock);
                string codeResult = EmbedCode(code);

                return codeResult;
            }
            else
            {
                // Log error message to standard error stream
                Console.Error.WriteLine("Error: Code block with name '{0}' not found.", block.Name);
                Fail = false;
                return string.Empty;
            }
        }

        private static string EmbedCode(string code)
        {
            // Remove extra indentation
            string[] lines = code.Split(Environment.NewLine);
            RemoveIdenation(lines);

            GetStartEnd(lines, out int start, out int end);
            if (start > end)// no valid lines
            {
                // empty block
                return $"```cs{Environment.NewLine}```";
            }

            string joinedCode = string.Join(Environment.NewLine, lines, start, end - start + 1);
            // Add markdown code block
            string codeResult = AddMarkdownCodeBlock(joinedCode);

            return codeResult;
        }


        private static string AddMarkdownCodeBlock(string codeResult)
        {
            codeResult = $"```cs{Environment.NewLine}{codeResult}{Environment.NewLine}```";
            return codeResult;
        }

        private static void RemoveIdenation(string[] lines)
        {
            int minIndent = int.MaxValue;

            foreach (string line in lines)
            {
                if (line.Trim().Length > 0)
                {
                    int indent = line.Length - line.TrimStart().Length;
                    minIndent = Math.Min(minIndent, indent);
                }
            }

            if (minIndent < int.MaxValue) // was set
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Trim().Length > 0)
                    {
                        lines[i] = lines[i].Substring(minIndent);
                    }
                }
            }
        }

        private static void GetStartEnd(string[] lines, out int start, out int end)
        {
            start = 0;
            end = lines.Length - 1;

            while (start <= end && string.IsNullOrEmpty(lines[start]))
            {
                start++;
            }

            while (end >= start && string.IsNullOrEmpty(lines[end]))
            {
                end--;
            }
        }
    }
}