using Microsoft.AspNetCore.Mvc.Rendering;

public static class ManageNavPages
{
    public static string Index => "Index";
    public static string Email => "Email";
    public static string ChangePassword => "ChangePassword";
    public static string PersonalData => "PersonalData";
    public static string TwoFactorAuthentication => "TwoFactorAuthentication";
    public static string Bildirimler => "_BildirimlerPartial";
    public static string MyItems => "MyItems";

    public static string? PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}
