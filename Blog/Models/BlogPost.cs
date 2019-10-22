﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            this.Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        [Display(Name = "Body")]
        [AllowHtml]
        public string BlogPostBody { get; set; }
        public string ImagePath { get; set; }
        public bool Published { get; set; }

        //Navigation
        public virtual ICollection<Comment> Comments { get; set; }

    }
}