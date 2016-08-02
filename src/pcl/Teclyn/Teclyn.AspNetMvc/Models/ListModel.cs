using System;
using System.Collections.Generic;
using System.Linq;

namespace Teclyn.AspNetMvc.Models
{
    public class ListModel<TEntity> : IListModel
    {
        public IEnumerable<TEntity> Entities { get; set; }
        public string PreviousLink { get; set; }
        public string NextLink { get; set; }
        public int PreviousMin { get; set; }
        public int PreviousMax { get; set; }
        public int NextMin { get; set; }
        public int NextMax { get; set; }
        public int Total { get; set; }

        public ListModel(IQueryable<TEntity> entities, Func<int, string> getUrl, int skip, int maxItems = 10)
        {
            var entitiesPlusOne = entities.Skip(skip).Take(maxItems + 1).ToList();

            this.Entities = entitiesPlusOne.Take(maxItems).ToList();
            this.Total = entities.Count();

            if (entitiesPlusOne.Count > maxItems)
            {
                this.NextMin = Math.Max(0, skip + maxItems) + 1;
                this.NextMax = this.NextMin + maxItems - 1;
                this.NextLink = getUrl(this.NextMin - 1);
            }

            if (skip > 0)
            {
                this.PreviousMin = Math.Max(0, skip - maxItems) + 1;
                this.PreviousMax = this.PreviousMin + maxItems - 1;
                this.PreviousLink = getUrl(this.PreviousMin - 1);
            }
        }
    }
}