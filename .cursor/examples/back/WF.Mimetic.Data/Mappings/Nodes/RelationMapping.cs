namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;

public class RelationMapping : EntityMapping<Relation>
{
    protected override string TableName => "WF_Mimetic_Nodes_Relations";

    protected override void ConfigureMapping(EntityTypeBuilder<Relation> builder)
    {
        builder.HasOne<Graph>().WithMany(graph => graph.Relations).HasForeignKey(relation => relation.FlowId).OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne<Node>().WithMany().HasForeignKey(node => node.OriginId).OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne<Node>().WithMany().HasForeignKey(node => node.DestinationId).OnDelete(DeleteBehavior.ClientCascade);
    }
}
