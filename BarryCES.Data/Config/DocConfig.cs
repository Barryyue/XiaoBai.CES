using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BarryCES.Data.Entity;

namespace BarryCES.Data.Config
{
    public class DocConfig: EntityTypeConfiguration<DocEntity>
    {
        public DocConfig()
        {
            ToTable("Doc");
            HasKey(item => item.Id);
            //Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            //Property(item => item.Title).HasColumnType("varchar").IsRequired().HasMaxLength(100);
            //Property(item => item.Content).HasColumnType("text").IsRequired();
            //Property(item => item.FailTimes).IsRequired();
        }
    }
}
