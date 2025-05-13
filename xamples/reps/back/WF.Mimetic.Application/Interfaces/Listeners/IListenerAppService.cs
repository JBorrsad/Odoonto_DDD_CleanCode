namespace WF.Mimetic.Application.Interfaces.Listeners;

using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.FlowResults;

public interface IListenerAppService
{
    Task<FlowResultReadDto> ListenGet(IDictionary<string, string> query, params string[] segments);
    Task<FlowResultReadDto> ListenPost(string body, IDictionary<string, string> query, params string[] segments);
    Task<FlowResultReadDto> ListenPut(string body, IDictionary<string, string> query, params string[] segments);
    Task<FlowResultReadDto> ListenDelete(IDictionary<string, string> query, params string[] segments);
}
