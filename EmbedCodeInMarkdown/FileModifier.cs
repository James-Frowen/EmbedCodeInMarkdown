using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    internal static class FileModifier
    {
        public static void ModifyFile(string filePath, CodeFileFinder codeFinder)
        {
            string fileContents = File.ReadAllText(filePath);
            StringBuilder sb = new StringBuilder(fileContents);


            List<MarkdownBlock> blocks = BlockFinder.FindBlocks(fileContents);

            foreach (MarkdownBlock block in blocks)
            {
                ReplaceBlockWithCodeResult(sb, block, codeFinder);
            }

            // Save the modified file contents
            File.WriteAllText(filePath, sb.ToString());
        }

        private static void ReplaceBlockWithCodeResult(StringBuilder sb, MarkdownBlock block, CodeFileFinder codeFinder)
        {
            CodeFile codeFile = codeFinder.GetCodeFile(block.Path);

            if (codeFile.CodeBlocks.TryGetValue(block.Name, out CodeBlock codeBlock))
            {
                string codeResult = codeFile.FileContents.Substring(codeBlock.StartIndex, codeBlock.EndIndex - codeBlock.StartIndex);
                sb.Remove(block.StartIndex, block.EndIndex - block.StartIndex);
                sb.Insert(block.StartIndex, codeResult);
            }
        }
    }
}