# Installing DotVVM in the project

> The `main` branch contains the project **before** applying the following steps.
>
> The `step01` shows how the project looks like **after** applying the following steps.

## Upgrade to .NET Framework 4.7.2

DotVVM supports .NET Core 3.1, .NET 5 and higher, and the old .NET Framework 4.7.2 and newer.

Unfortunately, the application still targets .NET Framework 4.5.1. We have to upgrade it to .NET Framework 4.7.2´or newer first.

1. Upgrade all projects in the solution to **.NET Framework 4.7.2** or newer.

    ![Figure 02-01 Open project properties](figures/02-01.png)

    ![Figure 02-02 Switch target framework](figures/02-02.png)

    > Perform these steps for **all three projects** in the solution.

1. Make sure the solution can be built without errors.

## Add DotVVM in the project

1. Right-click on the `Altairis.VtipBaze.WebCore` package and select **Add DotVVM**.

    ![Figure 02-03 Select Add DotVVM option](figures/02-03.png)

1. Confirm the changes and wait for the process to complete. 

    ![Figure 02-04 Confirm the changes](figures/02-04.png)

    ![Figure 02-05 Wait for the process to complete](figures/02-05.png)

1. Finally, right-click the project and select **Manage NuGet Packages**. We will need to add one optional NuGet package (`DotVVM.Adapters.WebForms`) manually. This package contains several helper components and methods which will simplify the migration.

    ![Figure 02-06 Opening the NuGet Package Manager](figures/02-06.png)

1. In the package manager window, search for `DotVVM.Adapters.WebForms` package and install it in the project.

    ![Figure 02-07 Opening the NuGet Package Manager](figures/02-07.png)

1. Open the `DotvvmStartup.cs` file and add the following line in the `Configure` method:

    ```diff
     public void Configure(DotvvmConfiguration config, string applicationPath)
     {
    +    config.AddWebFormsAdapters();
    +
         ConfigureRoutes(config, applicationPath);
         ConfigureControls(config, applicationPath);
         ConfigureResources(config, applicationPath);
     }    
    ```

    > After this step, the application will still be using ASP.NET Web Forms, but you may use DotVVM pages in the project as well.

1. Make sure that `Altairis.VtipBaze.WebCore` is the startup project.

1. Run the application by pressing F5 and make sure the application did not break.

    > If you get _System.IO.FileLoadException_ (Could not load file or assembly XXX), it is probably caused by broken assembly redirects section.
    > 
    > Open the `Altairis.VtipBaze.WebCore\Web.config` file, delete the entire `<runtime>` section. Then open the _Error List_ window in Visual Studio and double-click on the warning which starts with "Found conflicts between different versions of the same dependent assembly." Visual Studio will regenerate the section with the correct assembly binding redirects.