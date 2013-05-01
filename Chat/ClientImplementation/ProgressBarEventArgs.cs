using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientImplementation
{
    public class ProgressBarEventArgs : EventArgs
    {

        public int CurrentPercentage { get; set; }

        public bool IsCompleted { get; set; }

        public string CurrentAction { get; set; }

    }
}
