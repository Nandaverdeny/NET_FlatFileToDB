using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using NET_FlatFileToDB.Models;

namespace NET_FlatFileToDB.DataContexts
{
    public class DBContext : DbContext
    {

        public DBContext()
        : base("FlatFileDB")
        { }

        public DbSet<FlatFile> FlatFiles { get; set; }
        public DbSet<FlatFileWithData> FlatFilesWithData { get; set; }

    }
}