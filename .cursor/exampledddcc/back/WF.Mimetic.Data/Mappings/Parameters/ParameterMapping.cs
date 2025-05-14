namespace WF.Mimetic.Data.Mappings.Parameters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Parameters;

public class ParameterMapping : EntityMapping<Parameter>
{
    protected override string TableName => "WF_Mimetic_Parameters_Parameters";

    protected override void ConfigureMapping(EntityTypeBuilder<Parameter> builder)
    {
        builder.HasDiscriminator<string>("ParameterType")
            .HasValue<BooleanParameter>(ParameterType.Boolean.ToString())
            .HasValue<NumericParameter>(ParameterType.Numeric.ToString())
            .HasValue<DateParameter>(ParameterType.Date.ToString())
            .HasValue<TextParameter>(ParameterType.Text.ToString());

        builder.Property<string>("_defaultValue");
        builder.Ignore(parameter => parameter.DefaultValue);
        builder.Ignore(parameter => parameter.Type);

        builder.HasOne<Node>().WithMany(node => node.Parameters).HasForeignKey(parameter => parameter.FatherId).OnDelete(DeleteBehavior.ClientCascade);
    }
}
