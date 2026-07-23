using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace AbpCodePowerTools;

internal sealed class AbpCodePowerToolsCommand
{
	public const int CommandId = 256;

	public static readonly Guid CommandSet = new Guid("aab77349-8d14-45ee-b742-3576a40524dc");

	private readonly AsyncPackage package;

	public static AbpCodePowerToolsCommand Instance { get; private set; }

	private AbpCodePowerToolsCommand(AsyncPackage package, OleMenuCommandService commandService)
	{
		this.package = package ?? throw new ArgumentNullException("package");
		commandService = commandService ?? throw new ArgumentNullException("commandService");
		CommandID command = new CommandID(CommandSet, 256);
		MenuCommand command2 = new MenuCommand(Execute, command);
		commandService.AddCommand(command2);
	}

	public static async Task InitializeAsync(AsyncPackage package)
	{
		await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
		object obj = await package.GetServiceAsync(typeof(IMenuCommandService));
		OleMenuCommandService commandService = (OleMenuCommandService)((obj is OleMenuCommandService) ? obj : null);
		Instance = new AbpCodePowerToolsCommand(package, commandService);
	}

	private void Execute(object sender, EventArgs e)
	{
		ThreadHelper.ThrowIfNotOnUIThread("Execute");
		new AbpCodePowerToolsCommandExecute(package).Run(sender, e);
	}
}
