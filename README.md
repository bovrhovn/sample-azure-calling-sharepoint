# Sample to demonstrate Azure calling SharePoint

Demonstration how to leverage Azure with web app to call SharePoint API in a context of a currently signed in user.

## Technology used

The solution will use following technology:
- [Bootstrap CSS](https://getbootstrap.com/) for design
- [ASP.NET](https://asp.net) as a technology to provide UI and code to call the API's
- [Azure Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) to get information about metrics / logs and to build interactive, rich dashboards
- [PowerShell](https://docs.microsoft.com/en-us/powershell/) support for Automating operations, scripting and deploying the environment

## Minimal requirements

1. an active [Azure](https://www.azure.com) subscription - [MSDN](https://my.visualstudio.com) or trial
   or [Azure Pass](https://microsoftazurepass.com) is fine - you can also do all of the work
   in [Azure Shell](https://shell.azure.com) (all tools installed) and by
   using [Github Codespaces](https://docs.github.com/en/codespaces/developing-in-codespaces/creating-a-codespace)
2. [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed - we do recommend an editor like [Visual Studio Code](https://code.visualstudio.com) to be able to write
   scripts, YAML pipelines and connect to repos to submit changes.
3. [OPTIONAL] [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/) installed to work with Azure or Azure PowerShell
   module installed
4. [OPTIONAL] [Windows Terminal](https://learn.microsoft.com/en-us/windows/terminal/install) to be able to work with
   multiple terminal Windows with ease

If you will be working on your local machines, you will need to have:

1. [Powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows)
   installed
2. git installed - instructions step by step [here](https://docs.github.com/en/get-started/quickstart/set-up-git)
3. [.NET](https://dot.net) installed to run the application if you want to run it locally
4. an editor (besides notepad) to see and work with code, scripts and more (for
   example [Visual Studio Code](https://code.visualstudio.com) or [NeoVim](https://neovim.io/))

## Step by step deployment

To prepare the environment, you can run the following script in your PWSH terminal:

```powershell
#to go root folder where you cloned the repo f.e. C:\Work\Project
Set-Location "C:\Work\Project\Scripts"
Add-DirToSystemEnv -RestartCurrentSession
Init-Environment -Location "WestEurope"

```

It will create resource for you to be able to work with:

1. Resource group with name and tags (you can change values in [parameters](bicep/rg.parameters.json) file)
2. Log Analytics workspace with per gb sku (you can change values in [parameters](bicep/log-analytics.parameters.json)
   file)
3. Application Insights assigned to Log Analytics workspace (you can change values
   in [parameters](bicep/application-insights.parameters.json) file)
4. Azure Web App to showcase the integration and UI interfaces.

## Knowledge expected

This sample requires from you basic understanding of:

1. Azure - learning path is available [here](https://learn.microsoft.com/en-us/training/azure/)
   - [certifications](https://learn.microsoft.com/en-us/certifications/browse/?resource_type=certification&products=azure%2Csql-server%2Cwindows-server&type=fundamentals%2Crole-based%2Cspecialty&expanded=azure%2Cwindows)
   like AZ 900 can help
2. [Git](https://git-scm.com/book/en/v2) to understand how to clone, fork, branch, merge, rebase, etc.
3. [scripting](https://en.wikipedia.org/wiki/Scripting_language#Examples) -
   either [PowerShell](https://en.wikipedia.org/wiki/PowerShell)
   or [bash](https://en.wikipedia.org/wiki/Bash_(Unix_shell)) (if you will decide to go this path)

# Additional information

You can read about different techniques and options here:

1. [What-The-Hack initiative](https://aka.ms/wth)
2. [GitHub and DevOps](https://resources.github.com/devops/)
3. [Azure Samples](https://github.com/Azure-Samples)
   or [use code browser](https://docs.microsoft.com/en-us/samples/browse/?products=azure)
4. [Azure Architecture Center](https://docs.microsoft.com/en-us/azure/architecture/)
5. [Application Architecture Guide](https://docs.microsoft.com/en-us/azure/architecture/guide/)
6. [Cloud Adoption Framework](https://docs.microsoft.com/en-us/azure/cloud-adoption-framework/)
7. [Well-Architected Framework](https://docs.microsoft.com/en-us/azure/architecture/framework/)
8. [Microsoft Learn](https://docs.microsoft.com/en-us/learn/roles/solutions-architect)

# Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
