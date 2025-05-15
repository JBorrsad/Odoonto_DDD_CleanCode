namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;

public class NodeMapping : EntityMapping<Node>
{
    protected override string TableName => "WF_Mimetic_Nodes_Nodes";

    protected override void ConfigureMapping(EntityTypeBuilder<Node> builder)
    {
        builder.HasDiscriminator<string>("NodeType")
            .HasValue<Gate>(NodeType.Gate.ToString())
            .HasValue<Response>(NodeType.Response.ToString())
            .HasValue<Request>(NodeType.Request.ToString())
            .HasValue<Scripter>(NodeType.Scripter.ToString())
            .HasValue<Printer>(NodeType.Printer.ToString())
            .HasValue<Switcher>(NodeType.Switcher.ToString())
            .HasValue<Serializer>(NodeType.Serializer.ToString())
            .HasValue<Watchdog>(NodeType.Watchdog.ToString());

        builder.Ignore(node => node.Type);

        builder.Navigation(node => node.Parameters).HasField("_parameters").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(node => node.Relations).HasField("_relations").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<Flow>().WithMany(flow => flow.Nodes).HasForeignKey(node => node.FlowId).OnDelete(DeleteBehavior.ClientCascade);
    }
}
