using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

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
            fileContent.Append(",\n\t").Append($"\"{Key}\":\"{Value}\"").Append("\n}");
            using (var sw = new StreamWriter(file))
            {
                sw.Write(fileContent.ToString());
            }
        }

        private StringBuilder ReadFile(string path)
        {
            //open the file
            var retval = new StringBuilder();
            using (var sr = new StreamReader(path))
            {
                // Read the stream as a string, and write the string to the console.
                retval.Append(sr.ReadToEnd());
                retval.Length -= 2;
            }

            //return the file as string
            return retval;
        }

    }
}
