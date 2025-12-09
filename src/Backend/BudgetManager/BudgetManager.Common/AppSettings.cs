namespace BudgetManager.Common
{
    public class AppSettings
    {
        public static readonly string AppEnv = DotNetEnv.Env.GetString("APP_ENV");
        public static readonly string AppHost = DotNetEnv.Env.GetString("HOST");
        public static readonly int AppPort = DotNetEnv.Env.GetInt("PORT");
        public static readonly string DbConnectionString = DotNetEnv.Env.GetString("DB_CONNECTION");
        public static readonly string JWT_Secret = DotNetEnv.Env.GetString("JWT_SECRET");
        public static readonly string JWT_ValidAudience = DotNetEnv.Env.GetString("JWT_VALID_AUDIENCE");
        public static readonly string JWT_ValidIssuer = DotNetEnv.Env.GetString("JWT_VALID_ISSUER");
        public static readonly string DefaultAdminEmail = DotNetEnv.Env.GetString("DEFAULT_ADMIN_EMAIL");
        public static readonly string DefaultAdminPassword = DotNetEnv.Env.GetString("DEFAULT_ADMIN_PASSWORD");
    }
}
