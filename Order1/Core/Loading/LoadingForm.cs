using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Loading
{
    interface ILoadingForm
    {
        int Step { get; set; }
        string[] Messages { get; set; }
        void MoveHendler();
        void AddMessage(string message);
        void ChangeProcessName(string processName);
    }
}
