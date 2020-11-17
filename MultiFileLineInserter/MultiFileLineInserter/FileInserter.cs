using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MultiFileLineInserter
{
    [Command(Description = "Multi file content inserter app", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw)]
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

        public Dictionary<string, string> FileData { get; set; }

        private void OnExecute()
        {
            FileData = new Dictionary<string, string>();
            var files = Directory.GetFiles(SourceDir);

            foreach (var file in files)
            {
                if (file.EndsWith(".json"))
                {
                    var content = "";
                    var result = GetNewFileContent(file, ref content);
                    if (!result)
                    {
                        Console.WriteLine("There was a problem with one of the files!");
                        return;
                    }
                    FileData.Add(file, content);
                }
            }

            if (FileData.Count == 0)
            {
                Console.WriteLine("No .json files found");
                return;
            }

            foreach (var data in FileData)
            {
                File.WriteAllText(data.Key, data.Value);
                Console.WriteLine($"The key:\"{Key}\" was added sucssesfully to the File:{data.Key} with the value: \"{Value}\"");
            }

        }

        private bool GetNewFileContent(string file, ref string content)
        {
            var fileContent = ReadFile(file);
            if (fileContent.ContainsKey(Key))
            {
                Console.WriteLine("Error inserting values into file. Check your key");
                return false;
            }
            if (!fileContent.TryAdd(Key, Value))
            {
                Console.WriteLine("Error inserting values into file.");
                return false;
            }

            content = JsonConvert.SerializeObject(fileContent, Formatting.Indented);
            return true;
        }

        private Dictionary<string, object> ReadFile(string path)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path));
        }

    }
}
