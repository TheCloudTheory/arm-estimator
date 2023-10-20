namespace ACE.Compilation
{
    internal interface ICompiler
    {
        string? Compile(FileInfo templateFile, CancellationToken token);
    }
}
