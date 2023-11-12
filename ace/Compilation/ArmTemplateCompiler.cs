using System.Text.RegularExpressions;

namespace ACE.Compilation
{
    internal class ArmTemplateCompiler : ICompiler
    {
        public string? Compile(FileInfo templateFile, CancellationToken token)
        {
            return Regex.Replace(File.ReadAllText(templateFile.FullName), @"\s+", string.Empty);
        }
    }
}
