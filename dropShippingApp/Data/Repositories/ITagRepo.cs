using dropShippingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dropShippingApp.Data.Repositories
{
    public interface ITagRepo
    {
        Task AddTag(Tag tag);
        Task<Tag> RemoveTag(int tagId);
        Task UpdateTag(Tag tag);
        Task<Tag> GetTagByName(string name);
        Task<Tag> GetTagById(int id);
        List<Tag> GetTags { get; }
    }
}
