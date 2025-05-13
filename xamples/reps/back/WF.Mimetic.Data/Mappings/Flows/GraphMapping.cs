namespace WF.Mimetic.Data.Mappings.Flows;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Flows;

public class GraphMapping : DerivedEntityMapping<Graph, Flow>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Graph> builder)
    {
        builder.Navigation(graph => graph.Relations).HasField("_relations").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(f => f.CategoryId)
            .IsRequired(false)
            .HasDefaultValue(null);
    }
}
