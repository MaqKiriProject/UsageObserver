using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Threading;

namespace MaqKiriAppTest
{
    class CPUObserver
    {
        private PerformanceCounter cpuCounter =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

        public float cpuUsage { get; private set; }

        public CPUObserver()
        {
            RefreshCpuUsage();  // 読み捨て
            RefreshCpuUsage();
        }

        public void RefreshCpuUsage()
        {
            Thread.Sleep(100);
            cpuUsage = cpuCounter.NextValue();
        }
    }
}
