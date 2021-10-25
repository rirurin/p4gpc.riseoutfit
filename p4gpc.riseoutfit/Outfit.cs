using p4gpc.riseoutfit.Configuration;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace p4gpc.riseoutfit
{
    public class Outfit
    {
        private IMemory _memory;
        private int _baseAddress;
        private Utils _utils;
        public Config _config { get; set; }
        public int instantSwitch1, instantSwitch2, instantSwitchOriginal1, instantSwitchOriginal2;
        public int rise_w_address, rise_s_address;

        public Outfit(Utils utils, IMemory memory, Config configuration)
        {
            _utils = utils;
            _memory = memory;
            _config = configuration;
            using var thisProcess = Process.GetCurrentProcess();
            _baseAddress = thisProcess.MainModule.BaseAddress.ToInt32();
            using var scanner = new Scanner(thisProcess, thisProcess.MainModule);
            // scan for infinite switching address
            rise_w_address = scanner.CompiledFindPattern("57 2E 42 45 44 00 62 61 74 74").Offset + _baseAddress;
            rise_s_address = scanner.CompiledFindPattern("53 2E 42 45 44 00 62 61 74 74").Offset + _baseAddress;
            _utils.Log($"Initialising mod");

            var _outfitChange = new Thread(tick);
            _outfitChange.Start();
        }
        void tick()
        {
            var stopwatch = Stopwatch.StartNew();
            _memory.SafeRead((IntPtr)0x4DDDA28, out int outfit);
            while (true)
            {
                _memory.SafeRead((IntPtr)0x4DDDA28, out int outfitRead);
                _utils.LogDebug($"Tick at {stopwatch.ElapsedMilliseconds / 1000.0} seconds | Current outfit is {outfitRead}");
                if (outfitRead != outfit)
                {
                    _utils.Log($"Outfit changed from {outfit} to {outfitRead}");
                    outfit = outfitRead;
                    // modify addresses time
                    if (outfit == 0)
                    {
                        // switch to defaults
                        _memory.SafeWrite((IntPtr)rise_w_address, 0x6162004445422E57);
                        _memory.SafeWrite((IntPtr)rise_s_address, 0x6162004445422E53);
                    } else
                    {
                        _memory.SafeWrite((IntPtr)rise_w_address, 0x6162004445422E42 + (outfit - 2));
                        _memory.SafeWrite((IntPtr)rise_s_address, 0x6162004445422E42 + (outfit - 2));
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
