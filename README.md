# Guide for Creating Custom BlackBaud NetCommunity Plugins
BlackBaud NetCommunity is one of, if not the, most popular web based solutions for constituent relations management among higher education institutions. This guide will take you step-by-step to creating a custom BBNC plugin with Visual Studio, C#, and ASP.Net.

## Create an empty web application in Visual Studio
1. Launch Visual Studio
1. Click "New Project..."
1. Templates -> Visual C# -> Web -> ASP.NET Empty Web application
1. Currently, we have .NET 4.7 on this server, I have had success selecting any .NET framework >= 4.0. For this plugin, we select .NET 4.5
1. After selecting a .NET version, name the project something relevant to the plugin you are creating, choose a save location, and click OK

![Creating an Empty Web Application in Visual Studio](https://raw.githubusercontent.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/master/screenshots/CreateEmptyWebApplication.jpg)

## Add `BBNCExtensions` Assembly Reference

Now that we have our empty web app created, we need to add assembly references for the `BBNCExtensions.dll`. This library will be used to tie in to BBNC to create custom parts. Navigate to the `Bin` folder of the NetCommunity installation. Typically this is going to be in `\Program Files\BlackBaud\NetCommunity\Bin`. Copy the `BBNCExtensions.dll` file.

![BBNCExtensionsdll](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/BBNCExtensionsdll.jpg)

Navigate to your project folder and paste the `BBNCExtensions.dll` file. Your project's `bin` folder should now look like this:

![Bin Folder Step 1](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/bin1.jpg)

Next, we need to add an assembly reference to `BBNCExtensions.dll`. In Visual Studio, under the project name, right click on "References", then, click "Add Reference". Click "Browse" at the bottom of the dialog box to open a file browser. Navigate to your project's `bin` folder. In my case, the path is `C:\Users\{username}\Desktop\CustomBBNCPlugin\CustomBBNCPlugin\bin` You should now see the `BBNCExtensions.dll` file. Select the file and click "Add". You will be returned back to Visual Studio. In the dialog, click "OK".

![Add Assembly Reference](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/AddReference.jpg)

If you expand your "References" tab in visual studio, you should now see a reference for `BBNCExtensions`.

![BBNCExtensions Reference](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/BBNCExtensionsReference.jpg)

## Create `Display.ascx` and `Editor.ascx` `WebUserControls`

Now that we have added our `BBNCExtensions.dll` assembly reference, we are ready to create our plugin. We need to create two `WebUserControl`s named `Display.ascx` and `Editor.ascx`. Note, we can actually call these whatever we wish, BBNC conventions, however are to call these files `Display` and `Editor`, more on this later.

To create these items in Visual Studio:
1. Right click on your project name
1. Select "Add"
1. Select "New Item..."
1. On the left-hand navigation, expand "Visual C#" and click "Web"
1. Select "WebUserControl"
1. Name your file `Display.ascx`
1. Click "Add"

![Create Display.ascx](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/CreateDisplayFile.jpg)

Repeat this process to create `Editor.ascx`. Note, Visual Studio should automagically create a corresponding `*.ascx.cs` and `*.ascx.designer.cs` file for `Display.ascx` and `Editor.ascx`. Your project should now look like this:

![Display.ascx and Editor.ascx Created](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/DisplayAndEditorCreated.jpg)

## Set Up `Display.ascx.cs` and `Editor.ascx.cs`

We are now all set up and ready to start coding. The first thing we need to do is add a couple `using` directives to each `.ascx.cs` page. In both `Display.ascx.cs` and `Editor.ascx.cs`, add the following lines:

```cs
using BBNCExtensions;
using BBNCExtensions.Parts;
```

 Now, change the class that our pages inherit from. In `Display.ascx.cs`, change the class that the page inherits from, `System.WebUI.UserControl`, to `BBNCExtensions.Parts.CustomPartDisplayBase`. In `Editor.ascx.cs`, change the class that the page inherits from, `System.WebUI.UserControl`, to `BBNCExtensions.Parts.CustomPartEditorBase`.

 At this point, our files should look like this:

`Display.ascx.cs`:
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BBNCExtensions;
using BBNCExtensions.Parts;

namespace CustomBBNCPlugin
{
    public partial class Display : BBNCExtensions.Parts.CustomPartDisplayBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
```

`Editor.ascx.cs`:
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BBNCExtensions;
using BBNCExtensions.Parts;

namespace CustomBBNCPlugin
{
    public partial class Editor : BBNCExtensions.Parts.CustomPartEditorBase;
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
```

## Create a `PluginProperties` class

The next step is to create a class to hold all of our plugin's properties. The properties class is going to be used to "glue" data between `Editor.ascx.cs` and `Display.ascx.cs`. The `Editor.ascx.cs` page will be used to set up unique, custom data than can be used by `Display.ascx.cs`. You can make changes via `Editor.ascx.cs` that can be used to set up part specific settings, displaying data, making API calls, UI customizations, etc... The only limit to what this properties class can contain is your imagination. For our simple example, we will use `Editor.ascx.cs` to display a message to the user in our plugin.

1. Right click on your project name and click "Add"
1. Click "New Item..."
1. On the left-hand navigation, expand "Visual C#" and click "Code"
1. Select "Class"
1. Give your class a name, I am going to call mine PluginProperties.cs
1. Click "Add"

Now that we have our class created. Let's add a property called `message` that we can use to display a message to the user.

Your `PluginProperties` class should now look like this:

`PluginProperties.cs`:
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomBBNCPlugin
{
    public class PluginProperties
    {
        public string message = "";
    }
}
```

## Using the `PluginProperties` class

Now that we've created a class for storing our plugin's properties, let's put it to work. Firstly, we need to add a field in `Editor.ascx` to capture data from the user. Note that, in this sense, user is a BBNC administrator, content manager, designer, etc and does not refer to a user in the sense of someone viewing your page. Let's create a label and a form input:

`Editor.ascx`:
```asp
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="CustomBBNCPlugin.Editor" %>
<%@ Register TagPrefix="bbnc" Namespace="BBNCExtensions.ServerControls" Assembly="BBNCExtensions" %>

<p>
    <asp:Label ID="lbl_message" Text="Message" runat="server"></asp:Label><br />
    <asp:TextBox ID="ctrl_message" runat="server"></asp:TextBox>
</p>
```

In `Editor.ascx.cs` we need to add two method overrides, `OnLoadContent` and `OnSaveContent`. `OnLoadContent` will not return anything, `OnSaveContent` should return a `bool` to tell BBNC whether or not to actually save the data, this could be due to invalid data, a caught exception, etc.

`Editor.ascx.cs` should now look like so:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BBNCExtensions;
using BBNCExtensions.Parts;

namespace CustomBBNCPlugin
{
   public partial class Editor : BBNCExtensions.Parts.CustomPartEditorBase;
   {
       protected void Page_Load(object sender, EventArgs e)
       {

       }

       public override void OnLoadContent()
       {

       }

       public override bool OnSaveContent(bool bDialogIsClosing)
       {

       }
   }
}
```

`OnLoadContent` we can use the BBNC API to load up our class and set our form values to their current saved values

```cs
public override void OnLoadContent()
{
    // Make sure we're not updating data, e.g. editor was just opened
    if(Request.RequestType != "POST")
    {
        // Load the saved content from our part
        PluginProperties getData = base.Content.GetContent(typeof(PluginProperties)) as PluginProperties;

        // If get_data is null, this is a brand new part
        if(getData != null)
        {
            ctrl_message.Text = getData.message;
        }
    }
}
```

`OnSaveContent` we need to `new` up our `PluginProperties` class, set it's properties to the values in the form fields, and use the BBNC API to save it to the part

```cs
public override bool OnSaveContent(bool bDialogIsClosing)
{
    // Create an empty PluginProperties class to save our data
    PluginProperties saveData = new PluginProperties();

    // Set each class property to it's respective form field value
    saveData.message = ctrl_message.Text;

    // Save all properties to the BBNC part
    base.Content.SaveContent(saveData);

    // Return false if the form doesn't pass validation, an exception is caught, or you don't want to actually save the part
    return true;

}
```

## Setup `Display.ascx`

Now that we have our editor working, it's time to display our message to the user. First, let's set up `Display.ascx`. All we need to do is add an `asp:Label`, or any other valid control, that we can update in `Display.ascx.cs`.

```asp
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Display.ascx.cs" Inherits="CustomBBNCPlugin.Display" %>
<%@ Register TagPrefix="bbnc" Namespace="BBNCExtensions.ServerControls" Assembly="BBNCExtensions" %>

<p>
    <asp:Label ID="message" runat="server"></asp:Label>
</p>
```

## Setup `Display.ascx.cs`

Finally, grab the value saved to the part and display it to the user.

```cs
protected void Page_Load(object sender, EventArgs e)
{
    if (!Page.IsPostBack)
    {
        // Read in our saved values. Note, these values don't exist if a new part has never been saved.
        PluginProperties getData = base.Content.GetContent(typeof(PluginProperties)) as PluginProperties;

        // Store the value of our message for later use
        string message = getData.message;

        // Display the value of message to the user
        lbl_message.Text = message;
    }
}
```

## Deploying the Plugin

Our plugin is now complete. Let's deploy it to our BBNC installation.

1. In Visual Studio, press `Ctrl+Shift+B` to build the application
1. Navigate to the `Bin` folder of your BBNC installation (Usually found in `C:\Program Files\BlackBaud\NetCommunity\`)
1. Copy the `<PluginName>.dll` file from your project into the `Bin` folder described above. In our case, this file is named `CustomBBNCPlugin.dll`
1. Next, navigate to the `Custom` folder of your BBNC installation (Usually found in `C:\Program Files\Blackbaud\NetCommunity`)
1. Create a folder to house your `Display.ascx` and `Editor.ascx` files. The name is arbitrary, but picking a relevant folder name is important. I'm going to call mine `CustomBBNCPlugin`, matching the name of the project
1. Copy your `Display.ascx` and `Editor.ascx` files into the folder

Our BBNC Bin and Custom folders should now look like this:

`C:\Program Files\BlackBaud\NetCommunity\Bin`:

![BBNC Bin Folder Final](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/BBNCBinFolder.jpg)

`C:\Program Files\BlackBaud\NetCommunity\Custom\CustomBBNCPlugin`:

![BBNC Custom Folder Final](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/BBNCCustomFolder.jpg)

## Installing the Plugin

Now that we've deployed our plugin code, we must tell BBNC that it exists by creating a new BBNC `Custom Framework Part`. You can think of a `Custom Framework Part` as a kind of class definition. We use the custom framework part to create individual instances of that custom part for use on our BBNC site.

1. Login to BBNC with an administrator account
1. In the ribbon at the top of the page, click the dropdown arrow next to "Administration"
1. Click "Custom Parts"
1. In the top left, click "New Framework Part"
1. In the "Name" field, give your framework part a relevant name. Sticking with convention, I'm going to name mine `CustomBBNCPlugin`, matching the project name
1. In the "Display Control Source" field, specify the path to `Display.ascx`. In my case, it is `~\Custom\CustomBBNCPlugin\Display.ascx`
1. In the "Edit Control Source" field, specify the path to `Editor.ascx`. In my case, it is `~\Custom\CustomBBNCPlugin\Editor.ascx`
1. Make sure the dropdown for "Supports Personalization for" is set to \<Not Supported\>
1. Check "Require SSL"
1. Click "Save"

Before clicking save, the form should look more or less like this:

![Installing the Plugin](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/InstallingThePlugin.jpg)

Note: the tilde `~` specifies a relative path to the web root. In our case `~` represents `C:\Program Files\BlackBaud\NetCommunity\`

## Creating a New "Instance" of the Plugin

Now that we've installed the plugin, let's "instantiate" it and add it to a new page.

1. In the ribbon at the top of the page, click the dropdown arrow next to "Site Explorer"
1. Click "Pages & Templates"
1. In the top left, click "New Page"
1. Give your page a name, select a template, and give it a browser title. The page URL should be auto-populated based on the browser title, feel free to change it. I'm going to name mine "CustomPlugin Demo Page". The auto-generated URL is https://\<My Domain\>/customplugin-demo-page
1. Click "Next"

You should now be able to add a new instance of your plugin. Click one of the menu buttons in any available "pane" on the page, then click "New Part". Now we are going to create a new instance of our part and insert it onto the page.

1. For the part type, select the part type that you created in the "Name" field when installing your plugin. In my case, it is "CustomBBNCPlugin".
1. Give your part a name, I'm going to call mine "CustomBBNCPlugin Demo Part"
1. Click Next

We are now going to be taken to the "Editor.ascx" page that we created earlier. If everything has gone correctly, you should see the label "Message" above the input field that we created earlier.

![Editor Page](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/EditorAscx.jpg)

Input a message into the field and click save. If you haven't guessed that I'm going use "Hello World!", then shame on you!

Lastly, in the ribbon at the top of the page, click "View this page". You should see the "Hello World!" message displayed on the screen.

![Hello World](https://github.com/brycejech/BlackBaud-NetCommunity-Custom-Plugin-Guide/raw/master/screenshots/HelloWorld.jpg)

Congratulations on creating your first BlackBaud NetCommunity custom part!

Thank you for following along. If you have any updates to this guide, please send me a pull request.
