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
    public partial class Editor : BBNCExtensions.Parts.CustomPartEditorBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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
    }
}