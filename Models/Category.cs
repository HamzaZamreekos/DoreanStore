using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Models
{
    public class Category : RealmObject
    {
        [PrimaryKey]
        public string Name { get; set; }
    }
}
