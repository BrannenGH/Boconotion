using Nuke.Common.ProjectModel;

namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.IO;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "gtk-build",
        GitHubActionsImage.UbuntuLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileGtk) })] 
    public partial class Build 
    {
        protected Project TodoTaskManagerSkiaGtk => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.Skia.Gtk");

        public Target RestoreGtk => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s =>
                    {
                        return s.SetProjectFile(this.TodoTaskManagerSkiaGtk);
                    });
            });


        public Target CompileGtk => _ => _
            .DependsOn(RestoreGtk)
            .DependsOn(CompileStandard)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s
                        .SetProjectFile(this.TodoTaskManagerSkiaGtk));
            }); 
    }
}
