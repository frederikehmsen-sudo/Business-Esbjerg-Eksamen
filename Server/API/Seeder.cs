using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using Service.Implementation;
using Service.Presets;
using Service.Security;

namespace API;

public class Seeder(AppDatabase db, IPasswordHasher passwordHasher)
{
    private static readonly Guid AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid AccountantId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid BusinessEsbjergRuleSetId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public void Seed()
    {
        db.CreateTable<Employee>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<HolidayRuleSet>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<CalendarDayOff>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<WorkSchedule>(tableOptions: TableOptions.CreateIfNotExists);

        SeedUsers();
        SeedHolidayRuleSet();
    }

    private void SeedUsers()
    {
        if (db.Users.Count() != 0)
            return;

        db.Insert(new User
        {
            Id = AdminId,
            Username = "Admin",
            Password = passwordHasher.HashAndSaltPassword("123"),
            Role = Role.Admin
        });
        db.Insert(new User
        {
            Id = AccountantId,
            Username = "Accountant",
            Password = passwordHasher.HashAndSaltPassword("123"),
            Role = Role.Accountant
        });
    }
    
    private void SeedHolidayRuleSet()
    {
        if (db.HolidayRuleSets.Any(r => r.Id == BusinessEsbjergRuleSetId))
            return;

        var businessEsbjerg = HolidayRuleSetPresets.FerielovenWith6thWeek();
        businessEsbjerg.Name = "Business Esbjerg";
        businessEsbjerg.EffectiveFrom = new DateOnly(2020, 9, 1);

        var ruleSet = HolidayRuleSetMapper.ToEntity(
            businessEsbjerg,
            actorId: AdminId,
            createdAt: new DateTime(2020, 9, 1, 0, 0, 0, DateTimeKind.Utc));
        ruleSet.Id = BusinessEsbjergRuleSetId;

        db.Insert(ruleSet);
    }
}