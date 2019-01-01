using System;
using System.Linq;
using Helpers;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.Git;
using Nuke.Common.Utilities;

namespace AbcVersionTool
{
    public class GitTool
    {
        static readonly PathConstruction.AbsolutePath _rootPath = NukeBuild.RootDirectory;
        static readonly LocalRunner _gitLocalRunner = new LocalRunner(GitTasks.GitPath);
        public static string GetHash()
        {
            var result = _gitLocalRunner
                .Run("log --max-count=1 --pretty=format:%H HEAD",
                    _rootPath, logOutput: false);
            var hash = result.Select(x => x.Text).Take(1).Join(Environment.NewLine);
            return hash;
        }

        public static AbcVersionGitSubData GetAllGitData()
        {
            var hash = GetHash();
            var commitNumber = GetCommitNumberAll();
            var commitNumberCurrentBranch = GetCommitNumberCurrentBranch();
            var commitNumberCurrentBranchFirstParent = GetCommitNumberCurrentBranchFirstParent();
            var branch = GetBranch();
            var ret = new AbcVersionGitSubData(hash, commitNumber, branch, commitNumberCurrentBranch,
                commitNumberCurrentBranchFirstParent);
            return ret;
        }


        public static int GetCommitNumberAll()
        {
            var text = RunGenericCommandReturnText("rev-list --all --count");
            try
            {
                var number = int.Parse(text);
                return number;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        public static int GetCommitNumberCurrentBranch()
        {
            var text = RunGenericCommandReturnText("rev-list HEAD --count");
            try
            {
                var number = int.Parse(text);
                return number;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }


        public static string GetTimestamp()
        {
            var text = RunGenericCommandReturnText("log --max-count=1 --pretty=format:%cI HEAD");
            try
            {
                var date = DateTime.Parse(text).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                return date;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        static string RunGenericCommandReturnText(string command, int numberLinesToReturn = 1)
        {
            try
            {
                var result = _gitLocalRunner.Run(command, _rootPath, logOutput: false);
                var text = result.Select(x => x.Text).Take(numberLinesToReturn).Join(Environment.NewLine);
                return text;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }


        public static string GetBranch()
        {
            var result1 = _gitLocalRunner
                .Run("rev-parse --abbrev-ref HEAD",
                    _rootPath, logOutput: false);
            var result2 = result1.Select(x => x.Text).Take(1).Join(Environment.NewLine);
            if (result2.IndexOf("HEAD", StringComparison.OrdinalIgnoreCase) == -1) return result2;

            var result3 = _gitLocalRunner
                .Run("symbolic-ref --short -q HEAD",
                    _rootPath, logOutput: false);
            var result4 = result3.Select(x => x.Text).Take(1).Join(Environment.NewLine);
            if (result4.IndexOf("HEAD", StringComparison.OrdinalIgnoreCase) == -1) return result2;

            return string.Empty;
        }

        /// <summary>
        /// https://stackoverflow.com/a/49567820
        /// Follow only the first parent commit upon seeing a merge commit.
        /// This option can give a better overview when viewing the evolution of a particular topic branch,
        /// because merges into a topic branch tend to be only about adjusting to updated upstream from time to time,
        /// and this option allows you to ignore the individual commits brought in to your history by such a merge.
        /// Cannot be combined with --bisect.
        /// </summary>
        /// <returns></returns>
        public static int GetCommitNumberCurrentBranchFirstParent()
        {
            var text = RunGenericCommandReturnText("rev-list HEAD --count --first-parent");
            try
            {
                var number = int.Parse(text);
                return number;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }


        public static int GetCommitNumberCurrentBranchFirstParent(string sinceSha)
        {
            var text = RunGenericCommandReturnText($"rev-list {sinceSha}..HEAD --count --first-parent");
            try
            {
                var number = int.Parse(text);
                return number;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
    }
}