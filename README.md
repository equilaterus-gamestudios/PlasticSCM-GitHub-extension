# PlasticSCM-GitHub-extension

Integrate PlasticSCM with GitHub. Access your issues directly from Plastic!

## Installation

### Extension Parameters

* **User email**: Your email account that is associated with GitHub.
* **Branch Prefix**: By default, we suggest "*task*".
* **Project owner**: User or organization account that owns the repository.
* **Project name**: Repository name.
* **Authentication Token**: Go to [https://github.com/settings/  tokens/new](https://github.com/settings/tokens/new) and create a *Personal token* with **repo** and **write:discussion** scopes.
* **Plastic WebUI**: Customize the URL. After doing a commit on Plastic the GitHub issue will have a comment linked with the commit using the provided URL.

	```
	https://www.plasticscm.com/orgs/YOUR_ORG/repos/YOUR_REPO/diff/changeset/
	```
	
* **Timeout**: Timeout in seconds, by default 100.
* **Linux**: true or false, enables or disables **Linux support**.

### Installation

Download the latest [release](https://github.com/equilaterus-gamestudios/PlasticSCM-GitHub-extension/releases) and extract file contents inside the *client* folder of your PlasticSCM installation. Usually:

* Linux:

  ```
  /opt/plasticscm5/client/
  ```

* Windows:

  ```
  C:\Program Files\PlasticSCM5\client
  ```

### Configuration

> Note: UI may look a little bit different on newer versions of PlasticSCM but steps are the same.

1. Run PlasticSCM, go to *Preferences* / *Issue trackers*. You should see on the dropdown *GitHub Extension*.

    ![PlasticSCM GitHub issues integration](_docs/basic-configuration.png)

    Select a **Working Mode** from the dropdown and configure the extension. For more information see **Extension Parameters** at the top of this file.

2. Before doing a check-in, you can link your PlasticSCM changeset with a Github Issue (if using **WorkingMode=TaskOnChangeset**, otherwise you can do it via-branching):

   ![PlasticSCM GitHub issues link](_docs/changeset-link-task.png)


## Build

*Windows only*: If you want to Build this project locally, clone this repo and open the solution with [Visual Studio](https://visualstudio.microsoft.com/es/) as an **Administrator user**.

To see the extension on the dropdown, you'll first need to modify the file **customextensions.conf**, located in your PlasticSCM installation folder, inside **/client** directory.

```
C:\Program Files\PlasticSCM5\client\customextensions.conf
```

Add the following line:

```
GitHub Extension=GitHubExtension.dll
```

In case that your PlasticSCM installation is not located on *C:\Program Files\PlasticSCM5\client*, additionally, you need to open on your text editor the file **src/GitHubExtension.csproj** (not the *.sln*!) and replace *C:\Program Files\PlasticSCM5\client* with your installation path (it appears multiple times).

Finally, you'll need to double check that *Output Path* and  *Start External program* are configured to run *PlasticSCM*, see the following images:

![Visual Studio configuration](_docs/debug-1.png)

![Visual Studio configuration](_docs/debug-2.png)

For more information: [See PlasticSCM documentation](https://www.plasticscm.com/documentation/extensions/plastic-scm-version-control-task-and-issue-tracking-guide#WritingPlasticSCMcustomextensions).

This project is developed and maintained by [Equilaterus](https://www.equilaterussoftworks.com/).
