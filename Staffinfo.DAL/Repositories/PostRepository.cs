using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Repository for Posts
    /// </summary>
    public class PostRepository: IRepository<Post>
    {
        private readonly StaffContext _staffContext;

        public PostRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<Post> Select()
        {
            return _staffContext.Posts;
        }

        public Post Select(int id)
        {
            return _staffContext.Posts.Find(id);
        }

        public IEnumerable<Post> Find(Func<Post, bool> predicate)
        {
            return _staffContext.Posts.Where(predicate).ToList();
        }

        public void Create(Post item)
        {
            _staffContext.Posts.Add(item);
        }

        public void Update(Post item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Post post = _staffContext.Posts.Find(id);

            if (post != null)
                _staffContext.Posts.Remove(post);
        }
    }
}