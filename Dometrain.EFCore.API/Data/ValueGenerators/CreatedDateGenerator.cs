using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Dometrain.EFCore.API.Data.ValueGenerators;

public class CreatedDateGenerator : ValueGenerator<DateTime>
{
    // this will run anytime we ask EF to generate a value
    public override DateTime Next(EntityEntry entry)
    {
        return DateTime.UtcNow;
    }

    public override bool GeneratesTemporaryValues => false;
}