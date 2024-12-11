using Uniqloooo.Views.Enum;

namespace Uniqloooo.Extensions
{
    public static class RolesExtensions
    {
        public static string GetRole(this Roles role)
        {
            return role switch

            {
                Roles.Admin =>nameof(Roles.Admin),
                Roles.User => nameof(Roles.User),
                Roles.Moderator => nameof(Roles.Moderator),
            };

        }
    }
}
