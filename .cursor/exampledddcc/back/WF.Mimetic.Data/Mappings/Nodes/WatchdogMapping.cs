namespace WF.Mimetic.Data.Mappings.Nodes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Nodes;

internal class WatchdogMapping : DerivedEntityMapping<Watchdog, Node>
{
    protected override void ConfigureMapping(EntityTypeBuilder<Watchdog> builder)
    {
        builder.Property(x => x.StatusCode).HasColumnName("StatusCode");
        builder.Property(x => x.Content).HasColumnName("Content");
        builder.Property(x => x.MediaType).HasColumnName("MediaType");
    }
}
