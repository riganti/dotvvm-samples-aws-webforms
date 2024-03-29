# Using DotVVM to modernize ASP.NET Web Forms applications and deploy them in AWS

This repo contains a sample **ASP.NET Web Forms** application and demonstrates the process of its **incremental modernization to .NET 8** using an open-source framework [DotVVM](https://www.dotvvm.com). 

The migration is done **in-place** and most of the business logic will remain unchanged. This approach should be faster than using the side-by-side approach with two applications and YARP.

![Side-by-side migration vs In-place migration](images/migration-comparison.png)

The migration also shows how to:

* Migrate Entity Framework 6 to Entity Framework Core
* Migrate ASP.NET Membership (ASP.NET Universal Providers) to ASP.NET Core Identity 

The original Web Forms application is called [VtipBaze](https://www.vtipbaze.cz) (database of jokes) and was made by [Michal Altair Valášek](https://www.altair.blog).

## Tutorials

You can follow the individual tutorials that will guide you through the entire migration procesS:

* [01 - Run the .NET Framework version](tutorials/01-run-net-framework-version-locally.md)
* [02 - Install DotVVM in the project](tutorials/02-install-dotvvm.md)
* [03 - Migrate the master page](tutorials/03-migrate-master-page.md)
* [04 - Migrate the first pages](tutorials/04-migrate-first-page.md)
* [05 - Migrate remaining pages](tutorials/05-migrate-remaining-pages.md)
* [06 - Switch database to the new .NET](tutorials/06-switch-database-to-new-dotnet.md)
* [07 - Switch web app to the new .NET](tutorials/07-switch-web-app-to-new-dotnet.md)

## Resources

1. Install the [DotVVM for Visual Studio](https://www.dotvvm.com/get-dotvvm) extension. _Make sure you have installed the latest updates of Visual Studio 2022 - the extension always supports only the latest stable version._

2. Check out the [Cheat-sheet of differences between ASPX and DotHTML syntax](https://www.dotvvm.com/webforms).

3. Checkout the individual branches of this repo to track each step in the blog post.

4. If you have any questions, join the community at [DotVVM Forum](https://forum.dotvvm.com).

---

## Other resources

* [DotVVM Forum](https://forum.dotvvm.com)
* [DotVVM Official Website](https://www.dotvvm.com)
* [DotVVM Documentation](https://www.dotvvm.com/docs)
* [DotVVM GitHub](https://github.com/riganti/dotvvm)
* [Twitter @dotvvm](https://twitter.com/dotvvm)
* [Samples](https://www.dotvvm.com/samples)
