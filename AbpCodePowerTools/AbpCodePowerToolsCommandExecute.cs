using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AbpCodePowerTools.Common;
using AbpDtoGenerator;
using AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;

namespace AbpCodePowerTools;

public class AbpCodePowerToolsCommandExecute
{
    private readonly AsyncPackage package;

    public AbpCodePowerToolsCommandExecute(AsyncPackage package)
    {
        this.package = package;
    }

    public async void Run(object sender, EventArgs e)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        try
        {
            IComponentModel val = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            VisualStudioWorkspace workspace = val.GetService<VisualStudioWorkspace>();
            var val2 = await GetSelectedSolutionExplorerItemAsync();
            if (val2 != null && val2.Name != null && !val2.Name.EndsWith(".cs"))
            {
                "ABP代码生成器,仅支持C#文件！".ErrorMsg();
                return;
            }
            Document currentSelectedDocument;
            if (((val2 != null) ? val2.Document : null) != null)
            {
                currentSelectedDocument = workspace.CurrentSolution.GetDocumentByFilePath(val2.Document.FullName);
            }
            else
            {
                if (val2 == null)
                {
                    "致命异常-无法获得当前选择的解决方案：请重新打开这个解决方案，然后重试本工具。".ErrorMsg();
                    return;
                }
                string file = val2.Name;
                string projectName = val2.ContainingProject.Name;
                List<Project> list = workspace.CurrentSolution.Projects.ToList();
                List<Project> list2 = list.Where((Project o) => o.Name.StartsWith(projectName + "(net")).ToList();
                List<Document> list3 = (list.Exists((Project p) => p.Name.Contains(".Shared")) ? list.Where((Project p) => p.Name.Contains(projectName) && !p.Name.Contains(".Shared")).FirstOrDefault().Documents.Where((Document d) => ((TextDocument)d).Name == file).ToList() : ((list2.Count <= 1) ? (from d in list.Where((Project p) => p.Name == projectName).SelectMany((Project p) => p.Documents)
                                                                                                                                                                                                                                                                                                           where d.Name == file
                                                                                                                                                                                                                                                                                                           select d).ToList() : list2.FirstOrDefault().Documents.Where(d => d.Name == file).ToList()));
                if (list3.Count == 0)
                {
                    "致命异常-无法获得当前选择的解决方案。请尝试重新打开解决方案，然后再使用本工具".ErrorMsg();
                    return;
                }
                if (list3.Count > 1)
                {
                    "当前解决方案中，有多个同名的类文件，请处理之后再使用本工具，比如删除一个^_^".ErrorMsg();
                    return;
                }
                currentSelectedDocument = list3.FirstOrDefault();
            }
            SolutionInfoModel solutionInfoModel = SolutionInfoCreator.Create(currentSelectedDocument);
            string content = JsonConvert.SerializeObject(solutionInfoModel);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Path.GetDirectoryName(typeof(CommonConsts).Assembly.Location), "UI", "AbpCodePowerTools.UI.exe"),
                UseShellExecute = false
            };
            processStartInfo.Environment.Add($"{nameof(AbpCodePowerTools)}_{nameof(SolutionInfoModel)}", content);
            Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            try
            {
                ex.ToString().ErrorMsg();

            }
            catch (Exception)
            {
                "发生了一个异常。 无法将堆栈跟踪写入临时目录.".ErrorMsg();
            }
        }
    }

    private async Task<EnvDTE.ProjectItem> GetSelectedSolutionExplorerItemAsync()
    {
        var val = (EnvDTE80.DTE2)await package.GetServiceAsync(typeof(EnvDTE.DTE));
        object[] array = val.ToolWindows.SolutionExplorer.SelectedItems as object[];
        if (array.Length != 1)
        {
            return null;
        }
        object obj = array[0];
        if (obj is not EnvDTE.UIHierarchyItem val2)
            return null;
        var val3 = val.Solution.FindProjectItem(val2.Name);
        if (val3 == null)
        {
            return null;
        }
        return val3;
    }
}
