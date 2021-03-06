using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO;
using umbraco.IO;

namespace umbraco.editorControls
{
	/// <summary>
	/// Summary description for folderBrowser.
	/// </summary>
	public class folderBrowser : System.Web.UI.WebControls.WebControl, interfaces.IDataEditor
	{
		private cms.businesslogic.datatype.DefaultData _data;
        public folderBrowser(umbraco.cms.businesslogic.datatype.DefaultData Data)
		{_data = Data;}

		public Control Editor {get{return this;}}

		#region IDataField Members

		
		public virtual bool TreatAsRichTextEditor 
		{
			get {return false;}
		}
		public bool ShowLabel
		{
			get
			{
				return true;
			}
		}
		

		
		
		public void Save()
		{
			
		}

		protected override void Render(HtmlTextWriter writer)
		{
			uploadfield.DataTypeUploadField uft = new uploadfield.DataTypeUploadField();
		
			cms.businesslogic.Content c = cms.businesslogic.media.Media.GetContentFromVersion(_data.Version);
            
            //store children array here because iterating over an Array property object is very inneficient.
            var children = c.Children;
			foreach (BusinessLogic.console.IconI cc in children) 
			{
				cms.businesslogic.media.Media m = new cms.businesslogic.media.Media(cc.UniqueId);
                var props = m.getProperties;
				foreach (cms.businesslogic.property.Property p in props) 
				{
					if (p.PropertyType.DataTypeDefinition.DataType.Id == uft.Id && p.Value.ToString() != "") 
					{
						// Check for thumbnail!
						string fileNameOrg = p.Value.ToString();
						string ext = fileNameOrg.Substring(fileNameOrg.LastIndexOf(".")+1, fileNameOrg.Length-fileNameOrg.LastIndexOf(".")-1);
						string fileNameThumb = fileNameOrg.Replace("."+ext, "_thumb.jpg");
						if (File.Exists(IOHelper.MapPath(IOHelper.FindFile(fileNameThumb))))  
						{
							writer.WriteLine("<a href=\"?id=" +  m.Id.ToString() + "\"><img src=\"" + IOHelper.ResolveUrl(fileNameThumb) + "\" alt=\"" + m.Text + "\" style=\"border: none\"/></a> &nbsp; ");
						}
					}
				}
			}
			base.Render (writer);
		}
		#endregion
	}
}
