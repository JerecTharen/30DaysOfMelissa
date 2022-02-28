using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _30DaysOfMelissa
{
    public class StoreEmail
    {
        public static readonly string DbName = "3DMdb";
        private readonly SqliteDbContext _db;
        public static async Task<StoreEmail> Init()
        {
            var dbContext = new SqliteDbContext();
            if (!File.Exists($"{DbName}.db"))
            {
                await dbContext.Database.EnsureCreatedAsync();
                await dbContext.GmailEmails.AddRangeAsync(new GmailEmail[0]);
                await dbContext.SaveChangesAsync();
            }
            return new StoreEmail(dbContext);
                
        }
        public StoreEmail(SqliteDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<bool> AddEmail(GmailEmail mail)
        {
            _db.GmailEmails.Add(mail);
            return (await _db.SaveChangesAsync()) == 1;
        }
        public async Task<List<GmailEmail>> GetUnsentMail(int? limit = null)
        {
            var query = _db.GmailEmails.Where(mail => !mail.IsSentSMS);
            if (limit.HasValue)
                return await query.Take(limit.Value).ToListAsync();
            else
                return await query.ToListAsync();
        }
        public async Task MarkMsgSent(GmailEmail mail)
        {
            var gmail = await _db.GmailEmails.Where(g => g.EmailID == mail.EmailID).FirstOrDefaultAsync();
            gmail.IsSentSMS = true;
            await _db.SaveChangesAsync();
        }
    }

    public class SqliteDbContext: DbContext
    {
        public DbSet<GmailEmail> GmailEmails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlite($"FileName={StoreEmail.DbName}", sqliteOptionsAction: (option)=> 
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GmailEmail>().ToTable("GmailEmails", "test");
            modelBuilder.Entity<GmailEmail>(entity=>
            {
                entity.HasKey(k => k.EmailID);
                entity.HasIndex(i => i.EmailUid).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
