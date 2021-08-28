using FixedWidthParserWriter;
using System.Collections.Generic;
using System.Reflection;

namespace FixedLenghtFileReader
{
    public interface IMyClass<T> where T : class, new()
    {
        List<T> ParseFieldsFromLines(List<string> dataLines, int StructureTypeId = 0);
    }

    public class MyClass<T> : IMyClass<T> where T : class, new()
    {
        public List<T> ParseFieldsFromLines(List<string> dataLines, int StructureTypeId = 0)
        {
            throw new System.NotImplementedException();
        }
    }
    public class MyClass2<T> where T : class, new()
    {
        public List<T> ParseFieldsFromLines(List<string> dataLines, int StructureTypeId = 0) // dataLines are stripped of header
        {
            List<T> invoiceItems = new FixedWidthLinesProvider<T>().Parse(dataLines,  StructureTypeId);
            return invoiceItems;
        }
    }
}
