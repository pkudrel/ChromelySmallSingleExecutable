namespace AbcVersionTool
{
    public class AbcVersionGitSubData
    {

        public string GitBranch { get; }
        public int GitCommitsCurrentBranch { get; }
        public int GitCommitsCurrentBranchFirstParent { get; }
        public string GitSha { get; }
        public int GitCommitsAll { get; }

        public AbcVersionGitSubData(
            string gitSha,
            int gitCommitsAll,
            string gitBranch,
            int gitCommitsCurrentBranch,
            int gitCommitsCurrentBranchFirstParent)
        {
            GitBranch = gitBranch;
            GitCommitsCurrentBranch = gitCommitsCurrentBranch;
            GitCommitsCurrentBranchFirstParent = gitCommitsCurrentBranchFirstParent;
            GitSha = gitSha;
            GitCommitsAll = gitCommitsAll;
        }
    }
}