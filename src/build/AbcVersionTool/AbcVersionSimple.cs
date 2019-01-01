using System;

namespace AbcVersionTool
{
    public class AbcVersionSimple
    {
        public AbcVersionSimple(
            int major,
            int minor,
            int patch,
            string special,
            int buildCounter,
            DateTime dateUtc,
            string env)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Special = special;
            BuildCounter = buildCounter;
            DateUtc = dateUtc;
            Env = env;
        }

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string Special { get; }
        public string Env { get; }
        public int BuildCounter { get; }
        public DateTime DateUtc { get; }


    }

    
}