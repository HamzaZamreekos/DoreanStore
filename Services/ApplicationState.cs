using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Services
{
    public static class ApplicationState
    {
        public static bool Downloading { get; set; } = false;
    }
}
