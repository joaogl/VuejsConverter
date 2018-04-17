using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool.Processors
{

    interface ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        List<string> FileTypesToAffect();

        /**
         * Execute the process that the processor requires.
         */ 
        bool Execute(string fileName, ref string contents);

    }

}
