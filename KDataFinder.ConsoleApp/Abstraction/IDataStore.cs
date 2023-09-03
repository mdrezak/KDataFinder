using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDataFinder.ConsoleApp.Abstraction
{
    internal interface IDataStore
    {
        IOperationResult SaveObjects(IEnumerable<object> objects);
    }
}
