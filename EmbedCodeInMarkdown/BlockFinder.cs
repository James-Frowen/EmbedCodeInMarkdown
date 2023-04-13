using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal static class BlockFinder
    {
        public static List<MarkdownBlock> FindBlocks(string fileContents)
        {
            List<MarkdownBlock> blocks = new List<MarkdownBlock>();

            Regex blockRegex = new Regex(@"{{{ Path:'(?<path>.+?)' Name:'(?<name>.+?)' }}}");
            MatchCollection matches = blockRegex.Matches(fileContents);

            foreach (Match match in matches)
            {
                MarkdownBlock block = new MarkdownBlock
                {
                    StartIndex = match.Index,
                    EndIndex = match.Index + match.Length,
                    Path = match.Groups["path"].Value,
                    Name = match.Groups["name"].Value
                };

                blocks.Add(block);
            }

            return blocks;
        }
    }
}