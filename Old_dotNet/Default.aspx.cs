using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
      if (IsPostBack)
        return;
      
      string query = Request.QueryString["q"];
      if (!String.IsNullOrEmpty(query))
      {
        uxQuery.Text = query;
        ProcessQuery();
      }
    }

    protected void uxScurry_Click(object sender, EventArgs e)
    {
      ProcessQuery();
    }

    private void ProcessQuery()
    {
      string squrlSymbol = GetSqurlSymbol();
      StringBuilder links = new StringBuilder();
      links.Append(RenderSqurlLink("squrl", "", squrlSymbol));
      links.Append(RenderSqurlLink("mobile", "m", squrlSymbol));
      links.Append(RenderSqurlLink("cache", "c", squrlSymbol));
      links.Append(RenderSqurlLink("translated*", "t", squrlSymbol));
      links.Append("<div class=\"tiny\">(*Translate will only work if the language of the original page and that of the browser are different.)</div>");
      
      uxSqurlResult.Text = links.ToString();
    }
    
    private string RenderSqurlLink(string description, string preface, string symbol)
    {
      string renderedHtml = "";
      if (!String.IsNullOrEmpty(preface))
        preface += ".";
      
      renderedHtml = string.Format(@"
      <div class=""squrlResult"">
        <span class=""description"">{0}:</span>
        <a class=""squrlLink"" href=""http://{1}squrl.us/{2}"" title=""{0} link"">http://{1}squrl.us/{2}</a>
      </div>", 
        description, //0
        preface, //1
        symbol //2
       );
      return renderedHtml;
    }
    
    
    private string GetSqurlSymbol()
    {
      int? squrlId = null;
      new SqurlDataClassesDataContext().AddSqurl(uxQuery.Text, ref squrlId);
      string squrlSymbol = HumanBase32.Encode(squrlId.Value);
      return squrlSymbol;
    }
}
