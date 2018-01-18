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
    public class PicConfig:EntityTypeConfiguration<PicEntity>
    {
        public PicConfig()
        {
            ToTable("Pic");
            HasKey(item => item.Id);
            Property(item => item.Id).HasColumnType("varchar").HasMaxLength(20);
            
        }
    }
}
