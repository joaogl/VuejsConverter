using MigrationTool.Processors;
using MigrationTool.VersionMigrationTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MigrationTool
{

    public class MigrationTool
    {
        
        public MigrationTool(VersionToConvert version, string directory)
        {

            if (version == VersionToConvert.From0_12_16To1_0_7)
            {
                if (MigrationTool0_12_16To1_0_7.Instance == null)
                    new MigrationTool0_12_16To1_0_7();

                MigrationTool0_12_16To1_0_7.Instance.MigrateDirectory(directory);
            }

        }

    }

}
