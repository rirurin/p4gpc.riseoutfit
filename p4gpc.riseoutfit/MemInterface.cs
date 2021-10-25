using p4gpc.riseoutfit.Configuration;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace p4gpc.riseoutfit
{
    class MemInterface
    {
        private IReloadedHooks _hooks;

        private Utils _utils;
        private int _baseAddress;
        private IMemory _memory;
        private Outfit _outfit;
        private Config _config;

        public MemInterface(Config configuration, Utils utils)
        {
            _memory = new Memory();
            _utils = utils;
            _config = configuration;
            using var thisProcess = Process.GetCurrentProcess();
            _baseAddress = thisProcess.MainModule.BaseAddress.ToInt32();
            _utils.Log("Initialising Switcher");
            _outfit = new Outfit(_utils, _memory, _config);
        }
        public void UpdateConfig(Config configuration)
        {
            _config = configuration;
            _outfit._config = configuration;
        }
    }
}
