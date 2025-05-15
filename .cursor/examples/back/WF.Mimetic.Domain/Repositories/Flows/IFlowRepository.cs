namespace WF.Mimetic.Domain.Repositories.Flows;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Models.Flows;

public interface IFlowRepository : IRepository<Flow>
{
    Task<Flow> GetByIdWithoutParameters(Guid id);
    Task<Flow> GetByIdWithoutParametersOrThrow(Guid id);
}
