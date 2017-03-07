using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Client.Forms;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Reflection;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Support.ContentSearch.Client.Forms
{
  public class IndexingManagerWizard : Sitecore.ContentSearch.Client.Forms.IndexingManagerWizard
  {
    protected override void BuildIndexCheckbox(string name, string header, ListString selected, ListString indexMap)
    {
      Assert.ArgumentNotNull(name, "name");
      Assert.ArgumentNotNull(header, "header");
      Assert.ArgumentNotNull(selected, "selected");
      Assert.ArgumentNotNull(indexMap, "indexMap");
      ITranslate translate = GetField(this, typeof(Sitecore.ContentSearch.Client.Forms.IndexingManagerWizard), "translate") as ITranslate;
      Checkbox child = new Checkbox();
      this.Indexes.Controls.Add(child);
      this.Indexes.Controls.Add(new LiteralControl("<br />"));
      child.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      child.Header = header;
      child.Value = name;
      child.Checked = selected.Contains(name);
      indexMap.Add(child.ID);
      indexMap.Add(name);
      Literal literal = new Literal();
      this.IndexStats.Controls.Add(literal);
      literal.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      literal.Text = string.Format("<p style=\"font-weight: bold;font-size: 12px;margin-top: 10px;\">{0}</p>", name);
      Literal literal2 = new Literal();
      this.IndexStats.Controls.Add(literal2);
      literal2.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      string str = this.BuildTime(name);
      string str2 = string.Format("<p> <strong> {0}: </strong> {1}</p>", translate.Text("Rebuild Time"), str);
      literal2.Text = str2;
      Literal literal3 = new Literal();
      this.IndexStats.Controls.Add(literal3);
      literal3.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      Literal literal4 = new Literal();
      this.IndexStats.Controls.Add(literal4);
      literal4.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      Literal literal5 = new Literal();
      this.IndexStats.Controls.Add(literal5);
      literal5.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      Literal literal6 = new Literal();
      this.IndexStats.Controls.Add(literal6);
      literal6.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");      
      try
      {
        long numberOfDocuments = ContentSearchManager.GetIndex(name).Summary.NumberOfDocuments;
        
        string str3 = string.Format("<p> <strong>{0}: </strong> 0 {1}</p>", translate.Text("Approximate Throughput"), translate.Text("items per second"));
        if ((numberOfDocuments > 0L) && (IndexHealthHelper.GetIndexRebuildTime(name) > 0))
        {
          int num2 = IndexHealthHelper.GetIndexRebuildTime(name) / 0x3e8;
          if (num2 > 0)
          {
            long num3 = numberOfDocuments / ((long)num2);
            if (num3 > 0L)
            {
              str3 = string.Format("<p> <strong>{0}: </strong> {1} {2}</p>", translate.Text("Approximate Throughput"), num3.ToString(CultureInfo.InvariantCulture), translate.Text("items per second"));
            }
          }
        }
        literal3.Text = str3;
      }
      catch (NullReferenceException exception)
      {
        string cannotMsg = "<p>Stats are not available</p>";
        literal3.Text = cannotMsg;
        return;
      }
      try
      {
        ISearchIndex index = ContentSearchManager.GetIndex(name);
        long num5 = index.Summary.NumberOfDocuments;
        Literal literal7 = new Literal();
        this.IndexStats.Controls.Add(literal7);
        literal7.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
        string str4 = string.Format("<p> <strong>{0} : </strong>{1}{2}</p>", translate.Text("Document Count"), num5, (num5 > 0xf4240L) ? string.Format(" ({0}) ", translate.Text("Consider sharding this index")) : string.Empty);
        literal7.Text = str4;
        Literal literal8 = new Literal();
        this.IndexStats.Controls.Add(literal8);
        literal8.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
        string str5 = string.Format("<p> <strong>{0} :</strong> {1}</p>", translate.Text("Is Healthy"), IndexHealthHelper.IsHealthy(name) ? translate.Text("True") : translate.Text("False"));
        literal8.Text = str5;
        Literal literal9 = new Literal();
        Literal literal10 = new Literal();
        Literal literal11 = new Literal();
        Literal literal12 = new Literal();
        ISearchIndex index2 = ContentSearchManager.GetIndex(name);
        bool hasDeletions = index2.Summary.HasDeletions;
        literal4.Text = string.Format("<p><strong>{0}: </strong>{1}</p>", translate.Text("Has Deletions"), hasDeletions ? translate.Text("True") : translate.Text("False"));
        literal5.Text = string.Format("<p><strong>{0}: </strong>{1}</p>", translate.Text("Is Clean"), index2.Summary.IsClean ? translate.Text("True") : translate.Text("False"));
        literal6.Text = string.Format("<p><strong>{0} : </strong>{1}</p>",translate.Text("Out of Date"), index2.Summary.OutOfDateIndex ? translate.Text("True") : translate.Text("False"));
        literal9.Text = string.Format("<p><strong>{0} : </strong>{1}", translate.Text("Number of Terms"), IndexHealthHelper.NumberOfTerms(index.Name).ToString(CultureInfo.InvariantCulture) + "</p>");
        literal11.Text = string.Format("<p><strong>{0} : </strong>{1} (UTC)</p>", translate.Text("Last Updated"), index2.Summary.LastUpdated.ToShortDateString() + " - " + index2.Summary.LastUpdated.ToShortTimeString());
        literal10.Text = string.Format("<p><strong>{0} : </strong>{1}</p>", translate.Text("Number of Fields"), IndexHealthHelper.NumberOfFields(index.Name).ToString(CultureInfo.InvariantCulture));
        this.IndexStats.Controls.Add(literal10);
        literal10.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
        this.IndexStats.Controls.Add(literal11);
        literal11.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
        literal12.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
        this.IndexStats.Controls.Add(literal12);
        this.IndexStats.Controls.Add(literal9);
        literal9.ID = Sitecore.Web.UI.HtmlControls.Control.GetUniqueID("dk_");
      }
      catch (Exception exception2)
      {
        Log.Error(exception2.Message, exception2, this);
      }
    }

    public  object GetField(object obj, Type type, string name)
    {
      if ((obj != null) && (name.Length > 0))
      {
        FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
          return field.GetValue(obj);
        }
      }
      return null;
    }
  }
}