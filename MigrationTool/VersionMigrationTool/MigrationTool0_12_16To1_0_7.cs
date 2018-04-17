using MigrationTool.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool.VersionMigrationTool
{

    /**
     * Class to Migrate a vuejs project from 0.12.16 to 1.0.7 Pre-Release.
     */
    public class MigrationTool0_12_16To1_0_7 : MigrationTool
    {

        #region Variable Definition

        /**
         * List of processors that will be ran during the migration.
         * The order is important since it is the order they will be ran.
         */
        private List<ProcessorInterface> processors = new List<ProcessorInterface>(new ProcessorInterface[] {
            new DynamicTemplateBinderReplacer(),
            new OnDirectiveReplacer(),
            new ClassAttributeReplacer(),
            new TransitionReplacer(),
            new RepeatReplacer(),
        });

        public List<string> MissingTags = new List<string>();

        public static MigrationTool0_12_16To1_0_7 Instance { get; set; }

        #endregion

        /**
         * Default constructor.
         */
        public MigrationTool0_12_16To1_0_7()
        {
            Instance = this;
        }

        /**
         * This method bootstraps the process to start migrating the vuejs.
         * It requires the directory for conversion as a parameter.
         */
        public void MigrateDirectory(string directory)
        {
            Console.WriteLine("===================== Starting =====================");
            Console.WriteLine("Starting converter 0.12.16 to 1.0.7");
            Console.WriteLine("Checking directory: " + directory);

            // First get all file types we need to search for.
            List<string> fileTypes = this.getAllFileTypesToProcess();

            Console.WriteLine("Searching directory for " + string.Join(", ", fileTypes) + " file types.");

            // Get all files in the requested directory. 
            string[] files = fileTypes
                            .SelectMany(i => Directory.GetFiles(directory, "*" + i, SearchOption.AllDirectories))
                            .ToArray();

            Console.WriteLine(files.Count() + " files found for possible processing.");

            Console.WriteLine("====================================================");

            List<string> filesProcessed = new List<string>();
            int changes = 0;

            // Go through every selected file
            foreach (string file in files)
            {
                try
                {
                    // Ge the file contents for processing
                    string contents = File.ReadAllText(file);
                    bool anyExecutionOccured = false;

                    // Run each processor and check if it needs to be ran.
                    foreach (ProcessorInterface proc in processors)
                    {
                        bool fileChanged = false;
                        bool toExecute = false;

                        // Check if the processor needs to process this file type.
                        foreach (string fileType in proc.FileTypesToAffect())
                        {
                            if (file.EndsWith(fileType))
                            {
                                toExecute = true;
                                break;
                            }
                        }

                        // If so, execute it!!
                        if (toExecute)
                        {
                            fileChanged = proc.Execute(file, ref contents);
                            anyExecutionOccured = anyExecutionOccured || fileChanged;
                            if (fileChanged)
                            {
                                Console.WriteLine("Executed " + proc.GetType().Name.ToString() + " in file " + file);
                                changes++;
                            }
                        }
                    }

                    if (anyExecutionOccured)
                    {
                        filesProcessed.Add(file);

                        // The changes to this file have been made, now save it.
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.WriteAllText(file, contents);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("====================== Report ======================");

            Console.WriteLine("Out of the " + files.Count() + ", " + filesProcessed.Count() + " have been processed.");
            Console.WriteLine(changes + " were made to the selected files.");
            Console.WriteLine("Missing types: " + string.Join(", ", MissingTags));

            Console.WriteLine("====================================================");
        }

        #region Internal Helpers

        /**
         * This method should go through every registred processor and get all the file types 
         * that each processor needs. In the end a list of all file types required 
         * will be obtained with distinct to run in the file checker.
         */
        public List<string> getAllFileTypesToProcess()
        {
            List<string> toReturn = new List<string>();

            foreach (ProcessorInterface proc in processors)
            {
                toReturn.AddRange(proc.FileTypesToAffect());
            }

            return toReturn.Distinct<string>().ToList();
        }

        #endregion

    }

}
