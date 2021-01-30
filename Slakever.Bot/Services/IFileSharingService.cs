using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlakeverBot.Services
{
    public interface IFileSharingService
    {
        void Share(string file, IEnumerable<string> emails);
    }
}
