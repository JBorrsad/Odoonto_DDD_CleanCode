namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Nodes;

public class GateMapping : DerivedEntityMapping<Gate, Node>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Gate> builder)
    {
        builder.Property(x => x.Route).HasColumnName("Route");
        builder.Property(x => x.Method).HasConversion(typeof(string)).HasColumnName("Method");
    }
}
