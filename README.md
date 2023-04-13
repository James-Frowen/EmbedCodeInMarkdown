# Embedding Code in Markdown

This program allows you to embed code from external files into your markdown files using special comments.

## How to Use

To embed a block of code from an external file into your markdown file, you need to add a special comment in the following format:

```
{{{ Path:'path/to/code' Name:'block-name' }}}
```

This comment specifies the path to the code file and the name of the code block that you want to embed.

In the code file, you need to add `CodeEmbed-Start` and `CodeEmbed-End` comments around the block of code that you want to embed. The comments should have the following format:

```cs
// CodeEmbed-Start: block-name 
… your code here … 
// CodeEmbed-End: block-name
```

The `block-name` in the `CodeEmbed-Start` and `CodeEmbed-End` comments should match the `Name` specified in the markdown file.

## Running the Program

To run the program, you need to pass in the path to the markdown file and the path to the directory containing your code files as command-line arguments. Here's an example:

```sh
dotnet run – -docs=path/to/markdown/directory -code=path/to/code/directory
```

This will modify the markdown file by replacing all special comments with the corresponding blocks of code from the specified code files.

