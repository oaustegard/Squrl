using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class Scurry : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    string url = GetSqurledUrl();
    this.Title = "Scurrying to " + url;
    RenderHtml(url);
  }

  private string GetSqurledUrl()
  {
    string url = "";
    string squrl = Request.QueryString["q"];
    if (!String.IsNullOrEmpty(squrl))
    {
      int squrlId = Convert.ToInt32(HumanBase32.Decode(squrl));
      new SqurlDataClassesDataContext().GetSqurl(squrlId, ref url);
    }
    return url;
  }

  private void RenderHtml(string url)
  {

    if (!String.IsNullOrEmpty(url))
    {
      string tld = Request.ServerVariables["HTTP_HOST"].Split('.')[0];
      bool redirect = true;
      string locationHref = "";
      string redirectDescription = "";

      switch (tld)
      {
        case "www":
        case "squrl": //no tld
          locationHref = string.Format("\"{0}\"", url);
          break;

        case "m": //mobile
          redirectDescription = "Skweezer based mobile version of";
          locationHref = GetPrefixedEscapedHref("http://www.skweezer.net/s.aspx?q=", url);
          break;

        case "c": //cache
          redirectDescription = "Google cached copy of";
          locationHref = GetPrefixedEscapedHref("http://www.google.com/search?hl=en&q=cache", ":" + url);
          break;

        case "t": //translate
          redirectDescription = "Google translated version of";
          locationHref = GetPrefixedEscapedHref("http://translate.google.com/translate?u=", url);
          break;

        case "i": //info
          redirect = false;
 
      }

      if (redirect)
      {
        uxSqurlResult.Text = string.Format(@"
        Scurrying to {0} {1} ...
        <scr" + @"ipt type=""text/javascript"">
        location.href={2};
        </scr" + "ipt>", redirectDescription, url, locationHref);
      }
      else
      {
        uxSqurlResult.Text = string.Format(@"
        Coming soon: Info for <a href=""{0}"">{0}</a>", url);
      }
    }
    else
    {
      uxSqurlResult.Text = "Nothing here. :-(";
    }
  }

  private string GetPrefixedEscapedHref(string prefix, string url)
  {
    return string.Format("\"{0}\" + escape(\"{1}\")", prefix, url);
  }
}
