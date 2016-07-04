using System;
using System.ComponentModel;
using System.Data.Entity.Design.PluralizationServices;
using System.Reflection;

namespace male.services.biz
{
  public static class Strings
  {
    private static PluralizationService pluralizationService = PluralizationService.CreateService(System.Globalization.CultureInfo.CurrentUICulture);
    public static string Pluralize(this MemberInfo memberInfo)//types, propertyinfos, ect
    {
      return Pluralize(memberInfo.Name.StripEnd());
    }

    public static string Pluralize(this string name)
    {
      return pluralizationService.Pluralize(name); // remove EF type suffix, if any
    }

    public static string StripEnd(this string name)
    {
      return name.Split('_')[0];
    }
  }

  public static class Dates
  {
    public static DateTime FirstDayOfMonth(this DateTime date)
    {
      return new DateTime(date.Year, date.Month, 1);
    }

    public static DateTime LastDayOfMonth(this DateTime date)
    {
      return date.AddMonths(1).FirstDayOfMonth().AddDays(-1);
    }

    public static DateTime FirstDayOfWeekAfter(this DateTime date, DayOfWeek dayOfWeek)
    {
      return date.AddDays(Math.Abs(dayOfWeek - date.DayOfWeek));
    }

    public static DateTime FirstDayOfWeekBefore(this DateTime date, DayOfWeek dayOfWeek)
    {
      return date.AddDays(-Math.Abs(dayOfWeek - date.DayOfWeek));
    }
  }

  public static class PropertyInfos
  {
    public static PropertyDescriptor PropertyDescriptor(this PropertyInfo propertyInfo)
    {
      return TypeDescriptor.GetProperties(propertyInfo.DeclaringType)[propertyInfo.Name];
    }
  }
}
