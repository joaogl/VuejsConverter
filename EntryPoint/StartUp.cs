using MigrationTool.VersionMigrationTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntryPoint
{

    class StartUp
    {

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("======================= VueJs MigrationTool =======================");
                Console.WriteLine("To use the converter run the console with two arguments, first the");
                Console.WriteLine("directory to convert, second the converter to use.");
                Console.WriteLine("");
                Console.WriteLine(" Existent Converters: ");
                Console.WriteLine("   1: From version 0.16.17 to 1.0.7 ");
                return;
            }


            var path = args[0];
            FileAttributes attr = File.GetAttributes(path);

            if (!attr.HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("======================= VueJs MigrationTool =======================");
                Console.WriteLine("An invalid projects was received has the first argument.");
                Console.WriteLine("Please on the second argument send the project main folder");
                return;
            }

            var versionInt = 0;

            int.TryParse(args[1], out versionInt);

            if (!Enum.IsDefined(typeof(VersionToConvert), versionInt))
            {
                Console.WriteLine("======================= VueJs MigrationTool =======================");
                Console.WriteLine("An invalid converter was received has the second argument.");
                Console.WriteLine("Please use one of the following:");
                Console.WriteLine("");
                Console.WriteLine(" Existent Converters: ");
                Console.WriteLine("   1: From version 0.16.17 to 1.0.7 ");
                return;
            }

            VersionToConvert type = (VersionToConvert) Enum.Parse(typeof(VersionToConvert), args[1]);

            MigrationTool.MigrationTool tool = new MigrationTool.MigrationTool(type, path);

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();

        }

    }

}
