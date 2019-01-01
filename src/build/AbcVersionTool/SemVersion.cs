using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AbcVersionTool
{
    public sealed class SemVersion : IComparable<SemVersion>, IComparable
    {
        static readonly Regex _re =
            new Regex(@"^(?<major>\d+)" +
                      @"(\.(?<minor>\d+))?" +
                      @"(\.(?<patch>\d+))?" +
                      @"(\-(?<pre>[0-9A-Za-z\-\.]+))?" +
                      @"(\+(?<build>[0-9A-Za-z\-\.]+))?$",
                RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.ExplicitCapture);


        public SemVersion(int major, int minor = 0, int patch = 0, string preRelease = "", string build = "")
        {
            Major = major;
            Minor = minor;
            Patch = patch;

            PreRelease = preRelease ?? "";
            Build = build ?? "";
        }

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string PreRelease { get; }
        public string Build { get; }
        public int CompareTo(object obj) => CompareTo((SemVersion) obj);
        public int CompareTo(SemVersion other)
        {
            if (ReferenceEquals(other, null))
                return 1;

            var r = CompareByPrecedence(other);
            if (r != 0)
                return r;

            r = CompareComponent(Build, other.Build);
            return r;
        }

        public static SemVersion Parse(string version, bool strict = false)
        {
            var match = _re.Match(version);
            if (!match.Success)
                throw new ArgumentException("Invalid version.", "version");

            var major = int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture);

            var minorMatch = match.Groups["minor"];
            var minor = 0;
            if (minorMatch.Success)
                minor = int.Parse(minorMatch.Value, CultureInfo.InvariantCulture);
            else if (strict)
                throw new InvalidOperationException("Invalid version (no minor version given in strict mode)");

            var patchMatch = match.Groups["patch"];
            var patch = 0;
            if (patchMatch.Success)
                patch = int.Parse(patchMatch.Value, CultureInfo.InvariantCulture);
            else if (strict)
                throw new InvalidOperationException("Invalid version (no patch version given in strict mode)");

            var prerelease = match.Groups["pre"].Value;
            var build = match.Groups["build"].Value;

            return new SemVersion(major, minor, patch, prerelease, build);
        }
        public override string ToString()
        {
            var version = "" + Major + "." + Minor + "." + Patch;
            if (!string.IsNullOrEmpty(PreRelease))
                version += "-" + PreRelease;
            if (!string.IsNullOrEmpty(Build))
                version += "+" + Build;
            return version;
        }
        public int CompareByPrecedence(SemVersion other)
        {
            if (ReferenceEquals(other, null))
                return 1;

            var r = Major.CompareTo(other.Major);
            if (r != 0) return r;

            r = Minor.CompareTo(other.Minor);
            if (r != 0) return r;

            r = Patch.CompareTo(other.Patch);
            if (r != 0) return r;

            r = CompareComponent(PreRelease, other.PreRelease, true);
            return r;
        }
        static int CompareComponent(string a1, string a2, bool lower = false)
        {
            var a1Empty = string.IsNullOrEmpty(a1);
            var a2Empty = string.IsNullOrEmpty(a2);
            if (a1Empty && a2Empty)
                return 0;

            if (a1Empty)
                return lower ? 1 : -1;
            if (a2Empty)
                return lower ? -1 : 1;

            var a1Comps = a1.Split('.');
            var a2Comps = a2.Split('.');

            var minLen = Math.Min(a1Comps.Length, a2Comps.Length);
            for (var i = 0; i < minLen; i++)
            {
                var ac = a1Comps[i];
                var bc = a2Comps[i];
                int a1num, a2num;
                var isanum = int.TryParse(ac, out a1num);
                var isbnum = int.TryParse(bc, out a2num);
                int s;
                if (isanum && isbnum)
                {
                    s = a1num.CompareTo(a2num);
                    if (s != 0) return a1num.CompareTo(a2num);
                }
                else
                {
                    if (isanum)
                        return -1;
                    if (isbnum)
                        return 1;
                    s = string.CompareOrdinal(ac, bc);
                    if (s != 0)
                        return s;
                }
            }

            return a1Comps.Length.CompareTo(a2Comps.Length);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var other = (SemVersion) obj;

            return Major == other.Major &&
                   Minor == other.Minor &&
                   Patch == other.Patch &&
                   string.Equals(PreRelease, other.PreRelease, StringComparison.Ordinal) &&
                   string.Equals(Build, other.Build, StringComparison.Ordinal);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var result = Major.GetHashCode();
                result = result * 31 + Minor.GetHashCode();
                result = result * 31 + Patch.GetHashCode();
                result = result * 31 + PreRelease.GetHashCode();
                result = result * 31 + Build.GetHashCode();
                return result;
            }
        }
    }
}