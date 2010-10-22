using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Arena.Core;

namespace Arena.Custom.CCV.Core
{
    public static class FamilyExtensions
    {
        //public static List<FamilyMember> CompleteFamily(this Family family)
        //{
        //    if (family == null)
        //        family = new Family();

        //    List<FamilyMember> familyMembers = new List<FamilyMember>();

        //    familyMembers.Add(family.Father() ?? NewFather());
        //    familyMembers.Add(family.Mother() ?? NewMother());
        //    foreach(FamilyMember fm in family.Children())
        //        familyMembers.Add(fm);

        //    if (familyMembers.Count == 2)
        //        familyMembers.Add(NewChild());

        //    return familyMembers;
        //}

        //public static FamilyMember Father(this Family family)
        //{
        //    foreach (FamilyMember fm in family.FamilyMembers)
        //        if (fm.FamilyRole.Guid == SystemLookup.FamilyRole_Adult &&
        //            fm.Gender == Arena.Enums.Gender.Male)
        //            return fm;
        //    return null;
        //}

        //public static FamilyMember Mother(this Family family)
        //{
        //    foreach (FamilyMember fm in family.FamilyMembers)
        //        if (fm.FamilyRole.Guid == SystemLookup.FamilyRole_Adult &&
        //            fm.Gender == Arena.Enums.Gender.Female)
        //            return fm;
        //    return null;
        //}

        public static FamilyMember FindByGuid(this FamilyMemberCollection familyMembers, Guid guid)
        {
            foreach (FamilyMember fm in familyMembers)
                if (fm.Guid == guid)
                    return fm;
            return null;
        }

        //public static Address PrimaryAddress(this Family family)
        //{
        //    Address primaryAddress = family.Father().PrimaryAddress;
        //    if (primaryAddress != null)
        //        return primaryAddress;

        //    primaryAddress = family.Mother().PrimaryAddress;
        //    if (primaryAddress != null)
        //        return primaryAddress;

        //    foreach(FamilyMember fm in family.Children())
        //    {
        //        primaryAddress = fm.PrimaryAddress;
        //        if (primaryAddress != null)
        //            return primaryAddress;
        //    }

        //    return null;
        //}

        //public static PersonPhone PhoneNumber(this Family family, Guid phoneType)
        //{
        //    PersonPhone personPhone = family.Father().Phones.FindByType(phoneType);
        //    if (personPhone != null)
        //        return personPhone;

        //    personPhone = family.Mother().Phones.FindByType(phoneType);
        //    if (personPhone != null)
        //        return personPhone;

        //    foreach (FamilyMember fm in family.Children())
        //    {
        //        personPhone = fm.Phones.FindByType(phoneType);
        //        if (personPhone != null)
        //            return personPhone;
        //    }

        //    return new PersonPhone();
        //}

        //private static FamilyMember NewFather()
        //{
        //    FamilyMember fm = new FamilyMember();
        //    fm.PersonGUID = Guid.NewGuid();
        //    fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
        //    fm.Gender = Arena.Enums.Gender.Male;
        //    return fm;
        //}

        //private static FamilyMember NewMother()
        //{
        //    FamilyMember fm = new FamilyMember();
        //    fm.PersonGUID = Guid.NewGuid();
        //    fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
        //    fm.Gender = Arena.Enums.Gender.Female;
        //    return fm;
        //}

        //private static FamilyMember NewChild()
        //{
        //    FamilyMember fm = new FamilyMember();
        //    fm.PersonGUID = Guid.NewGuid();
        //    fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Child);
        //    fm.Gender = Arena.Enums.Gender.Unknown;
        //    return fm;
        //}
    }
}
