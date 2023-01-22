using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BrandNewDay.Bank.Web.API.Models
{
    public partial class BrandNewDayContext : DbContext
    {
        public BrandNewDayContext()
        {
        }

        public BrandNewDayContext(DbContextOptions<BrandNewDayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<BankAccount> BankAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        public bool DepositMoney(string iban, decimal amount, decimal fee)
        {
            string sql = "Bank.usp_ai_deposit";

            bool is_success = true;
            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var iban_parameter = command.CreateParameter();
                iban_parameter.ParameterName = "@iban_no";
                iban_parameter.Value = iban;

                var amt_parameter = command.CreateParameter();
                amt_parameter.ParameterName = "@amount";
                amt_parameter.Value = amount;

                var fee_parameter = command.CreateParameter();
                fee_parameter.ParameterName = "@fee";
                fee_parameter.Value = fee;

                command.Parameters.Add(iban_parameter);
                command.Parameters.Add(amt_parameter);
                command.Parameters.Add(fee_parameter);

                command.Connection.Open();

                var roweffect = command.ExecuteNonQuery();

                if (command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();

                is_success = roweffect > 0;
            }

            return is_success;
        }

        public bool TransferMoney(string sourceIban, string toIban, decimal amount)
        {
            string sql = "Bank.usp_ai_transfer";

            bool is_success = true;
            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var s_iban_parameter = command.CreateParameter();
                s_iban_parameter.ParameterName = "@source_iban_no";
                s_iban_parameter.Value = sourceIban;

                var to_iban_parameter = command.CreateParameter();
                to_iban_parameter.ParameterName = "@to_iban_no";
                to_iban_parameter.Value = toIban;

                var amt_parameter = command.CreateParameter();
                amt_parameter.ParameterName = "@amount";
                amt_parameter.Value = amount;


                command.Parameters.Add(s_iban_parameter);
                command.Parameters.Add(to_iban_parameter);
                command.Parameters.Add(amt_parameter);

                command.Connection.Open();

                var roweffect = command.ExecuteNonQuery();

                if (command.Connection.State == System.Data.ConnectionState.Open)
                    command.Connection.Close();

                is_success = roweffect > 0;
            }

            return is_success;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.IbanNumber);

                entity.ToTable("BankAccount", "Bank");

                entity.HasIndex(e => e.NetUserId, "IX_BankAccount_NetUserId");

                entity.Property(e => e.IbanNumber).HasMaxLength(100);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 6)");

                entity.HasOne(d => d.NetUser)
                    .WithMany(p => p.BankAccounts)
                    .HasForeignKey(d => d.NetUserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
