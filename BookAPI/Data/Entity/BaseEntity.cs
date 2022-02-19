﻿using System;

namespace BookAPI.Data.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public  DateTime CreatedAt { get; set; }
        public  DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

    }
}
