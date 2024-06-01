﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderItemCongigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {


            builder.OwnsOne(OI => OI.Product, P => P.WithOwner());
            builder.Property(OI=>OI.Price).HasColumnType("decimal(18,2)");

        }
    }
}
