using System;
using System.IO;

namespace ConsoleApp
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
                    codePath = arg.Substring(6);
                }
            }

            if (docsPath == "" || codePath == "")
            {
                Console.WriteLine("Usage: ConsoleApp.exe -docs=\"path/to/folder\" -code=\"path/to/folder\" -out=\"path/to/folder\"");
                return;
            }

            // Your code here
            CodeFileFinder codeFinder = new CodeFileFinder();
            _ = codeFinder.FindCodeFiles(codePath);
            ProcessDocs(docsPath, outputPath, codeFinder);
        }

        private static void ProcessDocs(string docsRoot, string outputPath, CodeFileFinder codeFinder)
        {
            string[] allMarkdown = Directory.GetFiles(docsRoot, "*.md", SearchOption.AllDirectories);
            foreach (string markdown in allMarkdown)
            {
                FileModifier.ModifyFile(markdown, docsRoot, outputPath, codeFinder);
            }
        }
    }
}