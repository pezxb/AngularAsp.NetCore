﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.DbModels
{
    public class Post
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string Content { get; set; }
    }
}
