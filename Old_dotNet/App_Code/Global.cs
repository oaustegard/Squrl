using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Global class common to all Squrl application objects.
/// </summary>
public class Global : HttpApplication
{
  public override void Init()
  {
    BeginRequest += new EventHandler(Global_BeginRequest);
    base.Init();
  }
  
  /// <summary>
  /// Handle BeginRequest events.
  /// </summary>
  /// <param name="sender">Sender object</param>
  /// <param name="e">Event Arguments</param>
  private void Global_BeginRequest(Object sender, EventArgs e)
  {
    String path = Request.Path;
    String lowercasePath = path.ToLower();


    //image or client resource requests - pass through
    if (lowercasePath.StartsWith("/images/") || lowercasePath.StartsWith("/client/"))
      return;

    //squrl welcome
    if (path == "/" || lowercasePath.StartsWith("/default."))
    {
      Context.RewritePath("/default.aspx");
      return;
    }

    //other redirects
    if (HandleRedirects())
      return;

    //squrl
    Context.RewritePath("/scurry.aspx?q=" + path.Substring(1));
  }


  /// <summary>
  /// Handle the generic redirects.  Edit redirects dictionary as needed
  /// </summary>
  /// <returns><c>true</c> if handled; otherwise <c>false</c>.</returns>
  private bool HandleRedirects()
  {
    bool handled = false;
    String path = Request.Path;
    String lowercasePath = path.ToLower();

    Dictionary<string, string> redirects = new Dictionary<string, string>();
    redirects.Add("/?q=", "default");
    //any other actions?

    foreach (string key in redirects.Keys)
    {
      if (lowercasePath.StartsWith(key))
      {
        Context.RewritePath(string.Format("/{0}.aspx?q={1}",
          redirects[key], //0
          path.Substring(key.Length) //1
          ));
        handled = true;
        break;
      }
    }
    return handled;
  }
}
