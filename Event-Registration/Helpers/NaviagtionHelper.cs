using Microsoft.AspNetCore.Mvc.Rendering;

namespace Event_Registration.Helpers
{
    public static class NavigationHelper
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
        {
            var currentAction = (string)htmlHelper.ViewContext.RouteData.Values["Action"];
            var currentController = (string)htmlHelper.ViewContext.RouteData.Values["Controller"];

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            if (string.IsNullOrEmpty(actions))
                actions = currentAction;

            return controllers.Split(',').Any(c => c.Equals(currentController, StringComparison.OrdinalIgnoreCase)) &&
                   actions.Split(',').Any(a => a.Equals(currentAction, StringComparison.OrdinalIgnoreCase))
                ? cssClass
                : string.Empty;
        }
    }
}