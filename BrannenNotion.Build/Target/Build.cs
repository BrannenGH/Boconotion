using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.Git;
    using Nuke.Common.ProjectModel;

    public partial class Build: NukeBuild
    {
        public static int Main () => Execute<Build>(x => x.CompileGtk);
        
        [Solution("BrannenNotionMac.sln")]
        protected readonly Solution MacSolution;

        [Solution("BrannenNotion.sln")]
        protected readonly Solution WindowsSolution;

        protected Project Shared => MacSolution.GetProject("BrannenNotion.Shared");

        protected Project TodoTaskManagerShared => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.Shared");

        [GitRepository]
        protected readonly GitRepository GitRepository;

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        protected readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        public Target Clean => _ => _
            .Executes(() =>
            {
                RootDirectory
                    .GlobDirectories("*[!Build]/**/bin", "*[!Build]/**/obj")
                    .ForEach(FileSystemTasks.DeleteDirectory);
            });

        public Target CompileStandard => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s
                        .SetProjectFile(this.TodoTaskManagerShared)
                        .SetFramework("netstandard2.0"));
            });
    }
}
