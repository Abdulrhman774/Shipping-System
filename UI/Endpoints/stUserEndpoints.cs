namespace UI.Endpoints
{
    public class stUserEndpoints
    {
        public const string Base = "Api/User";

        public const string GetAll = Base;

        public const string GetById = $"{Base}/{{id}}";

        public const string Search = $"{Base}/Search";

        public const string Update = $"{Base}/{{id}}";

        public const string DeleteUser = Base;

        public const string DeleteMyAccount = $"{Base}/Me";
    }
}
