﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class Collection
    {
        public int CollectionID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

    }
}
