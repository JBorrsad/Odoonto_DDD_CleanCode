namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Nodes;

public class SerializerMapping : DerivedEntityMapping<Serializer, Node>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Serializer> builder)
    {
        builder.Property(x => x.SerializationType).HasColumnName("Serialization").HasConversion<string>();
    }
}
