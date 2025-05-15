namespace WF.Mimetic.Data.Repositories.Flows;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Data.Core.Contexts;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Repositories.Flows;

public class PipelineRepository : Repository<Pipeline>, IPipelineRepository
{
    private readonly ApplicationContext _context;

    public PipelineRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public override Task<Pipeline> GetById(Guid id)
    {
        return _context.Set<Pipeline>()
            .Include(pipeline => pipeline.Nodes)
            .ThenInclude(node => node.Parameters)
            .AsSplitQuery()
            .FirstOrDefaultAsync(pipeline => pipeline.Id == id);
    }

    public Task<IEnumerable<Pipeline>> GetAllWithNodesAndParameters(Func<IQueryable<Pipeline>, IQueryable<Pipeline>> query = null)
    {
        if (query is null)
        {
            return GetAll(q => q.Include(pipeline => pipeline.Nodes).ThenInclude(node => node.Parameters).AsSplitQuery());
        }
        else
        {
            return GetAll(q => query(q.Include(pipeline => pipeline.Nodes).ThenInclude(node => node.Parameters).AsSplitQuery()));
        }
    }

    public async Task<IEnumerable<Pipeline>> GetAllWithGateNode()
    {
        IEnumerable<Pipeline> pipelines = await _context.Set<Pipeline>()
            .Include(p => p.Nodes)
            .Where(p => p.Nodes.Any(n => n is Gate))
            .ToListAsync();

        return pipelines;
    }
}
