namespace WF.Mimetic.Data.Mappings.Categories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WF.Mimetic.Data.Core.Mappings;
using WF.Mimetic.Domain.Models.Categories;

public class CategoryMapping : EntityMapping<Category>
{
    protected override string TableName => "WF_Mimetic_Flows_Categories";
    protected override void ConfigureMapping(EntityTypeBuilder<Category> builder)
    {
       
        builder.Property(c => c.Name).IsRequired();
        builder.Navigation(category => category.Flows).HasField("_flows")
        .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
