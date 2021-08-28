using FixedWidthParserWriter;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Unity;

namespace FixedLenghtFileReader
{
    class Program
    {
        public static void Main(string[] args)
        {

            MyClass<InvoiceModel> myClass = new MyClass<InvoiceModel>();
            Wrapper<InvoiceModel> wa = new Wrapper<InvoiceModel>(myClass);
            wa.m1();
            List<FixedFileLineConfig> config = new List<FixedFileLineConfig> 
            {
                   new FixedFileLineConfig{ PropertyName="Name", start=1, end=13 },
                   new FixedFileLineConfig{ PropertyName="Email", start=14, end=24 },
            };
            List<DummyFile> files = ReadFile<DummyFile>("C:\\SqlScript\\dummy.txt",config);
            //foreach (var file in files)
            //{
            //    Console.WriteLine(file.Name+"-"+file.Email);
            //}
            Console.WriteLine("________________________________________");
            List<FixedFileLineConfig> config2 = new List<FixedFileLineConfig>
            {
                   new FixedFileLineConfig{ PropertyName="Id", start=1, end=10 },
                   new FixedFileLineConfig{ PropertyName="Password", start=11, end=30 },
                   new FixedFileLineConfig{ PropertyName="place", start=31, end=32 },
                   new FixedFileLineConfig{ PropertyName="date", start=33, end=46 },
            };
            List<DummyFile2> files2 = ReadFile<DummyFile2>("C:\\SqlScript\\dummy2.txt", config2);
            //foreach (var file in files2)
            //{
            //    Console.WriteLine(file.Id + "-" + file.Password + "-" + file.place+"-"+file.date);
            //}
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IMyClass<>), typeof(IMyClass<>));
        }
        private static List<T> ReadFile<T>(string filePath, List<FixedFileLineConfig> config)
        {

            List<T> files = new List<T>();
            string line;
            
                StreamReader file = new StreamReader(File.OpenRead(filePath), true);
            
                while ((line = file.ReadLine()) != null)
                {
                  files.Add(GetFileModel<T>(line, config));//return DummyFile              
                }
            
            file.Close();
            return files;
        }

        private static T GetFileModel<T>(string line, List<FixedFileLineConfig> config)
        {
            var data = new Dictionary<string, string>();
            foreach (var lineconfig in config)
            {
                int start = lineconfig.start;
                int end = lineconfig.end;
                var a = GetPropertyValueFromFile(line, start, end);
                data.Add(lineconfig.PropertyName, a);
            }
            //var serializer = new JavaScriptSerializer();
            // var model = serializer.Deserialize<T>( serializer.Serialize(data));//json serialize
            var model = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));//json serialize
            return model;
            
        }

        private static string GetPropertyValueFromFile(string line, int start, int end)
        {
            //return line.Substring(start-1, end - (start-1)).Trim();
            return line[(start - 1)..end].Trim();
        }
        
}

    public class DummyFile
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class DummyFile2
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public int place{ get; set;}
        public DateTime date { get; set; }
    }

    public class FixedFileLineConfig
    {
        public int start { get; set; }
        public int end { get; set; }
        public string PropertyName { get; set; }
    }
}
