using Microsoft.AspNetCore.Identity;
using System;

namespace ContactsDetailsManager.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
