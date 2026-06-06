using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dr24CoreNet.WebUI.Pages;

public class BasePageModel : PageModel
{
    public string Lang { get; set; } = "fa";
    public bool IsRtl => Lang == "fa";

    public virtual void HandleLang()
    {
        Lang = Request.Query["lang"].ToString();
        if (string.IsNullOrEmpty(Lang)) Lang = "fa";
    }
}
