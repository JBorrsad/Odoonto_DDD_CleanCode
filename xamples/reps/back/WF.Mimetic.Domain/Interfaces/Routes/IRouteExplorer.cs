namespace WF.Mimetic.Domain.Interfaces.Routes;

using System.Threading.Tasks;
using WF.Mimetic.Domain.Models.Nodes;

public interface IRouteExplorer
{
    Task<Gate> Match(string uri, Method method);
    Task<Gate> MatchOrThrow(string uri, Method method);
    Task AddGate(Gate gate);
    Task UpdateGate(Gate gate);
    Task RemoveGate(Gate gate);

    abstract string ConvertTemplateFormat(string template);
}
