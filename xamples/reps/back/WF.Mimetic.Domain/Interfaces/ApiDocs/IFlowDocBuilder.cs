namespace WF.Mimetic.Domain.Interfaces.ApiDocs;

using System.Collections.Generic;
using WF.Mimetic.Domain.Models.ApiDocs;
using WF.Mimetic.Domain.Models.Flows;

public interface IFlowDocBuilder
{
    ApiDoc CreateDoc(string title, IEnumerable<Flow> flows);
}
