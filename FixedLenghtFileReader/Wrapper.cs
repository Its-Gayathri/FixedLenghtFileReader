using FixedWidthParserWriter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FixedLenghtFileReader
{
    public class Wrapper<T> where T : class, new()
    {
        private readonly FixedLenghtFileReader.IMyClass<T> myClass;

        public Wrapper(IMyClass<T> myClass)
        {
            this.myClass = myClass;
        }
        public void m1() 
        {
            int startingLines = 2;
            int endingLine = 2;

            Byte[] abcBytes = File.ReadAllBytes("C:\\SqlScript\\abc.DAT");            
            List<string> abcList = new List<string>();
            var abcFile = new StreamReader(new MemoryStream(abcBytes));
            string abcLine;
            while ((abcLine = abcFile.ReadLine()) != null)
            {
                abcList.Add(abcLine);
            }           
           //List<InvoiceModel> invoiceModels = Parse(model, 2, 2, (int)ConfigType.Alpha);

            List<InvoiceModel> invoiceModels = MyMethod((int)ConfigType.Alpha,abcList,startingLines,endingLine);
            foreach (var invoice in invoiceModels)
            {
                Console.WriteLine(invoice.Number + "-" + invoice.Id + "-" + invoice.Numb + "-" + invoice.Name + "-" + invoice.Type + "-" + invoice.Cdf);
            }

            Byte[] xyzBytes = File.ReadAllBytes("C:\\SqlScript\\xyz.DAT");           
            List<string> xyzList = new List<string>();
            var xyzFile = new StreamReader(new MemoryStream(xyzBytes));
            string xyzLine;
            while ((xyzLine = xyzFile.ReadLine()) != null)
            {
                xyzList.Add(xyzLine);
            }

            List<InvoiceModel> invoiceModels2 = MyMethod((int)ConfigType.Beta, xyzList, startingLines, endingLine);
            foreach (var invoice in invoiceModels2)
            {
                Console.WriteLine(invoice.Number + "-" + invoice.Id + "-" + invoice.Numb + "-" + invoice.Name + "-" + invoice.Type + "-" + invoice.Cdf);
            }
        }

        private List<InvoiceModel> MyMethod(int StructureTypeId, List<string> model, int startingLines, int endingLines)
        {
            model.RemoveRange(0, startingLines);
            model.RemoveRange(model.Count - endingLines, endingLines);
            MyClass2<InvoiceModel> mc = new MyClass2<InvoiceModel>();
            List<InvoiceModel> invoiceModels = mc.ParseFieldsFromLines(model, StructureTypeId);
            //List<InvoiceModel> invoiceModels = myClass.ParseFieldsFromLines<InvoiceModel>(model, StructureTypeId);
            return invoiceModels;
        }
        public List<T> Parse(List<string> model, int skipStartingLines = 0, int skipEndingLines = 0, int structureTypeId = 0)
        {
            string a = @"007045834718                                        TForce Worldwide Inc.                                         20210521171860662         
103926108768                                        TForce Worldwide Inc.                                         20210521171860662         
213926108768                                        00019075                                                                                                                                                                                                          KIRANI                                                                                                                                                                                                                                                                                                      0001000.00                                                             E                                                                                                              001CDF001TRANID001                                                       
800000000001
9000000000010000000001";
            model.RemoveRange(0, skipStartingLines);
            model.RemoveRange(model.Count - skipEndingLines, skipEndingLines);
            MyClass2<T> mc = new MyClass2<T>();
            List<T> invoiceModels = mc.ParseFieldsFromLines(model, structureTypeId);
            return invoiceModels;
        }
        private List<string> ReadFile<T>(string filePath)
        {
            string line;
            StreamReader file = new StreamReader(File.OpenRead(filePath), true);
            List<string> listOfString = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                listOfString.Add(line);
            }          
            file.Close();
            return listOfString;
        }
    }
    public enum ConfigType { Alpha, Beta }

    public class InvoiceModel 
    {       
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 1, Length = 52)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 1, Length = 43)]
        public string Number { get; set; }
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 53, Length = 209)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 44, Length = 198)]
        public string Id { get; set; }
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 263, Length = 299)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 243, Length = 302)]
        public string Name { get; set; }
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 563, Length = 70)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 546, Length = 70)]
        public string Numb { get; set; }
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 634, Length = 110)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 617, Length = 108)]
        public string Type { get; set; }
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Alpha, Start = 745, Length = 18)]
        [FixedWidthLineField(StructureTypeId = (int)ConfigType.Beta, Start = 726, Length = 18)]
        public string Cdf { get; set; }

    }
}
