using System.CodeDom;

namespace RHCQS_BE.Extenstion
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Auth
        {
            public const string RegisterEndpoint = ApiEndpoint + "/register";
            public const string LoginEndpoint = ApiEndpoint + "/login";
            public const string LogoutEndpoint = ApiEndpoint + "/logout";
        }

        public static class Role
        {
            public const string RoleEndpoint = ApiEndpoint + "/role";
            public const string TotalRoleEndpoint = RoleEndpoint + "/total";
        }

        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/account";
            public const string AccountByIdEndpoint = AccountEndpoint + "/id";
            public const string ActiveAccountEndpoint = ApiEndpoint + "/account/active-account";
            public const string TotalAccountEndpoint = ApiEndpoint + "/account/total-account";
            public const string AccountProfileEndpoint = AccountEndpoint + "/profile";
            public const string SearchAccountEndpoint = AccountEndpoint + "/name";
            public const string UpdateDeflagEndpoint = AccountEndpoint + "/updatedeflag/{id}";

        }

        public static class Project
        {
            public const string ProjectEndpoint = ApiEndpoint + "/project";
            public const string ProjectDetailEndpoint = ProjectEndpoint + "/id";
            public const string ProjectByNumberPhone = ProjectEndpoint + "/phone";
        }

        public static class Construction
        {
            public const string ConstructionEndpoint = ApiEndpoint + "/construction";
            public const string ConstructionDetailEndpoint = ConstructionEndpoint + "/id";
            public const string ConstructionRoughEndpoint = ConstructionEndpoint + "/type";
        }

        public static class Utility
        {
            public const string UtilityEndpoint = ApiEndpoint + "/utilities";
            public const string UtilityDetaidEndpoint = UtilityEndpoint + "/id";
            public const string UtilityByTypeEndpoint = UtilityEndpoint + "/type";
            public const string UtilitySectionEndpoint = UtilityEndpoint + "/section";
            public const string UtilitySectionDEndpoint = UtilitySectionEndpoint + "/id";
        }
    }
}
