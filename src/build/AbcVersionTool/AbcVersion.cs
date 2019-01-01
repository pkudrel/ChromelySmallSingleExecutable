using System;

namespace AbcVersionTool
{
    public class AbcVersion
    {
        public AbcVersion(AbcVersionGitSubData gitData, AbcVersionSimple simple) : this(gitData)
        {

            Major = simple.Major;
            Minor = simple.Minor;
            Patch = simple.Patch;
            BuildCounter = simple.BuildCounter;
            Special = simple.Special;
            DateTime = simple.DateUtc;
            Env = simple.Env;

        }

        public AbcVersion(AbcVersionGitSubData gitData)
        {
            GitSha = gitData.GitSha;
            GitCommitsAll = gitData.GitCommitsAll;
            GitCommitsCurrentBranch = gitData.GitCommitsCurrentBranch;
            GitCommitsCurrentBranchFirstParent = gitData.GitCommitsCurrentBranchFirstParent;
            GitBranch = gitData.GitBranch;
        }


        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public int Private { get; } = 0;
        public int BuildCounter { get; }
        public string Special { get; }
        public string Env { get; set; }
        public string GitBranch { get; }
        public string GitSha { get; }
        public int GitCommitsAll { get; }
        public int GitCommitsCurrentBranch { get; }
        public int GitCommitsCurrentBranchFirstParent { get; }

        public DateTime DateTime { get; } = DateTime.UtcNow;

        public string Version => $"{Major}.{Minor}.{Patch}";
        public string MajorMinor => $"{Major}.{Minor}.{Patch}";

        public string AssemblyVersion => $"{Major}.0.0.0";
        public string FileVersion => $"{Major}.{Minor}.{Patch}.0";
        public string PackageVersion => Version;
        public string SemVersion => string.IsNullOrEmpty(Special) ? Version : $"{Version}-{Special}";
        public string SemVersionExtend => string.IsNullOrEmpty(Special) ? $"{Version}+{BuildCounter} " : $"{Version}-{Special}+{BuildCounter}";


        public string NugetSpecial =>
            string.IsNullOrEmpty(Special) ?
                Special :
                Special.Replace(".", "").Replace("-", "");

        public string NugetVersion => string.IsNullOrEmpty(NugetSpecial) ? Version : $"{Version}-{NugetSpecial}";

        public string FullBuildMetaData => $"BuildCounter.{BuildCounter}." +
                                           $"Branch.{GitBranch}." +
                                           $"DateTime.{DateTime:s}Z." +
                                           $"Env.{Env}." +
                                           $"Sha.{GitSha}." +
                                           $"GitCommitsAll.{GitCommitsAll}." +
                                           $"GitCommitsCurrentBranch.{GitCommitsCurrentBranch}." +
                                           $"GitCommitsCurrentBranchFirstParent.{GitCommitsCurrentBranchFirstParent}";

        public string ShortBuildMetaData => $"BuildCounter.{BuildCounter}." +
                                            $"Branch.{GitBranch}." +
                                            $"DateTime.{DateTime:s}Z." +
                                            $"Env.{Env}." +
                                            $"Sha.{GitSha}." +
                                            $"GitCommits.{GitCommitsCurrentBranchFirstParent}";


        public string InformationalVersion => $"{SemVersion}+{ShortBuildMetaData}";

    }
}