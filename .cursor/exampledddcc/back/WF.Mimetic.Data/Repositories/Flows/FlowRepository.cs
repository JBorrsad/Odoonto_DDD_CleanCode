namespace WF.Mimetic.Data.Repositories.Flows;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Data.Core.Contexts;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Repositories.Flows;

public class FlowRepository : Repository<Flow>, IFlowRepository
{
    private readonly ApplicationContext _context;

    public FlowRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public Task<Flow> GetByIdWithoutParameters(Guid id)
    {
        return _context.Set<Flow>()
            .Include(pipeline => pipeline.Nodes)
            .AsSplitQuery()
            .FirstOrDefaultAsync(pipeline => pipeline.Id == id);
    }

    public async Task<Flow> GetByIdWithoutParametersOrThrow(Guid id)
    {
        Flow flow = await GetByIdWithoutParameters(id);

        if (flow is null)
        {
            throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {id}) not found.");
        }

        return flow;
    }

    public override Task<Flow> GetById(Guid id)
    {
        return _context.Set<Flow>()
            .Include(pipeline => pipeline.Nodes)
            .ThenInclude(node => node.Parameters)
            .AsSplitQuery()
            .FirstOrDefaultAsync(pipeline => pipeline.Id == id);
    }
}
