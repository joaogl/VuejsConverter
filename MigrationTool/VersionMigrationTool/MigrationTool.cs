using MigrationTool.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool.VersionMigrationTool
{

    public interface MigrationTool
    {
        List<string> getAllFileTypesToProcess();
        void MigrateDirectory(string directory);

    }

}
