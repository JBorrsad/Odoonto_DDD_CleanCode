namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Nodes;

public class ScripterMapping : DerivedEntityMapping<Scripter, Node>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Scripter> builder)
    {
    }
}
