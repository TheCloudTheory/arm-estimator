using System.Diagnostics;

namespace ACE_Tests;

public class TerraformBase
{
    protected void InitializeAndCreateTerraformPlan(string workingDirectory)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "terraform";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.Arguments = "init -no-color -upgrade";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = workingDirectory;

            string? error = null;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

            process.Start();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error) == false)
            {
                Console.WriteLine(error);
            }

            Assert.That(string.IsNullOrEmpty(error), Is.True);
        }

        using (var process = new Process())
        {
            process.StartInfo.FileName = "terraform";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.Arguments = "plan -out tfplan -no-color";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = workingDirectory;

            string? error = null;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error) == false)
            {
                Console.WriteLine(error);
            }

            Assert.That(string.IsNullOrEmpty(error), Is.True);
        }
    }
}