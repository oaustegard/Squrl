using System.IO;
using System.Text;
using System.Web.UI;

/// <summary>
/// Control class extension methods
/// </summary>
public static class ControlExt
{
  /// <summary>
  /// Gets the rendered Html for a web control
  /// </summary>
  /// <param name="control">Control to render.</param>
  /// <returns>The rendered html</returns>
  public static string GetRenderedHtml(this Control control)
  {
    StringBuilder sb = new StringBuilder();
    using (StringWriter sw = new StringWriter(sb))
    {
      using (HtmlTextWriter writer = new HtmlTextWriter(sw))
      {
        control.RenderControl(writer);
      }
    }
    return sb.ToString();
  }
}
