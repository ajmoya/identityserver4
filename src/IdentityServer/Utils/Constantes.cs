namespace IdentityServer.Utils
{
    public static class Constantes
    {
        public static class ConstApiResources
        {
            public static ApiResourceIya Iya = new ApiResourceIya();
        }

        public static class ConstIdentityResources
        {
            public static IdentityResourcePerfil Perfil = new IdentityResourcePerfil();
        }

        public static class ConstClaimTypes
        {
            public const string Rol = "rol";
            public const string Modulos = "modulos";
        }

        public static class ConstClaimValueTypes
        {
            public const string Admin = "admin";
        }
    }

    public class ApiResourceIya
    {
        public string Nombre => "api.iya";
        public string Descripcion => "API Incidencias y Avisos";
        public ScopesIya Scope => new ScopesIya();

        public class ScopesIya
        {
            public string Read => "api.iya.read";
            public string Write => "api.iya.write";
            public string Full => "api.iya.full";
        }
    }

    public class IdentityResourcePerfil
    {
        public string Nombre => "perfil";
        public string Descripcion => "Perfiles personalizados";
    }
}
