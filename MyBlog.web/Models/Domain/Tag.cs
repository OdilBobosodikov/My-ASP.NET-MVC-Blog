﻿namespace MyBlog.web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplatyName { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
