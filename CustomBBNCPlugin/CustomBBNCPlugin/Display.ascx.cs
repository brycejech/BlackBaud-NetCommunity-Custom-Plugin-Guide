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
    }
}