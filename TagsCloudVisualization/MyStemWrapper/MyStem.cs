using System.Diagnostics;
using System.Text;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.MyStemWrapper;

public class MyStem
{
    public string PathToMyStem { get; set; } = "mystem.exe";

    public string Parameters { get; set; } = string.Empty;

    public Result<string> Analysis(string text)
    {
        return Result.Of(() => GetResults(CreateProcess(), text));
    }

    private string GetResults(Process process, string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        process.StandardInput.BaseStream.Write(bytes, 0, bytes.Length);
        process.StandardInput.BaseStream.Flush();
        process.StandardInput.BaseStream.Close();
        var end = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return end;
    }

    private Process CreateProcess()
    {
        return Process.Start(new ProcessStartInfo()
        {
            FileName = PathToMyStem,
            Arguments = Parameters ?? string.Empty,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            StandardOutputEncoding = Encoding.UTF8
        });
    }
}