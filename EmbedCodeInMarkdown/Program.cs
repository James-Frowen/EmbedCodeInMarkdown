using System;
using System.IO;

namespace EmbedCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string docsPath = "";
            string codePath = "";
            string outputPath = "";

            foreach (string arg in args)
            {
                if (arg.StartsWith("-docs="))
                {
                    docsPath = arg.Substring(6);
                }
                else if (arg.StartsWith("-code="))
                {
                    codePath = arg.Substring(6);
                }
                else if (arg.StartsWith("-out="))
                {
                    outputPath = arg.Substring(5);
                }
            }

            if (docsPath == "")
            {
                Console.WriteLine("No -docs=");
                logUsage();
                return;
            }
            if (codePath == "")
            {
                Console.WriteLine("No -code=");
                logUsage();
                return;
            }

            if (!string.IsNullOrEmpty(outputPath))
            {
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);
            }


            // Your code here
            CodeFileFinder codeFinder = new CodeFileFinder();
            _ = codeFinder.FindCodeFiles(codePath);

            FileModifier fileModifier = new FileModifier();
            ProcessDocs(fileModifier, docsPath, outputPath, codeFinder);

            if (fileModifier.Fail)
            {
                Environment.Exit(1);
            }
        }

        private static void logUsage()
        {
            Console.WriteLine("Usage: EmbedCodeInMarkdown.exe -docs=\"path/to/folder\" -code=\"path/to/folder\" -out=\"path/to/folder\"");
        }

        private static void ProcessDocs(FileModifier fileModifier, string docsRoot, string outputPath, CodeFileFinder codeFinder)
        {
            string[] allMarkdown = Directory.GetFiles(docsRoot, "*.md", SearchOption.AllDirectories);

            foreach (string markdown in allMarkdown)
            {
                fileModifier.ModifyFile(markdown, docsRoot, outputPath, codeFinder);
            }
        }
    }
}