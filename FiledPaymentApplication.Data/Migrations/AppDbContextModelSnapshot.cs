﻿// <auto-generated />
using System;
using FiledPaymentApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FiledPaymentApplication.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12");

            modelBuilder.Entity("FiledPaymentApplication.Model.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<string>("CardHolder")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreditCardNumber")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("FiledPaymentApplication.Model.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId")
                        .IsUnique();

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FiledPaymentApplication.Model.Transaction", b =>
                {
                    b.HasOne("FiledPaymentApplication.Model.Payment", "Payment")
                        .WithOne("Transaction")
                        .HasForeignKey("FiledPaymentApplication.Model.Transaction", "PaymentId");
                });
#pragma warning restore 612, 618
        }
    }
}
