using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Realms;

namespace DoreanStore.Models
{
    public partial class ApplicationInfo : RealmObject
    {
        [PrimaryKey]
        public Guid id { get; set; } = Guid.NewGuid();
        public int Age { get; set; }
    }
}

