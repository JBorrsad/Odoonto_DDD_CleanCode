namespace WF.Mimetic.Data.Repositories.Nodes;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Data.Core.Contexts;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Repositories.Nodes;

public class NodeRepository : Repository<Node>, INodeRepository
{
    private readonly ApplicationContext _context;

    public NodeRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tnode>> GetAll<Tnode>(Func<IQueryable<Tnode>, IQueryable<Tnode>> query = null) where Tnode : Node
    {
        IQueryable<Tnode> nodes;

        if (query is not null)
        {
            nodes = query(_context.Set<Tnode>());
        }
        else
        {
            nodes = _context.Set<Tnode>();
        }

        return await nodes.ToArrayAsync();
    }

    public override Task<Node> GetById(Guid id)
    {
        return _context.Set<Node>()
            .Include(node => node.Relations)
            .Include(node => node.Parameters)
            .AsSplitQuery()
            .FirstOrDefaultAsync(node => node.Id == id);
    }

    public async Task<Tnode> GetById<Tnode>(Guid id) where Tnode : Node
    {
        Node node = await GetById(id);

        if (node is not Tnode)
        {
            return null;
        }

        return (Tnode)node;
    }

    public async Task<Tnode> GetByIdOrThrow<Tnode>(Guid id) where Tnode : Node
    {
        Tnode node = await GetById<Tnode>(id);

        if (node is null)
        {
            throw new InvalidValueException($"The {typeof(Tnode).Name} (Id: {id}) not found.");
        }

        return node;
    }

    public async Task<bool> IsUniqueGateRoute(string id, string route)
    {
        try
        {
           var gates = await _context.Set<Gate>().ToArrayAsync();
            if (id.Equals("new"))
            {
                return gates.Any(x => x.Route == route);
            }
            else
            {
                return !gates.Any(x => x.Route == route && x.Id.ToString() != id);
            }
            
        }
        catch (Exception)
        {
            throw new Exception("Database error. (repo)");
        }

    }
}
