using System.Collections.Generic;

namespace BargainBot.Repositories
{
    public class PagedResult<TR>
    {
        public IEnumerable<TR> Items { get; set; }

        public int TotalCount { get; set; }
    }
}