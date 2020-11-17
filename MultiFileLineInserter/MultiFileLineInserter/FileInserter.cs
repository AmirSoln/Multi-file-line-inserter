using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MultiFileLineInserter
{
    [Command(Description = "Multi file content inserter app")]
    public class FileInserter
    {
        [Option(Description = "Directory of json the files", ShortName = "d")]
        [Required]
        public string SourceDir { get; set; }

        [Option(Description = "The key to add to json file", ShortName = "k")]
        [Required]
        public string Key { get; set; }

        [Option(Description = "The value of the key", ShortName = "v")]
        [Required]
        public string Value { get; set; }



        private void OnExecute()
        {
            var fileFound = false;
            if (SourceDir.Trim().Length == 0)
            {
                throw new Exception("SourceDir must contain a path");
            }

            var files = Directory.GetFiles(SourceDir);

            foreach (var file in files)
            {
                if (file.EndsWith(".json"))
                {
                    fileFound = true;
                    InsertToFile(file);
                }
            }

            if (!fileFound)
            {
                Console.WriteLine("No .json files found");
            }
        }

        private void InsertToFile(string file)
        {
            var fileContent = ReadFile(file);
            if(!fileContent.TryAdd(Key, Value))
            {
                Console.WriteLine("Error inserting values into file. Check your key");
                return;
            }
            using (var sw = new StreamWriter(file))
            {
                sw.Write(JsonSerializer.Serialize(fileContent));
            }
        }

        private Dictionary<string,object> ReadFile(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

    }
}
