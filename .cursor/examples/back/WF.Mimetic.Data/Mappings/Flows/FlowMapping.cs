namespace WF.Mimetic.Data.Mappings.Flows;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Categories;
using WF.Mimetic.Domain.Models.Flows;

public class FlowMapping : EntityMapping<Flow>
{
    protected override string TableName => "WF_Mimetic_Flows_Flows";

    protected override void ConfigureMapping(EntityTypeBuilder<Flow> builder)
    {
        builder.HasDiscriminator<string>("FlowType")
            .HasValue<Pipeline>(FlowType.Pipeline.ToString())
            .HasValue<Graph>(FlowType.Graph.ToString());

        builder.Ignore(flow => flow.Type);
        builder.Navigation(flow => flow.Nodes)
        .HasField("_nodes")
        .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<Category>()
               .WithMany(c => c.Flows)
               .HasForeignKey(f => f.CategoryId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.Property(f => f.CategoryId)
            .IsRequired(false)
            .HasDefaultValue(null);
    }
}
