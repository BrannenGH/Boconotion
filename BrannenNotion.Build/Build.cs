namespace BrannenNotion.Build
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.Git;
    using Nuke.Common.IO;
    using Nuke.Common.ProjectModel;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "build",
        GitHubActionsImage.MacOsLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(Compile) })]
    public class Build : NukeBuild
    {
        public static int Main () => Execute<Build>(x => x.Compile);

        [Solution("BrannenNotionMac.sln")]
        readonly Solution Solution;
        [GitRepository] readonly GitRepository GitRepository;

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        public Target Clean => _ => _
             .Before(Restore)
             .Executes(() =>
             {
                 RootDirectory
                     .GlobDirectories("**/bin", "**/obj")
                     .ForEach(FileSystemTasks.DeleteDirectory);
             });

        public Target Restore => _ => _
             .Executes(() =>
             {
                 DotNetTasks
                     .DotNetRestore(s => s.SetProjectFile(this.Solution));
             });


        public Target Compile => _ => _
             .DependsOn(Restore)
             .Executes(() =>
             {
                 DotNetTasks
                     .DotNetBuild(s => s.SetProjectFile(this.Solution));
             });
     }
}

