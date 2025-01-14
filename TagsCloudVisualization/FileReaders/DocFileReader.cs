using System.Text;
using Spire.Doc;
using Spire.Doc.Documents;
using TagsCloudVisualization.FuncMonad;

namespace TagsCloudVisualization.FileReaders;

public class DocFileReader : IFileReader
{
    public Result<string> Read(string filePath)
    {
        return Result.Of(() =>
        {
            var text = new StringBuilder();
            var document = new Document();

            document.LoadFromFile(filePath);

            foreach (Section section in document.Sections)
            foreach (Paragraph paragraph in section.Paragraphs)
                text.Append(paragraph.Text);

            return text.ToString();
        });
    }
}