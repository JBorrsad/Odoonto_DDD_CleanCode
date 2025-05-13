namespace WF.Mimetic.Data.Mappings.Flows;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Flows;

public class PipelineMapping : DerivedEntityMapping<Pipeline, Flow>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Pipeline> builder)
    {
        builder.Property(f => f.CategoryId)
            .IsRequired(false)
            .HasDefaultValue(null);
    }
}
