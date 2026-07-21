using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace AbpCodePowerTools;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[Guid("84f60d27-a2da-4f62-bbbe-4000e8be2635")]
[ProvideMenuResource("Menus.ctmenu", 1)]
public sealed class AbpCodePowerToolsPackage : AsyncPackage
{
	public const string PackageGuidString = "84f60d27-a2da-4f62-bbbe-4000e8be2635";

	protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
	{
		await ((AsyncPackage)this).JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
		await AbpCodePowerToolsCommand.InitializeAsync((AsyncPackage)(object)this);
	}
}
