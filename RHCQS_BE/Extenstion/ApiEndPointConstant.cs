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
            public const string LoginEndpoint = ApiEndpoint + "/Login";
        }

        public static class Role
        {
            public const string RoleEndpoint = ApiEndpoint + "/Role";
            public const string TotalRoleEndpoint = RoleEndpoint + "/total";
        }

        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/Account";

        }

        public static class Project
        {
            public const string ProjectEndpoint = ApiEndpoint + "/Project";
        }
    }
}
