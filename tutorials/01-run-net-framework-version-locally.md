# Running the original .NET Framework application locally

1. Checkout the `main` branch of the repository.

1. Open the solution (`src\VtipBaze.sln`) in Visual Studio 2022.

1. Open the `App.config` file located in the `Altairis.VtipBaze.Import` project.

1. Update the connection string to point to the database. If the database with the specified name does not exist, it will be created automatically.

	> Caution: It needs to be changed on 3 places - lines 7, 8 and 16 (without the specification of the database).

1. Set the `Altairis.VtipBaze.Import` project as the startup project.

1. Run the project. It will initialize the database.

	> If the tool ends without printing out anything, it means that the database was created successfully.
	>
	> If Visual Studio stops on an error, the connection string is probably incorrect.

1. Open the `Web.config` file located in the `Altairis.VtipBaze.WebCore` project.

1. Update the connection string to point to the database. If the database with the specified name does not exist, it will be created automatically.

	> Caution: It needs to be changed on 3 places - lines 7, 8 and 70 (without the specification of the database).

1. Make sure the folder `C:\temp\mailpickup` exists. When the application needs to send an e-mail, it generates a file in this folder instead.

1. Set the `Altairis.VtipBaze.WebCore` project as the startup project.

1. Run the project.

1. To sign in into the application, locate the **π** symbol in the footer of the page, and use `admin` and `admin123` as user credentials.