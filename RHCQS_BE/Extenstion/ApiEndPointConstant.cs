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
            public const string ConstructionDetailEndpoint = ApiEndpoint + "/id";
        }
    }
}
