# Migrate remaining pages

> The `step03` branch contains the project **before** applying the following steps.
>
> The `step04` shows how the project looks like **after** applying the following steps.

## General guide

We need to follow the same process for the remaining pages (`Login`, `NewJoke` and `HomePage`) in the application. 

For every remaining page in the `Pages` folder:

* Create a DotVVM page with the same name in the `Views` folder.

* Paste the code from the old page into [ASPX to DotVVM converter](https://www.dotvvm.com/webforms/convert)

* Apply all suggested fixes.

* Paste the code (omitting the Web Forms directive) in the corresponding DotVVM page in the `Views` directory.

* Add the necessary state properties in the viewmodel and move or copy-paste the methods from ASP.NET Web Forms code-behind files.

* Perform the manual edits the converter was unable to do.

* Add the corresponding routing rule into `DotvvmStartup.cs` file.

You can refer to the `step04` branch to see how the `Views` and `ViewModels` folders look like after all pages have been migrated.

## Important differences

This section describes some aspects of the migration you will find.

### Themes

The application defines default CSS classes for the `TextBox` and `Button` controls in `App_Themes/Default/SkinFile.skin`.

In DotVVM, this is configured in `DotvvmStartup.cs`:

```diff
+using DotVVM.Framework.Controls;
...

    public void Configure(DotvvmConfiguration config, string applicationPath)
    {
        config.AddWebFormsAdapters();
    
        ConfigureRoutes(config, applicationPath);
        ConfigureControls(config, applicationPath);
        ConfigureResources(config, applicationPath);
    
+       config.Styles.Register<Button>().AddClass("button");
+       config.Styles.Register<TextBox>().AddClass("textbox");
    }
```

### The `NewJoke` page

* The `<asp:MultiView>` control was replaced by a group of HTML elements. Their visibility is switched using a `bool` property in the viewmodel.

* The `<asp:RequiredFieldValidator>` was removed `Views/NewJoke.dothtml`. To indicate the required field, you can use the `System.ComponentModel.DataAnnotations.RequiredAttribute`:

    ```csharp
    [Required(ErrorMessage = "Empty text is not very funny")]
    public string JokeText { get; set; }
    ```

* You don't need to check `this.IsValid` property to see whether the page contains the validation errors. In DotVVM, the `Submit` method will not be invoked in case the viewmodel is not valid. 

* You can redirect to other pages by calling `Context.RedirectToRouteHybrid`. This method will see whether the page has already been migrated to DotVVM; if not, it will fall back to the Web Forms route.

* To detect whether the user is authenticated, you can use `Context.HttpContext.User.Identity.IsAuthenticated`. 

* To support submission of the form by pressing the _Enter_ key, we can surround the form fields with the `<form>` element, and apply `IsSubmitButton=true` to a button. This will render the button as `type=submit` which will make the button to be triggered automatically when pressing _Enter_.

### The `HomePage` page

* Make sure you don't use Entity Framework entities in the viewmodel. Instead, create the model class `Model/JokeListModel.cs`, same as when we did the tag list page:

    ```csharp
    public class JokeListModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<TagListModel> Tags { get; set; }
        public bool Approved { get; set; }
        public string AdminNewTag { get; set; }
    }    
    ```

* You can bind viewmodel properties to route parameters by using the `DotVVM.Framework.ViewModel.FromRoute` attribute:

    ```csharp
    [FromRoute("PageIndex")]
    public int CurrentPageIndex { get; set; } = 1;
    ```

* The list of jokes can be represented by `GridViewDataSet<JokeListModel>` class. The `GridViewDataSet` contains a list of items displayed on the page, as well as `PagingOptions` which remember the page index and page size.

    You can then load data in this object using the `LoadFromQueryable` method. Just pass `IQueryable<JokeListModel>` of all items, and the method will apply `OrderBy`, `Skip` and `Take` operators automatically.

    ```csharp
    Jokes = new GridViewDataSet<JokeListModel>()
    {
        PagingOptions =
        {
            PageSize = 6,
            PageIndex = CurrentPageIndex - 1
        }
    };
    Jokes.LoadFromQueryable(SelectJokes());
    ```

* The original page used jQuery UI library to provide the auto-complete experience on `<asp:TextBox>`. We can replace this mechanism to use the `datalist` feature of HTML `<input>` elements:

    ```html
    <dot:TextBox style-width="120px" 
                 class="textbox" 
                 Text="{value: AdminNewTag}" 
                 list="TagList"/>
    ...

    <dot:Repeater ID="TagList" 
                  WrapperTagName="datalist" 
                  DataSource="{value: TagNames}">
        <option value="{value: _this}"></option>
    </dot:Repeater>
    ```

    You can drop the following segment in `Scripts/Site/ui.js` file:

    ```diff
     $(function () {
         // Apply jQuery UI
         $("nav a").button();
         $("article").addClass("ui-widget-content ui-corner-all");
         $("article header").addClass("ui-widget-header ui-corner-top");
         $(".paging a").button();
         $(".button").button();
         $(".aspNetDisabled").addClass("ui-state-disabled");
     
         // Click confirmation
         $("*[data-confirmprompt]").click(function () {
             return window.confirm($(this).data("confirmprompt"));
         });
    -    
    -    $.get("/tags.txt", function (data) {
    -        $(".ac-tag").autocomplete({
    -            source: data.split(","),
    -            minLength: 0,
    -            delay: 0
    -        });
    -    });
     });
    ```

    The `Handlers/TagListHandler.cs` which provides the list of tags for the `/tags.txt` endpoint can be removed eventually.

### The `Login` page

* DotVVM does not have the `<asp:Login>` control - you need to implement it on your own.

* To verify the user credentials and create the authentication cookie, use the following code (for now):

    ```csharp
    public void SignIn()
    {
        if (!Membership.ValidateUser(UserName, Password))
        {
            IsError = true;
            FailureText = "Invalid user credentials!";
            return;
        }

        FormsAuthentication.SetAuthCookie(UserName, RememberMe);
        var redirectUrl = FormsAuthentication.GetRedirectUrl(UserName, RememberMe);
        Context.RedirectToLocalUrl(redirectUrl);
    }
    ```

    > After the project is switched to .NET 8, you will to change this code to use the ASP.NET Core authentication.

### The `Handlers/FeedHandler.cs` HTTP handler

The HTTP handler which generates a RSS feed can be refactored to implement `IDotvvmPresenter`. 

```csharp
public class FeedPresenter : IDotvvmPresenter
{
    private readonly VtipBazeContext dbContext;

    public FeedPresenter(VtipBazeContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task ProcessRequest(IDotvvmRequestContext context)
    {
        context.HttpContext.Response.ContentType = "application/atom+xml";
...
```

It needs to be registered as a route and as a service in `DotvvmStartup.cs`:

```diff
+using Altairis.VtipBaze.WebCore.Handlers;
...

    private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
    {
        ...
+       config.RouteTable.Add("Feed", "feed.xml", presenterType: typeof(FeedPresenter));
    }
    ...
    public void ConfigureServices(IDotvvmServiceCollection options)
    {
        options.AddDefaultTempStorages("temp");
        options.Services.AddScoped<VtipBazeContext>();
+       options.Services.AddScoped<FeedPresenter>();
    }    
```

Since the URL looks like a file, it also need to be registered in `web.config`:

```diff
    ...
    <system.webServer>
        <modules runAllManagedModulesForAllRequests="true" />
+       <handlers>
+           <add name="Owin" verb="" path="feed.xml" type="Microsoft.Owin.Host.SystemWeb.OwinHttpHandler, Microsoft.Owin.Host.SystemWeb"/>
+       </handlers>
    </system.webServer>
    ...
```

Additionally, the handler was using an extension method `context.Request.ApplicationBaseUri()` from `Altairis.WebControls` package, which depends on ASP.NET Web Forms and will need to be removed in the future.

The method can be reimplemented as follows to use just DotVVM built-in API. Ass the `Helpers.cs` class with the following code.

```csharp
using DotVVM.Framework.Hosting;
using System;

namespace Altairis.VtipBaze.WebCore
{
    public static class Helpers
    {
        public static Uri GetApplicationBaseUri(this IDotvvmRequestContext context)
        {
            var uriBuilder = new UriBuilder(context.HttpContext.Request.Url);
            uriBuilder.Path = context.HttpContext.Request.PathBase.Value;
            uriBuilder.Query = string.Empty;
            uriBuilder.Fragment = string.Empty;
            if (!uriBuilder.Path.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }

            return uriBuilder.Uri;
        }
    }
}
```

### Do not forget to register routes

After migrating each page, you should add a corresponding route table entry for DotVVM in `DotvvmStartup.cs`. The resulting `ConfigureRoutes` method should look like this:

```csharp
private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
{
    // register routes   
    config.RouteTable.Add("TagList", "tags", "Views/TagList.dothtml");
    config.RouteTable.Add("NewJoke", "new", "Views/NewJoke.dothtml");

    config.RouteTable.Add("HomePage", "{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });
    config.RouteTable.Add("SingleJoke", "joke/{JokeId}", "Views/HomePage.dothtml");
    config.RouteTable.Add("TagSearch", "tags/{TagName}/{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });
    config.RouteTable.Add("AdminHomePage", "admin/{PageIndex:int}", "Views/HomePage.dothtml", new { PageIndex = 1 });

    config.RouteTable.Add("Login", "login", "Views/Login.dothtml");

    config.RouteTable.Add("Feed", "feed.xml", presenterType: typeof(FeedPresenter));
}
```

### Troubleshooting

* If you get an error that `<dot:Content ContentPlaceHolderID='MainCPH'>` is not found in the page, please make sure there is no invisible char at the beginning of the file. When the code of migrated pages is copied from GitHub to Visual Studio, the copied code sometimes starts with an unwanted character at the beginning. The first character in each DotVVM file should be always `@`.