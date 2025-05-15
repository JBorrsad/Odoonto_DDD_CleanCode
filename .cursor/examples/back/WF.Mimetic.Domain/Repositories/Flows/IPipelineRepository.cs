namespace WF.Mimetic.Domain.Repositories.Flows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Models.Flows;

public interface IPipelineRepository : IRepository<Pipeline>
{
    Task<IEnumerable<Pipeline>> GetAllWithNodesAndParameters(Func<IQueryable<Pipeline>, IQueryable<Pipeline>> query = null);
    Task<IEnumerable<Pipeline>> GetAllWithGateNode();
}
