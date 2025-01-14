using System.Text;
using NPOI.XWPF.UserModel;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.FileReaders;

public class DocxFileReader : IFileReader
{
    public Result<string> Read(string filePath)
    {
        return Result.Of(() => File.OpenRead(filePath))
            .Then(stream =>
            {
                using var doc = new XWPFDocument(stream);
                var text = new StringBuilder();
                    
                foreach (var paragraph in doc.Paragraphs)
                    text.Append(paragraph.Text);
                    
                return text.ToString();
            });
    }
}