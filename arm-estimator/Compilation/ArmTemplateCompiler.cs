using System.Text.RegularExpressions;

namespace ACE.Compilation
{
    internal class ArmTemplateCompiler : ICompiler
    {
        public string? Compile(FileInfo templateFile)
        {
            return Regex.Replace(File.ReadAllText(templateFile.FullName), @"\s+", string.Empty);
        }
    }
}
