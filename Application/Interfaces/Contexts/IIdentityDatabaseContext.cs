﻿using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IIdentityDatabaseContext
    {
        DbSet<Role> Roles { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserTokens> UserTokens { get; set; }
        DbSet<Message> Messages { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

    }
}
