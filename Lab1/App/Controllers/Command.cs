using System.ComponentModel;
using System.Reflection;

namespace Obj_OrientedProg.Lab1.App.Controllers;


public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        Type type = value.GetType();

        string? name = Enum.GetName(type, value);
        if (name is null)
            return value.ToString();

        FieldInfo? field = type.GetField(name);
        if (field is null)
            return value.ToString();
        
        DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? name;
    }
}

public enum Command
{
    [Description("Подойти к автомату")]
    S,  // start
    [Description("Сменить пользователя")]
    CU, // change user 
    [Description("Перейти в режим пользователя")]
    TC, // to customer
    [Description("Перейти в режим администратора")]
    TA, // to admin
    [Description("Посмотреть информацию о пользователе")]
    UI, // user info
    [Description("Посмотреть информацию об автомате")]
    MI, // machine info
    [Description("Положить деньги в кошелек")]
    GS, // get salary
    [Description("Положить деньги в автомат")]
    PC, // put coins
    [Description("Купить продукт")]
    BP, // buy product
    [Description("Положить продукты")]
    PP, // put products
    [Description("Изменить цену на товар")]
    CP, // change price 
    [Description("Вернуть внесенные средства")]
    RM, // return money
    [Description("Забрать выручку")]
    TM, // take money
    [Description("Выход")]
    E   // exit
}