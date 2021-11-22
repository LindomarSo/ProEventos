using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ProEventos.Domain.Identity
{
    // Uma role pode pertencer a diversos usuário/  many-to-many
    public class Role : IdentityRole<int> // Usada para dar permissões 
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}