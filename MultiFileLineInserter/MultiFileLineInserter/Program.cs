using McMaster.Extensions.CommandLineUtils;

namespace MultiFileLineInserter
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return CommandLineApplication.Execute<FileInserter>(args);
        }
    }
}
