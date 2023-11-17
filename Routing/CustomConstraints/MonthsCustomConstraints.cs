using System.Text.RegularExpressions;

namespace Routing.CustomConstraints
{
    public class MonthsCustomConstraints : IRouteConstraint
    {
        //sales-report/2020/apr
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(routeKey))
            {
                return false;
            }

            Regex regex = new Regex("^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if (regex.IsMatch(monthValue))
            {
                return true;
            }

            return false;
        }
    }
}
