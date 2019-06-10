using NPOI.SS.UserModel;
using System.Collections.Concurrent;
using System.IO;

namespace AtDb.Reader
{
    public class BookReference
    {
        private readonly ConcurrentDictionary<IWorkbook, string> bookReference = new ConcurrentDictionary<IWorkbook, string>();
        public void AddBookWithPath(IWorkbook workbook, string filePath)
        {
            string name = GetName(filePath);
            bookReference.TryAdd(workbook, name);
        }

        public string GetBookName(IWorkbook workbook)
        {
            string name = bookReference[workbook];
            return name;
        }

        private string GetName(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);
            return name;
        }
    }
}