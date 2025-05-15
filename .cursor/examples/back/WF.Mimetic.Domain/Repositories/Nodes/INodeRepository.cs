namespace WF.Mimetic.Domain.Repositories.Nodes;

using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Models.Nodes;

public interface INodeRepository : IRepository<Node>
{
    Task<Tnode> GetById<Tnode>(Guid id) where Tnode : Node;
    Task<Tnode> GetByIdOrThrow<Tnode>(Guid id) where Tnode : Node;
    Task<IEnumerable<Tnode>> GetAll<Tnode>(Func<IQueryable<Tnode>, IQueryable<Tnode>> query = null) where Tnode : Node;
    Task<bool> IsUniqueGateRoute(string id, string route);
}
