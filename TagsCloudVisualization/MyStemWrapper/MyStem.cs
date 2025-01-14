using System.Diagnostics;
using System.Text;

namespace TagsCloudVisualization.MyStemWrapper;

public class MyStem
{
    public string PathToMyStem { get; set; } = "mystem.exe";

    public string Parameters { get; set; } = string.Empty;

    public string Analysis(string text)
    {
        if (!File.Exists(PathToMyStem))
            throw new FileNotFoundException("Path to MyStem.exe is not valid! Change 'PathToMyStem' properties or move MyStem.exe in appropriate folder.");
        try
        {
            return GetResults(CreateProcess(), text);
        }
        catch
        {
            throw new FormatException("Invalid parameters! Look at https://tech.yandex.ru/mystem/doc/index-docpage");
        }
    }

    private string GetResults(Process process, string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        process.StandardInput.BaseStream.Write(bytes, 0, bytes.Length);
        process.StandardInput.BaseStream.Flush();
        process.StandardInput.BaseStream.Close();
        string end = process.StandardOutput.ReadToEnd();
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