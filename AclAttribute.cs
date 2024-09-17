using CMSProject.Data.DataAccess;
using CMSProject.Service.Interfaces;
using CMSProject.Service.Services;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMSProject.Web.Attributes
{
    public class AclAttribute : ActionFilterAttribute
    {
        private readonly string _actionName;
        public AclAttribute(string Action)
        {
            _actionName = Action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            var _userService = DependencyResolver.Current.GetService<IUserService>();                        
            
            var user_roles = (_userService.GetUserByUsername(HttpContext.Current.User.Identity.Name)
                .UserRoles).Select(x => x.Id)
                           .ToList();

            var allowed_roles = GetAllowedRoles(_actionName);

            var _has_permission = user_roles.Intersect(allowed_roles).Any();

            if (_has_permission)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
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
            var _unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

            var _query_page = "[dbo].[GetPageOperationRole] @Action";

            var queryparams_page = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    DbType = DbType.String,
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Action",
                    Size = 500,
                    SqlDbType = SqlDbType.NVarChar,
                    Value = action_name
                }
            };

            return _unitOfWork.Database.SqlQuery<int>(_query_page, queryparams_page.ToArray()).ToList();
        }        
    }
}