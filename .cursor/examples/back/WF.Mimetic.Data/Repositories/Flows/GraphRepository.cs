namespace WF.Mimetic.Data.Repositories.Flows;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Data.Core.Contexts;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Repositories.Flows;

public class GraphRepository : Repository<Graph>, IGraphRepository
{
    private readonly ApplicationContext _context;

    public GraphRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public Task<Graph> GetByIdWithoutParameters(Guid id)
    {
        return _context.Set<Graph>()
            .Include(pipeline => pipeline.Nodes)
            .Include(graph => graph.Relations)
            .AsSplitQuery()
            .FirstOrDefaultAsync(pipeline => pipeline.Id == id);
    }

    public async Task<Graph> GetByIdWithoutParametersOrThrow(Guid id)
    {
        Graph graph = await GetByIdWithoutParameters(id);

        if (graph is null)
        {
            throw new ValueNotFoundException($"The {nameof(Graph)} (Id: {id}) not found.");
        }

        return graph;
    }

    public override Task<Graph> GetById(Guid id)
    {
        return _context.Set<Graph>()
            .Include(graph => graph.Nodes)
            .ThenInclude(node => node.Parameters)
            .Include(graph => graph.Relations)
            .AsSplitQuery()
            .FirstOrDefaultAsync(graph => graph.Id == id);
    }
}
