namespace WF.Mimetic.Domain.Repositories.Flows;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Models.Flows;

public interface IGraphRepository : IRepository<Graph>
{
    Task<Graph> GetByIdWithoutParameters(Guid id);
    Task<Graph> GetByIdWithoutParametersOrThrow(Guid id);
}
