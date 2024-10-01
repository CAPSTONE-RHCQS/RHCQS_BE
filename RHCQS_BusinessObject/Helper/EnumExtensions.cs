using System;
using System.ComponentModel;
using System.Reflection;
using static RHCQS_BusinessObjects.AppConstant;
using TypeAlias = RHCQS_BusinessObjects.AppConstant.Type;


namespace RHCQS_BusinessObject.Helper
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    public static class DesignDrawingExtensions
    {
        public static string ToTypeString(this DesignDrawing designDrawing)
        {
            return designDrawing switch
            {
                DesignDrawing.Perspective => TypeAlias.PHOICANH,
                DesignDrawing.Architecture => TypeAlias.KIENTRUC,
                DesignDrawing.Structure => TypeAlias.KETCAU,
                DesignDrawing.ElectricityWater => TypeAlias.DIENNUOC,
                _ => throw new ArgumentOutOfRangeException(nameof(designDrawing), designDrawing, null)
            };
        }
    }
}
