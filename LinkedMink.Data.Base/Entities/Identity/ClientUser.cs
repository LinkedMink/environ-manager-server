using Microsoft.AspNetCore.Identity;

namespace LinkedMink.Data.Base.Entities.Identity
{
    public class ClientUser : IdentityUser<long>, IEntity
    {
    }
}
