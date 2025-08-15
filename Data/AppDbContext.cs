using Microsoft.EntityFrameworkCore;
using SimpleDataEntryForm.Models;
using System.Collections.Generic;

namespace SimpleDataEntryForm.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<DataEntry> DataEntries => Set<DataEntry>();
    }
}
