namespace CMSProject.Web.Attributes
{
    public class AclAttribute : ActionFilterAttribute
    {
        //if services add in constructor it start one time and for another request these are null
        private readonly string _actionName;
        public AclAttribute(string Action)
        {
            _actionName = Action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            var _userService = DependencyResolver.Current.GetService<IUserService>();   //because if services add in constructor it start one time and for another request these are null               
            
            var user_roles = (_userService.GetUserByUsername(HttpContext.Current.User.Identity.Name)
                .UserRoles).Select(x => x.Id)
                           .ToList();

            var allowed_roles = GetAllowedRoles(_actionName);
            
            var _has_permission = user_roles.Intersect(allowed_roles).Any();

            if (_has_permission)
            {
                // Called by the ASP.NET MVC framework before the action method executes.
                base.OnActionExecuting(filterContext);
            }
            else
            {
                // return access denied page - or return unauthorized 
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                        { "controller", "Security" },
                        { "action", "AccessDenied" },
                        { "pageUrl", filterContext.HttpContext.Request.RawUrl }
                   });
            }
        }

        private List<int> GetAllowedRoles(string action_name)
        {
            // operations ...
        }        
    }
}
