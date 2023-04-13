using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    internal static class FileModifier
    {
        public static void ModifyFile(string filePath, string fromRoot, string outputRoot, CodeFileFinder codeFinder)
        {
            string fileContents = File.ReadAllText(filePath);
            StringBuilder sb = new StringBuilder(fileContents);


            List<MarkdownBlock> blocks = BlockFinder.FindBlocks(fileContents);

            foreach (MarkdownBlock block in blocks)
            {
                ReplaceBlockWithCodeResult(sb, block, codeFinder);
            }

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