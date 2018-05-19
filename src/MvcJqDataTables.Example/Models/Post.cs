using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcJqDataTables.Example.Models
{
    [Serializable]
    public class Post
    {
        public Post(int id, string name, string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class PostsData
    {
        public IEnumerable<Post> Post = new List<Post>
        {
            new Post(1,"Name 1","Description 1"),
            new Post(2,"Name 2","Description 2"),
            new Post(3,"Name 3","Description 3"),
            new Post(4,"Name 4","Description 4"),
            new Post(5,"Name 5","Description 5"),
            new Post(6,"Name 6","Description 6"),
            new Post(7,"Name 7","Description 7"),
            new Post(8,"Name 8","Description 8"),
            new Post(9,"Name 9","Description 9"),
            new Post(10,"Name 10","Description 10"),
            new Post(11,"Name 11","Description 11"),
            new Post(12,"Name 12","Description 12"),
            new Post(13,"Name 13","Description 13"),
            new Post(14,"Name 14","Description 14"),
            new Post(15,"Name 15","Description 15"),
            new Post(16,"Name 16","Description 16"),
            new Post(17,"Name 17","Description 17"),
            new Post(18,"Name 18","Description 18"),
            new Post(19,"Name 19","Description 19"),
            new Post(20,"Name 20","Description 20"),
        };

    }
}