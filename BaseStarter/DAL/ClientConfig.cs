﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BaseStarter.Models;


namespace BaseStarter.DAL
{
    
    /// <summary>
    /// Database settings for object Client
    /// </summary>
    internal class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("Client");
            entity.Property(x => x.DateOfCreation).HasDefaultValueSql("getdate()");
        }
    }
}